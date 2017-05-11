using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using MatrixSDK.Structures;
using MatrixSDK.Client;
using Newtonsoft.Json;
namespace MatrixSDK.AppService
{
	struct ASEventBatch{
		public MatrixEvent[] events;
	}

	public delegate void AliasRequestDelegate(string alias,out bool roomExists);
	public delegate void UserRequestDelegate(string userid,out bool userExists);
	public delegate void EventDelegate(MatrixEvent ev);
	public class MatrixAppservice
	{
		
		const int DEFAULT_MAXREQUESTS = 64;
		public readonly int Port;
		public readonly string AsUrl;
		public readonly string HsUrl;
		public readonly int MaximumRequests;
		public readonly string Domain;

		public event AliasRequestDelegate OnAliasRequest;
		public event EventDelegate OnEvent;
		public event UserRequestDelegate OnUserRequest;

		public readonly ServiceRegistration Registration;

		private Semaphore accept_semaphore;
		private HttpListener listener;
		private readonly Regex urlMatcher;
		private MatrixAPI api;
		private string botuser_id;

		public MatrixAppservice (ServiceRegistration registration, string domain, string url = "http://localhost",int maxrequests = DEFAULT_MAXREQUESTS)
		{
			HsUrl = url;
			Domain = domain;
			MaximumRequests = maxrequests;
			Registration = registration;
			AsUrl = registration.URL;
			botuser_id = "@"+ registration.Localpart + ":"+Domain;
			urlMatcher = new Regex ("\\/(rooms|transactions|users)\\/(.+)\\?access_token=(.+)", RegexOptions.Compiled | RegexOptions.ECMAScript);

			api = new MatrixAPI (url,registration.AppServiceToken,null);

		}

		public void Run(){
			listener = new HttpListener ();
			listener.Prefixes.Add (AsUrl+"/rooms/");
			listener.Prefixes.Add (AsUrl+"/transactions/");
			listener.Prefixes.Add (AsUrl+"/users/");
			listener.Start ();
			accept_semaphore = new Semaphore (MaximumRequests, MaximumRequests);
			while(listener.IsListening){
				accept_semaphore.WaitOne ();
				listener.GetContextAsync ().ContinueWith (OnContext);
			}
		}

		public MatrixClient GetClientAsUser (string user = null)
		{
			if (user != null) {
				if (user.EndsWith (":" + Domain)) {
					user = user.Substring(0,user.LastIndexOf(':'));
				}
				if (user.StartsWith ("@")) {
					user = user.Substring(1);
				}
				CheckAndPerformRegistration (user);
				user = "@" + user;
				user = user + ":" + Domain;
			} else {
				user = botuser_id;
			}

			return new MatrixClient(HsUrl,Registration.AppServiceToken,user);
		}

		private void CheckAndPerformRegistration (string user)
		{
			MatrixProfile profile = api.ClientProfile ("@"+user+":"+Domain);
			if (profile == null) {
				api.RegisterUserAsAS(user);
			}
		}

		private async void OnContext (Task<HttpListenerContext> task)
		{
			await task;
			HttpListenerContext context = task.Result;
			Match match = urlMatcher.Match (context.Request.RawUrl);
			if (match.Groups.Count != 4) {
				context.Response.StatusCode = (int)HttpStatusCode.BadRequest; //Invalid response
				context.Response.Close ();
				accept_semaphore.Release ();
				return;
			}

			if (match.Groups [3].Value != Registration.HomeserverToken) {
				context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
				context.Response.Close ();
				accept_semaphore.Release ();
			}

			string type = match.Groups [1].Value;
			context.Response.StatusCode = (int)HttpStatusCode.OK;

			//Check methods
			switch (type) {
				case "users":
				case "rooms":
					if (context.Request.HttpMethod != "GET")
						context.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
					break;
				case "transactions":
					if (context.Request.HttpMethod != "PUT")
						context.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
					break;
			}

			if (context.Response.StatusCode == (int)HttpStatusCode.OK) {
				if (OnAliasRequest != null && type == "rooms") {
					context.Response.StatusCode = (int)HttpStatusCode.NotFound;
					bool exists;
					string alias = Uri.UnescapeDataString(match.Groups [2].Value);
					OnAliasRequest.Invoke (alias, out exists);
					if (exists) {
						context.Response.StatusCode = 200;
					}
				} else if (OnEvent != null && type == "transactions") {
					byte[] data = new byte[context.Request.ContentLength64];
					context.Request.InputStream.Read (data, 0, data.Length);
					ASEventBatch batch = JsonConvert.DeserializeObject<ASEventBatch> (System.Text.Encoding.UTF8.GetString (data), new JSONEventConverter ());
					foreach (MatrixEvent ev in batch.events) {
						OnEvent.Invoke (ev);
					}
				} else if (OnUserRequest != null && type == "users") {
					bool exists;
					string user = Uri.UnescapeDataString(match.Groups [2].Value);
					OnUserRequest.Invoke (user, out exists);
					context.Response.StatusCode = (int)HttpStatusCode.NotFound;
					if (exists) {
						context.Response.StatusCode = 200;
					}
				}
			}

			context.Response.OutputStream.Write(new byte[2]{123,125},0,2);//{}
			context.Response.Close ();
			accept_semaphore.Release ();
		}
	}
}


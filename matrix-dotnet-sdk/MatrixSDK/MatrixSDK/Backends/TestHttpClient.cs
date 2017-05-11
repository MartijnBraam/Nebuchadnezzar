using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
namespace MatrixSDK.Backends
{
	public class TestHttpClient : HttpClient
	{
		public TestHttpClient ()
		{
			
		}

		public override System.Threading.Tasks.Task<HttpResponseMessage> SendAsync (HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
		{
			HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.NotFound);
			return Task.FromResult<HttpResponseMessage> (response);
		}
	}
}


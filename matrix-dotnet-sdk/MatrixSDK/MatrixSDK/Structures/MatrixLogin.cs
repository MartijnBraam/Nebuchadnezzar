using System;
using Newtonsoft.Json.Linq;
namespace MatrixSDK.Structures
{
	public abstract class MatrixLogin
	{
		
	}

	/// <summary>
	/// Following http://matrix.org/docs/spec/r0.0.1/client_server.html#id67
	/// </summary>
	public class MatrixLoginPassword : MatrixLogin{
		public MatrixLoginPassword(string user,string pass){
			this.user = user;
			password = pass;
		}
		public readonly string type = "m.login.password";
		public readonly string user;
		public readonly string password;
	}

	public class MatrixLoginToken : MatrixLogin {
		public MatrixLoginToken(string user,string token){
			this.user = user;
			this.token = token;
		}
		public readonly string user;
		public readonly string token;
		public readonly string txn_id = Guid.NewGuid().ToString();
		public readonly string type = "m.login.token";
	}

	/// <summary>
	/// Following http://matrix.org/docs/spec/r0.0.1/client_server.html#id76
	/// </summary>
	public class MatrixLoginResponse{
		public string access_token;
		public string home_server;
		public string user_id;
		public string refresh_token;
	}
}


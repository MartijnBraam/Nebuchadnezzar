using System;

namespace MatrixSDK.Structures
{
	/// <summary>
	/// Following https://matrix.org/docs/spec/r0.0.1/client_server.html#m-typing
	/// </summary>
	public class MatrixMTyping : MatrixEventContent {
		public string[] user_ids {get;set;}
	}
}


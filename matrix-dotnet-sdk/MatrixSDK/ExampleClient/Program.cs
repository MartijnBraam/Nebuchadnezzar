using System;
using MatrixSDK;
using MatrixSDK.Client;
namespace ExampleClient
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			MatrixClient client = new MatrixClient ("");
			client.LoginWithPassword ("", "");
			MatrixUser user = client.GetUser ("");

		}
	}
}

using System;
using MatrixSDK.Client;
using MatrixSDK.Exceptions;
using NUnit.Framework;
namespace MatrixSDKTest
{
	[TestFixture]
	class Client
	{
		[Test]
		public void CreateClient ()
		{
			MatrixClient client;
			Assert.Throws<MatrixUnsuccessfulConnection>(() => { client = new MatrixClient();});
		}
	}
}

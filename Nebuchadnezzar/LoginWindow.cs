using System;
using MatrixSDK;
using MatrixSDK.Client;

namespace Nebuchadnezzar
{
	public partial class LoginWindow : Gtk.Window
	{
		public LoginWindow () :
			base (Gtk.WindowType.Toplevel)
		{
			this.Build ();
		}

		protected void onLogin (object sender, EventArgs e)
		{
			Console.WriteLine ("Trying to login to the matrix server...");
			var client = new MatrixClient (this.serverEntry.Text);
			try{
				client.LoginWithPassword (this.usernameEntry.Text, this.passwordEntry.Text);
				var user = client.GetUser();
				Console.WriteLine("Login success");
				var storage = new Storage();
				storage.Server = this.serverEntry.Text;
				storage.Username = this.usernameEntry.Text;
				storage.UserId = user.UserID;
				storage.Token = client.GetAccessToken();
				storage.Save();
			}catch(Exception loginException){
				Console.WriteLine (loginException.Message);
			}
		}
	}
}


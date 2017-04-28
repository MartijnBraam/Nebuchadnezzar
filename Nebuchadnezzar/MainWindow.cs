using System;
using Gtk;
using MatrixSDK;
using MatrixSDK.Client;
using Nebuchadnezzar;
using System.Threading;


public partial class MainWindow: Gtk.Window
{
	public MatrixClient client;
	public MatrixUser user;

	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build ();
		var statusContext = statusbar.GetContextId ("matrix");
		statusbar.Push (statusContext, "Logging in to matrix server...");
		Thread thr = new Thread (new ThreadStart (MatrixRoutine));
		thr.Start ();
	}

	public void MatrixRoutine(){
		var storage = new Storage ();

		client = new MatrixClient (storage.Server);

		try{
			client.UseExistingToken (storage.UserId, storage.Token);
			this.user = client.GetUser();
			Gtk.Application.Invoke (delegate {
				var statusContext = statusbar.GetContextId ("matrix");
				statusbar.Push (statusContext, "Logged in to "+storage.Server+" as "+storage.UserId);
				displayName.Text = user.DisplayName;
			});

		}catch(Exception loginException){
			
			Gtk.Application.Invoke (delegate {
				var statusContext = statusbar.GetContextId ("matrix");
				statusbar.Push (statusContext, "Login failed: "+loginException.Message);
			});

			storage.Token = null;
			storage.Save ();
		}

		var rooms = client.GetAllRooms ();

		var column = new TreeViewColumn ();
		column.Title = "Service";
		channelList.AppendColumn (column);
		var liststore = new ListStore (typeof(string));
		channelList.Model = liststore;

		var serviceCellRenderer = new CellRendererText ();
		column.PackStart (serviceCellRenderer, true);
		column.AddAttribute (serviceCellRenderer, "text", 0);

		foreach (var channel in rooms) {
			if (channel.Name == null) {
				if (channel.Members.Count == 2) {
					var channelName = "";
					foreach (var memberId in channel.Members.Keys) {
						if (memberId != storage.UserId) {
							var other = channel.Members [memberId];
							if (other.displayname != null) {
								channelName = other.displayname;
							} else {
								channelName = memberId;
							}
						}
					}
					liststore.AppendValues (channelName);
				}
			} else {
				liststore.AppendValues (channel.Name);
			}
		}

		Console.WriteLine ("Matrix thread done");
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		Environment.Exit (0);
		a.RetVal = true;
	}
}

using System;
using Gtk;
using MatrixSDK;
using MatrixSDK.Client;
using Nebuchadnezzar;
using System.Threading;
using System.Collections.Generic;
using MatrixSDK.Structures;
using Notifications;


public partial class MainWindow: Gtk.Window
{
	public MatrixClient client;
	public MatrixUser user;
	public MatrixRoom currentRoom;

	public Dictionary<int, MatrixRoom> rooms = new Dictionary<int, MatrixRoom>();
	public Dictionary<string, MatrixMRoomMember> users = new Dictionary<string, MatrixMRoomMember>();

	public Dictionary<string, System.Drawing.Image> avatars = new Dictionary<string, System.Drawing.Image>();

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

		channelList.AppendColumn("Icon", new Gtk.CellRendererPixbuf (), "pixbuf", 0);
		channelList.AppendColumn ("Artist", new Gtk.CellRendererText (), "text", 1);


		var liststore = new ListStore (typeof(Gdk.Pixbuf) ,typeof(string));

		int roomIndex = 0;
		foreach (var channel in rooms) {
			var label = getRoomLabel (channel);
			var icon = AvatarGenerator.createRoomAvatar (label, channel.Encryption != null);
			liststore.AppendValues (icon, label);
			this.rooms.Add (roomIndex, channel);
			roomIndex++;

			foreach (var member in channel.Members) {
				if (!this.users.ContainsKey (member.Key)) {
					this.users.Add (member.Key, member.Value);
				}
			}
		}

		channelList.Model = liststore;
		var avatar = client.DownloadMatrixContent (user.AvatarURL);
		var avatarScaled = Utils.resizeImage (System.Drawing.Image.FromStream(avatar), 48, 48);
		profileImage.Pixbuf = Utils.bitmapToPixbuf (avatarScaled);

		foreach (var member in this.users) {
			if (member.Value.avatar_url != null) {
				var memberAvatar = client.DownloadMatrixContent (member.Value.avatar_url);
				var scaled = Utils.resizeImage (System.Drawing.Image.FromStream (memberAvatar), 32, 32);
				this.avatars.Add (member.Key, scaled);
			}
		}
		refreshRoomList ();
		Console.WriteLine ("Matrix thread done");
	}

	private string getRoomLabel(MatrixRoom room){
		if (room.Name != null) {
			return room.Name;
		}

		var members = new List<string> ();
		foreach (var memberId in room.Members.Keys) {
			if (memberId != this.user.UserID) {
				var other = room.Members [memberId];
				if (other.displayname != null) {
					members.Add (other.displayname);
				} else {
					members.Add (memberId);
				}
			}
		}

		return string.Join (", ", members.ToArray ());
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		Environment.Exit (0);
		a.RetVal = true;
	}

	protected void OnChannelListCursorChanged (object sender, EventArgs e)
	{
		if (channelList.Selection.CountSelectedRows () != 1) {
			return;
		}
		var paths = channelList.Selection.GetSelectedRows ();
		var index = paths [0].Indices [0];
		loadRoom (this.rooms [index]);
	}

	protected void refreshRoomList(){
		var liststore = new ListStore (typeof(Gdk.Pixbuf) ,typeof(string));
		foreach (var channel in this.rooms.Values) {
			var label = getRoomLabel (channel);
			System.Drawing.Image avatar = null;

			if (channel.Members.Count == 2) {
				var other = getOtherRoomMember (channel);
				if (this.avatars.ContainsKey (other)) {
					avatar = this.avatars [other];
				}
			}

			var icon = AvatarGenerator.createRoomAvatar (label, channel.Encryption != null, avatar);
			liststore.AppendValues (icon, label);

			channel.OnMessage -= onRoomMessage;
			channel.OnMessage += onRoomMessage;

		}

		channelList.Model = liststore;
	}

	private string getOtherRoomMember(MatrixRoom room){
		foreach (var member in room.Members) {
			if (member.Key != this.user.UserID) {
				return member.Key;
			}
		}
		throw new Exception ("Room without other member");
	}

	protected void onRoomMessage(MatrixRoom sender, MatrixEvent e){
		if (sender == currentRoom) {
			Console.WriteLine ("Got message in current room");
			Gtk.Application.Invoke (delegate {
				loadRoom (currentRoom);
			});
		} else {
			Console.WriteLine ("Got message for another room");
			var notification = new Notification ();
			notification.Summary = getRoomLabel(sender);
			notification.Body = e.content.ToString ();
			notification.IconName = "dialog-information";
			notification.Show ();
		}
	}

	protected void loadRoom(MatrixRoom room){
		this.currentRoom = room;

		foreach (var widget in chatBox.Children) {
			chatBox.Remove (widget);
		}

		foreach (var message in room.Messages) {
			Widget messageContents = null;
			if (message.msgtype == "m.text") {
				messageContents = new Label (message.body);
				((Label)messageContents).Justify = Justification.Left;
			}
			if (message.msgtype == "m.image") {
				messageContents = new VBox ();
				var imageLabel = new Label (message.body);
				imageLabel.Justify = Justification.Left;

				var loader = new AsyncImageLoader ((MMessageImage)message, this.client);
				var imageContents = loader.GetImageWidget (250, 200);

				((VBox)messageContents).PackStart (imageLabel);
				((VBox)messageContents).PackStart (imageContents);
			}
			if (messageContents != null) {
				var messageContainer = new HBox ();
				string senderName;

				senderName = this.users [message.sender].displayname;
				if (senderName == null) {
					senderName = message.sender;
				}
				if (this.avatars.ContainsKey (message.sender)) {
					var senderIcon = new System.Drawing.Bitmap(this.avatars [message.sender]);
					messageContainer.PackStart (new Gtk.Image (Utils.bitmapToPixbuf(senderIcon)), false, false, 6);
				} else {
					var senderIcon = AvatarGenerator.createRoomAvatar (senderName);
					messageContainer.PackStart (new Gtk.Image (senderIcon), false, false, 6);
				}
				messageContainer.PackStart (messageContents, false, false,6);
				chatBox.PackStart (messageContainer, false, false, 0);
			}

			Console.WriteLine (message.msgtype);
		}
		chatBox.ShowAll ();

	}

	protected void OnChatEntryActivated (object sender, EventArgs e)
	{
		var message = this.chatEntry.Text;
		this.chatEntry.Text = "";

		if (this.currentRoom != null) {
			this.currentRoom.SendMessage (message);
		}
	}

	protected void OnChatBoxSizeAllocated (object o, SizeAllocatedArgs args)
	{
		var adj = this.chatScroller.Vadjustment;
		this.chatScroller.Vadjustment.Value = adj.Upper - adj.PageSize;
	}
}

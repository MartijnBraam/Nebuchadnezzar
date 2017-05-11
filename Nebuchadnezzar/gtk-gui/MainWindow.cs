
// This file has been generated by the GUI designer. Do not modify.

public partial class MainWindow
{
	private global::Gtk.UIManager UIManager;
	
	private global::Gtk.Action ClientAction;
	
	private global::Gtk.Action DisconnectAction;
	
	private global::Gtk.VBox vbox2;
	
	private global::Gtk.MenuBar menubar1;
	
	private global::Gtk.HPaned hpaned1;
	
	private global::Gtk.VBox vbox3;
	
	private global::Gtk.HBox hbox1;
	
	private global::Gtk.Image profileImage;
	
	private global::Gtk.Label displayName;
	
	private global::Gtk.ScrolledWindow GtkScrolledWindow;
	
	private global::Gtk.TreeView channelList;
	
	private global::Gtk.VBox vbox4;
	
	private global::Gtk.ScrolledWindow chatScroller;
	
	private global::Gtk.VBox chatBox;
	
	private global::Gtk.Entry chatEntry;
	
	private global::Gtk.Statusbar statusbar;

	protected virtual void Build ()
	{
		global::Stetic.Gui.Initialize (this);
		// Widget MainWindow
		this.UIManager = new global::Gtk.UIManager ();
		global::Gtk.ActionGroup w1 = new global::Gtk.ActionGroup ("Default");
		this.ClientAction = new global::Gtk.Action ("ClientAction", global::Mono.Unix.Catalog.GetString ("Client"), null, null);
		this.ClientAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Client");
		w1.Add (this.ClientAction, null);
		this.DisconnectAction = new global::Gtk.Action ("DisconnectAction", global::Mono.Unix.Catalog.GetString ("Disconnect"), null, null);
		this.DisconnectAction.ShortLabel = global::Mono.Unix.Catalog.GetString ("Disconnect");
		w1.Add (this.DisconnectAction, null);
		this.UIManager.InsertActionGroup (w1, 0);
		this.AddAccelGroup (this.UIManager.AccelGroup);
		this.Name = "MainWindow";
		this.Title = global::Mono.Unix.Catalog.GetString ("Nebuchadnezzar");
		this.WindowPosition = ((global::Gtk.WindowPosition)(4));
		// Container child MainWindow.Gtk.Container+ContainerChild
		this.vbox2 = new global::Gtk.VBox ();
		this.vbox2.Name = "vbox2";
		this.vbox2.Spacing = 6;
		// Container child vbox2.Gtk.Box+BoxChild
		this.UIManager.AddUiFromString ("<ui><menubar name='menubar1'><menu name='ClientAction' action='ClientAction'><menuitem name='DisconnectAction' action='DisconnectAction'/></menu></menubar></ui>");
		this.menubar1 = ((global::Gtk.MenuBar)(this.UIManager.GetWidget ("/menubar1")));
		this.menubar1.Name = "menubar1";
		this.vbox2.Add (this.menubar1);
		global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.menubar1]));
		w2.Position = 0;
		w2.Expand = false;
		w2.Fill = false;
		// Container child vbox2.Gtk.Box+BoxChild
		this.hpaned1 = new global::Gtk.HPaned ();
		this.hpaned1.CanFocus = true;
		this.hpaned1.Name = "hpaned1";
		this.hpaned1.Position = 201;
		// Container child hpaned1.Gtk.Paned+PanedChild
		this.vbox3 = new global::Gtk.VBox ();
		this.vbox3.Name = "vbox3";
		this.vbox3.Spacing = 6;
		this.vbox3.BorderWidth = ((uint)(6));
		// Container child vbox3.Gtk.Box+BoxChild
		this.hbox1 = new global::Gtk.HBox ();
		this.hbox1.Name = "hbox1";
		this.hbox1.Spacing = 6;
		// Container child hbox1.Gtk.Box+BoxChild
		this.profileImage = new global::Gtk.Image ();
		this.profileImage.Name = "profileImage";
		this.profileImage.Pixbuf = global::Gdk.Pixbuf.LoadFromResource ("Nebuchadnezzar.Resources.Generic_Avatar.png");
		this.hbox1.Add (this.profileImage);
		global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.profileImage]));
		w3.Position = 0;
		w3.Expand = false;
		w3.Fill = false;
		// Container child hbox1.Gtk.Box+BoxChild
		this.displayName = new global::Gtk.Label ();
		this.displayName.Name = "displayName";
		this.displayName.LabelProp = global::Mono.Unix.Catalog.GetString ("...");
		this.hbox1.Add (this.displayName);
		global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.displayName]));
		w4.Position = 1;
		this.vbox3.Add (this.hbox1);
		global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.hbox1]));
		w5.Position = 0;
		w5.Expand = false;
		w5.Fill = false;
		// Container child vbox3.Gtk.Box+BoxChild
		this.GtkScrolledWindow = new global::Gtk.ScrolledWindow ();
		this.GtkScrolledWindow.Name = "GtkScrolledWindow";
		this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
		// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
		this.channelList = new global::Gtk.TreeView ();
		this.channelList.CanFocus = true;
		this.channelList.Name = "channelList";
		this.channelList.HeadersVisible = false;
		this.GtkScrolledWindow.Add (this.channelList);
		this.vbox3.Add (this.GtkScrolledWindow);
		global::Gtk.Box.BoxChild w7 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.GtkScrolledWindow]));
		w7.Position = 1;
		this.hpaned1.Add (this.vbox3);
		global::Gtk.Paned.PanedChild w8 = ((global::Gtk.Paned.PanedChild)(this.hpaned1 [this.vbox3]));
		w8.Resize = false;
		// Container child hpaned1.Gtk.Paned+PanedChild
		this.vbox4 = new global::Gtk.VBox ();
		this.vbox4.Name = "vbox4";
		this.vbox4.Spacing = 6;
		this.vbox4.BorderWidth = ((uint)(6));
		// Container child vbox4.Gtk.Box+BoxChild
		this.chatScroller = new global::Gtk.ScrolledWindow ();
		this.chatScroller.CanFocus = true;
		this.chatScroller.Name = "chatScroller";
		this.chatScroller.VscrollbarPolicy = ((global::Gtk.PolicyType)(0));
		this.chatScroller.HscrollbarPolicy = ((global::Gtk.PolicyType)(2));
		this.chatScroller.ShadowType = ((global::Gtk.ShadowType)(1));
		// Container child chatScroller.Gtk.Container+ContainerChild
		global::Gtk.Viewport w9 = new global::Gtk.Viewport ();
		w9.ShadowType = ((global::Gtk.ShadowType)(0));
		// Container child GtkViewport.Gtk.Container+ContainerChild
		this.chatBox = new global::Gtk.VBox ();
		this.chatBox.Name = "chatBox";
		this.chatBox.Spacing = 6;
		w9.Add (this.chatBox);
		this.chatScroller.Add (w9);
		this.vbox4.Add (this.chatScroller);
		global::Gtk.Box.BoxChild w12 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.chatScroller]));
		w12.Position = 0;
		// Container child vbox4.Gtk.Box+BoxChild
		this.chatEntry = new global::Gtk.Entry ();
		this.chatEntry.CanFocus = true;
		this.chatEntry.Name = "chatEntry";
		this.chatEntry.IsEditable = true;
		this.chatEntry.InvisibleChar = '●';
		this.vbox4.Add (this.chatEntry);
		global::Gtk.Box.BoxChild w13 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.chatEntry]));
		w13.Position = 2;
		w13.Expand = false;
		w13.Fill = false;
		this.hpaned1.Add (this.vbox4);
		this.vbox2.Add (this.hpaned1);
		global::Gtk.Box.BoxChild w15 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.hpaned1]));
		w15.Position = 1;
		// Container child vbox2.Gtk.Box+BoxChild
		this.statusbar = new global::Gtk.Statusbar ();
		this.statusbar.Name = "statusbar";
		this.statusbar.Spacing = 6;
		this.vbox2.Add (this.statusbar);
		global::Gtk.Box.BoxChild w16 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.statusbar]));
		w16.Position = 2;
		w16.Expand = false;
		w16.Fill = false;
		this.Add (this.vbox2);
		if ((this.Child != null)) {
			this.Child.ShowAll ();
		}
		this.DefaultWidth = 994;
		this.DefaultHeight = 627;
		this.Show ();
		this.DeleteEvent += new global::Gtk.DeleteEventHandler (this.OnDeleteEvent);
		this.channelList.CursorChanged += new global::System.EventHandler (this.OnChannelListCursorChanged);
		this.chatBox.SizeAllocated += new global::Gtk.SizeAllocatedHandler (this.OnChatBoxSizeAllocated);
		this.chatEntry.Activated += new global::System.EventHandler (this.OnChatEntryActivated);
	}
}

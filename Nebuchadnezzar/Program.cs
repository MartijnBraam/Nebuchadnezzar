using System;
using Gtk;

namespace Nebuchadnezzar
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Application.Init ();
			var storage = new Storage ();
			if (storage.Token == null || storage.Token == "") {
				LoginWindow lwin = new LoginWindow ();
				lwin.Show ();
			} else {
				MainWindow win = new MainWindow ();
				win.Show ();
			}
			Application.Run ();
		}
	}
}

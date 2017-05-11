using System;
using System.IO;
using System.Collections.Generic;
using MatrixSDK.AppService;
using MatrixSDK.Client;
namespace EchoAppservice
{
	class MainClass
	{
		static MatrixAppservice appservice;
		public static void Main (string[] args)
		{
			ServiceRegistration registration;
			string regfile;
			try {
				regfile = Path.GetFullPath (args.Length >= 1 ? args [0] : Environment.GetEnvironmentVariable ("APPSERVICE_REG"));
			} catch (ArgumentNullException e) {
				Console.Error.WriteLine("You need to provide either an argument or set APPSERVICE_REG to your registration file");
				Environment.Exit (1);
				return;
			}

			if (!File.Exists (regfile)) {
				Console.WriteLine ("Config does not exist at " + regfile);
				Console.WriteLine ("Writing new config");
				List<AppServiceNamespace> users = new List<AppServiceNamespace> ();
				List<AppServiceNamespace> aliases = new List<AppServiceNamespace> ();
				List<AppServiceNamespace> rooms = new List<AppServiceNamespace> ();
				registration = new ServiceRegistration ("FILL ME", "FILL ME", users, aliases, rooms);
				string yaml = registration.ToYAML ();
				try {
					File.WriteAllText (regfile, yaml);
				} catch (IOException e) {
					Console.Error.WriteLine ("Couldn't write new config file\n"+e.Message);
					Environment.Exit (2);
					return;
				}
			} else {
				try {
					registration = ServiceRegistration.FromYAML(File.ReadAllText(regfile));
				} catch (IOException e) {
					Console.Error.WriteLine ("Couldn't read config file\n"+e.Message);
					Environment.Exit (2);
					return;
				}
			}

			appservice = new MatrixAppservice (registration,"localhost","FILL ME");

			appservice.OnEvent += Appservice_OnEvent;
			appservice.OnAliasRequest += Appservice_OnAliasRequest;
			appservice.OnUserRequest += Appservice_OnUserRequest;
			appservice.Run ();

		}


		static void Appservice_OnEvent (MatrixSDK.Structures.MatrixEvent ev)
		{
			Console.WriteLine("Event recieved ("+ev.type+")");
			ev.ToString();
		}

		static void Appservice_OnUserRequest (string userid, out bool userExists)
		{
			Console.WriteLine("Got a request for " + userid);
			userExists = false;
		}

		static void Appservice_OnAliasRequest (string alias, out bool roomExists)
		{
			Console.WriteLine("Got a request for " + alias);
			MatrixClient bot = appservice.GetClientAsUser();
			string localpart = alias.Substring(1,alias.IndexOf(':')-1);
			MatrixRoom room = bot.CreateRoom(
				new MatrixSDK.Structures.MatrixCreateRoom(){
					room_alias_name = localpart,
					preset = MatrixSDK.Structures.EMatrixCreateRoomPreset.public_chat
				}
			);
			roomExists = true;
		}
	}
}

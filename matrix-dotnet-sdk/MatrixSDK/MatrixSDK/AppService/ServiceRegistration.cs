using System;
using System.Text;
using System.Collections.Generic;
using YamlDotNet.Serialization;
namespace MatrixSDK.AppService
{
	public struct AppServiceNamespace{
		public bool exclusive  { get; set; }
		public string regex  { get; set; }
	}

	public struct ServiceRegistrationOptionsNamespaces{
		public List<AppServiceNamespace> users  { get; set; }
		public List<AppServiceNamespace> aliases  { get; set; }
		public List<AppServiceNamespace> rooms  { get; set; }
	}

	public struct ServiceRegistrationOptions{
		public string id  { get; set; }
		public string url  { get; set; }
		public string as_token  { get; set; }
		public string hs_token  { get; set; }
		public string sender_localpart  { get; set; }
		public ServiceRegistrationOptionsNamespaces namespaces  { get; set; }
	}

	public class ServiceRegistration
	{

		public string URL {
			get;
			private set;
		}

		public string Localpart {
			get;
			private set;
		}

		public string ID {
			get;
			private set;
		} 

		public string HomeserverToken {
			get;
			private set;
		}

		public string AppServiceToken {
			get;
			private set;
		}

		public ICollection<AppServiceNamespace> NamespacesUsers {
			get;
			private set;
		}

		public ICollection<AppServiceNamespace> NamespacesAliases  {
			get;
			private set;
		}

		public ICollection<AppServiceNamespace> NamespacesRooms  {
			get;
			private set;
		}


		public ServiceRegistration(ServiceRegistrationOptions options){
			URL = options.url;
			Localpart = options.sender_localpart;
			ID = options.id;
			HomeserverToken = options.hs_token;
			AppServiceToken = options.as_token;
			NamespacesAliases = options.namespaces.aliases;
			NamespacesUsers = options.namespaces.users;
			NamespacesRooms = options.namespaces.rooms;
		}

		public ServiceRegistration(
			string url,
			string localpart,
			ICollection<AppServiceNamespace> users,
			ICollection<AppServiceNamespace> aliases,
			ICollection<AppServiceNamespace> rooms
		 ){
			this.URL = url;
			this.Localpart = localpart;
			this.NamespacesUsers = users;
			this.NamespacesAliases = aliases;
			this.NamespacesRooms = rooms;

			this.ID = GenerateToken();
			this.HomeserverToken = GenerateToken();
			this.AppServiceToken = GenerateToken();
		}

		public static string GenerateToken ()
		{
			return (Guid.NewGuid().ToString() + Guid.NewGuid().ToString()).Replace("-","");
		}

		public static ServiceRegistration FromYAML(string yaml){
			Deserializer serial = new Deserializer();
			ServiceRegistrationOptions opts =serial.Deserialize<ServiceRegistrationOptions>(new System.IO.StringReader(yaml));
			return new ServiceRegistration(opts);
		}

		public string ToYAML(){
			Serializer serial = new Serializer();
			System.IO.StringWriter writer = new System.IO.StringWriter();
			serial.Serialize(writer,new {
				id = ID,
				url = URL,
				as_token = AppServiceToken,
				hs_token = HomeserverToken,
				sender_localpart = Localpart,
				namespaces = new {
					users = NamespacesUsers,
					aliases = NamespacesAliases,
					rooms = NamespacesRooms
				}
			});
			return writer.ToString();
		}
	}
}


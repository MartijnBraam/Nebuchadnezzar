using System;
using System.Configuration;

namespace Nebuchadnezzar
{
	public class Storage : ApplicationSettingsBase
	{
		[UserScopedSettingAttribute()]
		public String Server
		{
			get { return (String)this["Server"]; }
			set { this["Server"] = value; }
		}

		[UserScopedSettingAttribute()]
		public String Username
		{
			get { return (String)this["Username"]; }
			set { this["Username"] = value; }
		}

		[UserScopedSettingAttribute()]
		public String UserId
		{
			get { return (String)this["UserId"]; }
			set { this["UserId"] = value; }
		}

		[UserScopedSettingAttribute()]
		public String Token
		{
			get { return (String)this["Token"]; }
			set { this["Token"] = value; }
		}
	}
}


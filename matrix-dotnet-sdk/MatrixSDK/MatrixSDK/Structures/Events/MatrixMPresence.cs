using System;

namespace MatrixSDK.Structures
{
	public enum EMatrixPresence{
		Online,
		Offline,
		Unavailable,
		FreeForChat,
		Hidden
	}

	public class MatrixMPresence : MatrixEventContent
	{
		public string user_id;
		public Int64 last_active_ago;
		public string avatar_url;
		public string displayname;
		public EMatrixPresence presence;
	}
}


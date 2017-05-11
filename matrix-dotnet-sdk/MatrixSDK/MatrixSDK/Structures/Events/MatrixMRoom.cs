using System;

namespace MatrixSDK.Structures
{
	public enum EMatrixRoomJoinRules{
		Public,
		Knock,
		Invite,
		Private
	}

	public enum EMatrixRoomHistoryVisibility{
		Invited,
		Joined,
		Shared,
		World_Readable
	}

	public class MatrixRoomStateEvent : MatrixEventContent{

	}

	public class MatrixMRoomAliases : MatrixRoomStateEvent
	{
		public string[] aliases;
	}

	public class MatrixMRoomCanonicalAlias : MatrixRoomStateEvent{
		public string alias;
	}

	public class MatrixMRoomCreate : MatrixRoomStateEvent{
		public bool mfederate = true;
		public string creator;	
	}

	public class MatrixMRoomJoinRules : MatrixRoomStateEvent{
		public EMatrixRoomJoinRules join_rule;
	}

	public class MatrixMRoomName : MatrixRoomStateEvent{
		public string name;
	}

	public class MatrixMRoomEncryption : MatrixRoomStateEvent{
		public string algorithm;
	}

	public class MatrixMRoomTopic : MatrixRoomStateEvent{
		public string topic;
	}

	public class MatrixMRoomHistoryVisibility : MatrixRoomStateEvent{
		public EMatrixRoomHistoryVisibility history_visibility;
	}
}


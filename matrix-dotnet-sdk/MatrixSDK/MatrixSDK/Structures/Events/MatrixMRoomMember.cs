using System;

namespace MatrixSDK.Structures
{
	public enum EMatrixRoomMembership{
		Invite,
		Join,
		Knock,
		Leave,
		Ban
	}

	public class MatrixMRoomMember : MatrixRoomStateEvent
	{
		public MatrixInvite third_party_invite;
		public EMatrixRoomMembership membership;
		public string avatar_url;
		public string displayname;
	}
}


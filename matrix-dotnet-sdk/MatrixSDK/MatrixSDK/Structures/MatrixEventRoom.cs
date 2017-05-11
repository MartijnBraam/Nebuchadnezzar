using System;

namespace MatrixSDK.Structures
{		
	public class MatrixEventRoomLeft{
		public MatrixTimeline timeline;
		public MatrixSyncEvents state;
	}

	public class MatrixEventRoomJoined{
		public MatrixTimeline timeline;
		public MatrixSyncEvents state;
		public MatrixSyncEvents account_data;
		public MatrixSyncEvents ephemeral;
	}

	public class MatrixEventRoomInvited{
		public MatrixSyncEvents events;
	}
}


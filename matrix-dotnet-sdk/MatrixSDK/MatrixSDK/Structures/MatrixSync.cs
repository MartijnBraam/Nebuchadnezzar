using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
namespace MatrixSDK.Structures
{
	/// <summary>
	/// From http://matrix.org/docs/spec/r0.0.1/client_server.html#get-matrix-client-r0-sync
	/// </summary>
	public class MatrixSync
	{
		public string next_batch;
		public MatrixSyncEvents account_data;
		public MatrixSyncEvents presence;
		public MatrixSyncRooms rooms;
	}

	public class MatrixSyncEvents{
		public MatrixEvent[] events;
	}

	public class MatrixSyncRooms{
		public Dictionary<string,MatrixEventRoomInvited> invite;
		public Dictionary<string,MatrixEventRoomJoined> join;
		public Dictionary<string,MatrixEventRoomLeft> leave;
	}
}


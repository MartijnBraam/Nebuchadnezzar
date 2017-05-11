using System;
using System.Collections.Generic;
namespace MatrixSDK.Structures
{
	public class MatrixEvent
	{
		/// <summary>
		/// Following http://matrix.org/docs/spec/r0.0.1/client_server.html#get-matrix-client-r0-sync
		/// </summary>	
		public MatrixEventContent content;
		public Int64 origin_server_ts;
		public Int64 age;
		public string sender;
		public string type;
		public string event_id;
		public string room_id;
		public MatrixEventUnsigned unsigned;
		public string state_key;

		// Special case for https://matrix.org/docs/spec/r0.0.1/client_server.html#m-room-member
		public MatrixStrippedState[] invite_room_state;

		public MatrixEvent ()
		{

		}

		public override string ToString ()
		{
			string str = "Event {";
			foreach (System.Reflection.PropertyInfo prop in typeof(MatrixEvent).GetProperties()) {
				str += "   " + (prop.Name + ": " + prop.GetValue (this).ToString ());
			}
			str += "}";
			return str;
		}
	}

	public class MatrixEventUnsigned{
		public MatrixEventUnsigned prev_content;
		public Int64 age;
		public string transaction_id;
	}
	/// <summary>
	/// Do not use this class directly.
	/// </summary>
	public class MatrixEventContent{
		public string sender;
	}

	public class MatrixTimeline{
		public bool limited;
		public string prev_batch;
		public MatrixEvent[] events;
	}
}


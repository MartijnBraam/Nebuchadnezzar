using System;
using MatrixSDK.Exceptions;
namespace MatrixSDK.Structures
{
	public class MatrixCreateRoom
	{
		/// <summary>
		/// A list of user IDs to invite to the room. This will tell the server to invite everyone in the list to the newly created room.
		/// </summary>
		public string[] invite;

		/// <summary>
		/// If this is included, an m.room.name event will be sent into the room to indicate the name of the room. See Room Events for more information on m.room.name
		/// </summary>
		public string name;

		/// <summary>
		/// A public visibility indicates that the room will be shown in the published room list. A private visibility will hide the room from the published room list. Rooms default to private visibility if this key is not included.
		/// </summary>
		public EMatrixCreateRoomVisibility visibility = EMatrixCreateRoomVisibility.Private;

		/// <summary>
		/// If this is included, an m.room.topic event will be sent into the room to indicate the topic for the room. See Room Events for more information on m.room.topic.
		/// </summary>
		public string topic;

		/// <summary>
		/// Convenience parameter for setting various default state events based on a preset
		/// </summary>
		public EMatrixCreateRoomPreset preset = EMatrixCreateRoomPreset.private_chat;

		private string _room_alias_name;
		/// <summary>
		/// The desired room alias **local part**. If this is included, a room alias will be created and mapped to the newly created room.
		/// </summary>
		/// <value>Room alias local part.</value>
		public string room_alias_name {
			get { return _room_alias_name; }
			set
			{
				if (value.Contains ("#") || value.Contains(":")) {
					throw new MatrixBadFormatException (value, "local alias", "a local alias must not contain : or #");
				}
				_room_alias_name = value;
			}
		}


		//TODO: Add invite_3pid
		//TODO: Add creation_content
		//TODO: Add initial_state
	}

	public enum EMatrixCreateRoomVisibility
	{
		Public,
		Private
	}

	public enum EMatrixCreateRoomPreset
	{
		private_chat,
		public_chat,
		trusted_private_chat
	}
}


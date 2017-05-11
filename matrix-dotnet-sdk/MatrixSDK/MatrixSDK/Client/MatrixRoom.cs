using System;
using System.Collections.Generic;
using MatrixSDK.Structures;
using MatrixSDK.Exceptions;
namespace MatrixSDK.Client
{
	
	public delegate void MatrixRoomEventDelegate(MatrixRoom room,MatrixEvent evt);
	public delegate void MatrixRoomChangeDelegate();
	public delegate void MatrixRoomRecieptDelegate(string event_id, MatrixReceipts receipts);
	public delegate void MatrixRoomTypingDelegate(string[] user_ids);
	public delegate void MatrixRoomMemberEvent(string user_id, MatrixMRoomMember member);

	/// <summary>
	/// A room that the user has joined on Matrix.
	/// </summary>
	public class MatrixRoom
	{

		const int MESSAGE_CAPACITY = 255;

		/// <summary>
		/// The server assigned ID for the room. This can never change.
		/// </summary>
		public readonly string ID;
		public string Name { get; private set; }
		public string Topic { get; private set; }
		public string Creator { get; private set; }
		public string Encryption {get; private set; }

		public Dictionary<string,MatrixMRoomMember> Members { get; private set; }

		/// <summary>
		/// Should this Matrix Room federate with other home servers?
		/// </summary>
		/// <value><c>true</c> if should federate; otherwise, <c>false</c>.</value>
		public bool ShouldFederate { get; private set; }
		public string CanonicalAlias { get; private set; }
		public string[] Aliases { get; private set; }

		public EMatrixRoomJoinRules JoinRule { get; private set; }
		public MatrixMRoomPowerLevels PowerLevels { get; private set; }

		/// <summary>
		/// Occurs when a m.room.message is recieved. 
		/// <remarks>This will include your own messages</remarks>
		/// </summary>
		public event MatrixRoomEventDelegate OnMessage;

		public event MatrixRoomChangeDelegate OnEphemeralChanged;
		public event MatrixRoomTypingDelegate OnTypingChanged;
		public event MatrixRoomRecieptDelegate OnRecieptsRecieved;

		public event MatrixRoomMemberEvent OnUserJoined;
		public event MatrixRoomMemberEvent OnUserChange;
		public event MatrixRoomMemberEvent OnUserLeft;
		public event MatrixRoomMemberEvent OnUserInvited;
		public event MatrixRoomMemberEvent OnUserBanned;


		/// <summary>
		/// Fires when any room message is recieved.
		/// </summary>
		public event MatrixRoomEventDelegate OnEvent;

		/// <summary>
		/// Don't fire OnMessage if the message exceeds this age limit (in milliseconds). Set to -1 to ignore.
		/// </summary>
		public int MessageMaximumAge = -1;

		private List<MatrixMRoomMessage> messages = new List<MatrixMRoomMessage>(MESSAGE_CAPACITY);

		private MatrixEvent[] ephemeral;

		/// <summary>
		/// Get a list of all the messages recieved so far.
		/// <remarks>This is not a complete list for the rooms entire history</remarks>
		/// </summary>
		public MatrixMRoomMessage[] Messages { get { return messages.ToArray (); } }

		private string prev_batch;

		private MatrixAPI api;

		/// <summary>
		/// This constructor is intended for the API only.
		/// Initializes a new instance of the <see cref="MatrixSDK.Client.MatrixRoom"/> class.
		/// </summary>
		/// <param name="API">The API to send/recieve requests from</param>
		/// <param name="roomid">Roomid</param>
		public MatrixRoom (MatrixAPI API,string roomid)
		{
			ID = roomid;
			api = API;
			Members = new Dictionary<string, MatrixMRoomMember>();
		}

		/// <summary>
		/// This method is intended for the API only.
		/// If a Room recieves a new event, process it in here.
		/// </summary>
		/// <param name="evt">New event</param>
		public void FeedEvent (MatrixEvent evt)
		{
			Type t = evt.content.GetType ();
			if (t == typeof(MatrixMRoomCreate)) {
				Creator = ((MatrixMRoomCreate)evt.content).creator;
			} else if (t == typeof(MatrixMRoomName)) {
				Name = ((MatrixMRoomName)evt.content).name;
			} else if (t == typeof(MatrixMRoomTopic)) {
				Topic = ((MatrixMRoomTopic)evt.content).topic;
			} else if (t == typeof(MatrixMRoomAliases)) {
				Aliases = ((MatrixMRoomAliases)evt.content).aliases;
			} else if (t == typeof(MatrixMRoomCanonicalAlias)) {
				CanonicalAlias = ((MatrixMRoomCanonicalAlias)evt.content).alias;
			} else if (t == typeof(MatrixMRoomJoinRules)) {
				JoinRule = ((MatrixMRoomJoinRules)evt.content).join_rule;
			} else if (t == typeof(MatrixMRoomJoinRules)) {
				PowerLevels = ((MatrixMRoomPowerLevels)evt.content);
			} else if (t == typeof(MatrixMRoomEncryption)) {
				Encryption = ((MatrixMRoomEncryption)evt.content).algorithm;
			} else if (t == typeof(MatrixMRoomMember)) {
				MatrixMRoomMember member = (MatrixMRoomMember)evt.content;
				if (!api.RunningInitialSync) {
					//Handle new join,leave etc
					MatrixRoomMemberEvent Event = null;
					switch (member.membership) {
						case EMatrixRoomMembership.Invite:
							Event = OnUserInvited;
							break;
						case EMatrixRoomMembership.Join:
							Event = Members.ContainsKey (evt.state_key) ? OnUserChange : OnUserJoined;
							break;
						case EMatrixRoomMembership.Leave:
							Event = OnUserLeft;
							break;
						case EMatrixRoomMembership.Ban:
							Event = OnUserBanned;
							break;
					}
					if (Event != null) {
						Event.Invoke(evt.state_key, member);
					}
				}
				Members [evt.state_key] = member;
			} else if (t.IsSubclassOf (typeof(MatrixMRoomMessage))) {
				messages.Add ((MatrixMRoomMessage)evt.content);
				if (OnMessage != null) {
					if (MessageMaximumAge <= 0 || evt.age < MessageMaximumAge) {
						try {
							OnMessage.Invoke (this, evt);
						} catch (Exception e) {
							Console.WriteLine ("A OnMessage handler failed");
							Console.WriteLine (e);
						}
					}
				}
			}

			if (OnEvent != null) {
				OnEvent.Invoke (this, evt);
			}
		}
			
		/// <summary>
		/// Attempt to set the name of the room.
		/// This may fail if you do not have the required permissions.
		/// </summary>
		/// <param name="newName">New name.</param>
		public void SetName(string newName){
			MatrixMRoomName nameEvent = new MatrixMRoomName ();
			nameEvent.name = newName;
			api.RoomStateSend (ID, "m.room.name", nameEvent); 
		}

		/// <summary>
		/// Attempt to set the topic of the room.
		/// This may fail if you do not have the required permissions.
		/// </summary>
		/// <param name="newTopic">New topic.</param>
		public void SetTopic(string newTopic){
			MatrixMRoomTopic topicEvent = new MatrixMRoomTopic ();
			topicEvent.topic = newTopic;
			api.RoomStateSend (ID, "m.room.topic", topicEvent);
		}

		/// <summary>
		/// Send a new message to the room.
		/// </summary>
		/// <param name="message">Message.</param>
		public void SendMessage(MatrixMRoomMessage message){
			api.RoomMessageSend (ID, "m.room.message", message);
		}

		/// <summary>
		/// Send a MMessageText message to the room.
		/// </summary>
		/// <param name="body">The string body of the message</param>
		public void SendMessage(string body){
			MMessageText message = new MMessageText ();
			message.body = body;
			SendMessage (message);
		}

        public void SendNotice(string notice){
            MMessageNotice message = new MMessageNotice ();
            message.body = notice;
            SendMessage (message);
        }
        /// <summary>
        /// Sends a state message.
        /// </summary>
        /// <param name="stateMessage">State message.</param>
        /// <param name="type">Type.</param>
        /// <param name="key">Key.</param>
		public void SendState (MatrixRoomStateEvent stateMessage,string type, string key = "")
		{
			api.RoomStateSend (ID, type, stateMessage, key);
		}

		/// <summary>
		/// Sets whether the current user is typing.
		/// </summary>
		/// <param name="typing">Whether the user is typing or not. If false, the timeout key can be omitted.</param>
		/// <param name="timeout">The length of time in milliseconds to mark this user as typing.</param>
		public void SetTyping (bool typing, int timeout = 30000)
		{
			api.RoomTypingSend(ID,typing,timeout);
		}

		/// <summary>
		/// Applies the new power levels.
		/// <remarks> You must set all the values in powerlevels.</remarks>
		/// </summary>
		/// <param name="powerlevels">Powerlevels.</param>
		public void ApplyNewPowerLevels(MatrixMRoomPowerLevels powerlevels){
			api.RoomStateSend (ID,"m.room.power_levels",powerlevels);
		}

		/// <summary>
		/// Invite a user to the room by userid.
		/// </summary>
		/// <param name="userid">Userid.</param>
		public void InviteToRoom(string userid){
			api.InviteToRoom (ID, userid);
		}

		/// <summary>
		/// Invite a user to the room by their object.
		/// </summary>
		/// <param name="user">User.</param>
		public void InviteToRoom(MatrixUser user){
			InviteToRoom (user.UserID);
		}

		/// <summary>
		/// Leave the room on the server.
		/// </summary>
		public void LeaveRoom(){
            api.FlushMessageQueue();
			api.RoomLeave (ID);
		}

		public void SetDisplayName (string displayname)
		{
			MatrixMRoomMember member;
			if (!Members.TryGetValue (api.user_id, out member)) {
				throw new MatrixException("Couldn't find the users membership event");
			}
			member.displayname = displayname;
			SendState(member, "m.room.member", api.user_id);
		}

		public void SetAvatar (string avatar)
		{
			MatrixMRoomMember member;
			if (!Members.TryGetValue (api.user_id, out member)) {
				throw new MatrixException("Couldn't find the users membership event");
			}
			member.avatar_url = avatar;
			SendState(member, "m.room.member", api.user_id);
		}

		public void SetEphemeral (MatrixSyncEvents ev)
		{
			ephemeral = ev.events;
			foreach (MatrixEvent evt in ephemeral) {
				if (evt.type == "m.reciept" && OnRecieptsRecieved != null) {
					MatrixMReceipt rec = (MatrixMReceipt)evt.content;
					foreach (KeyValuePair<string,MatrixReceipts> kv in rec.receipts) {
						OnRecieptsRecieved.Invoke (kv.Key, kv.Value);
					}
				} else if (evt.type == "m.typing" && OnTypingChanged != null) {
					OnTypingChanged.Invoke(((MatrixMTyping)evt.content).user_ids);
				}
			}
			if (OnEphemeralChanged != null) {
				OnEphemeralChanged.Invoke();
			}
		}

	}
}


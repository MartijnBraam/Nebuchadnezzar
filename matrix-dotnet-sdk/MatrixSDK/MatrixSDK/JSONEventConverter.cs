using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using MatrixSDK.Structures;
namespace MatrixSDK
{
	public class JSONEventConverter : JsonConverter
	{

		Dictionary<string,Type> contentTypes = new Dictionary<string, Type>{
			{"m.presence",					typeof(MatrixMPresence)},
			{"m.receipt",					typeof(MatrixMReceipt)}, //*Special case below
			{"m.room.encryption",			typeof(MatrixMRoomEncryption)},
			{"m.room.message",				typeof(MatrixMRoomMessage)},
			{"m.room.encrypted",			typeof(MatrixMRoomEncrypted)},
			{"m.room.member",				typeof(MatrixMRoomMember)},
			{"m.room.create",				typeof(MatrixMRoomCreate)},
			{"m.room.join_rules",			typeof(MatrixMRoomJoinRules)},
			{"m.room.aliases",				typeof(MatrixMRoomAliases)},
			{"m.room.canonical_alias", 		typeof(MatrixMRoomCanonicalAlias)},
			{"m.room.name", 				typeof(MatrixMRoomName)},
			{"m.room.topic", 				typeof(MatrixMRoomTopic)},
			{"m.room.power_levels", 		typeof(MatrixMRoomPowerLevels)},
			{"m.room.history_visibility",	typeof(MatrixMRoomHistoryVisibility)},
			{"m.typing",					typeof(MatrixMTyping)},
		};

		Dictionary<string,Type> messageContentTypes = new Dictionary<string, Type>{
			{"m.text",		typeof(MMessageText)},
			{"m.notice",	typeof(MMessageNotice)},
			{"m.emote",		typeof(MMessageEmote)},
			{"m.image",		typeof(MMessageImage)},
			{"m.file",		typeof(MMessageFile)},
			{"m.location",	typeof(MMessageLocation)},
		};

		public JSONEventConverter(Dictionary<string,Type> customMsgTypes = null){
			if (customMsgTypes != null) {
				foreach (KeyValuePair<string,Type> item in customMsgTypes) {
					if (contentTypes.ContainsKey (item.Key)) {
						contentTypes [item.Key] = item.Value;
					} else {
						contentTypes.Add (item.Key, item.Value);
					}
				}
			}
		}


		public void AddMessageType(string name, Type type){
			messageContentTypes.Add(name,type);
		}


		public void AddEventType (string msgtype, Type type)
		{
			contentTypes.Add(msgtype, type);
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(MatrixEvent);
		}
		
		public Type MessageContentType(string type){
			Type otype;
			if (messageContentTypes.TryGetValue (type, out otype)) {
				return otype;
			} else {
				#if DEBUG
				Console.WriteLine ("Unknown Message Type:" + type);
				#endif
			}
			return typeof(MatrixMRoomMessage);
		}

		public MatrixEventContent GetContent(JToken jobj,JsonSerializer serializer,string type){
			Type T;
			if (contentTypes.TryGetValue (type, out T)) {
				if (T == typeof(MatrixMRoomMessage)) {
					MatrixMRoomMessage message = new MatrixMRoomMessage ();
					serializer.Populate (jobj.CreateReader (), message);
					T = MessageContentType (message.msgtype);
				}
				MatrixEventContent content = (MatrixEventContent)Activator.CreateInstance(T);
				if (type == "m.receipt") {
					((MatrixMReceipt)content).ParseJObject((JObject)jobj);
				} else {
					serializer.Populate (jobj.CreateReader (), content);
				}
				return content;
			} else {
				#if DEBUG
				var payload = jobj.ToString();
				Console.WriteLine ("Unknown Event:" + type);
				#endif
				return new MatrixEventContent();
			}
		}

		public override object ReadJson(JsonReader reader, 
			Type objectType, 
			object existingValue, 
			JsonSerializer serializer)
		{
			// Load JObject from stream
			JObject jObject = JObject.Load(reader);
			// Populate the event itself
			MatrixEvent ev = new MatrixEvent();
			serializer.Populate (jObject.CreateReader (), ev);
			JToken redact;
			if (jObject ["content"].HasValues) {
				ev.content = GetContent (jObject ["content"], serializer, ev.type);
				ev.content.sender = ev.sender;
			} else if(((JObject)jObject["unsigned"]).TryGetValue("redacted_because",out redact)){
				//TODO: Parse Redacted
			}
			return ev;
		}

		public override bool CanWrite {
			get {
				return false;
			}

		}

		public override void WriteJson(JsonWriter writer, 
			object value,
			JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}
	}
}


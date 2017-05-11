using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace MatrixSDK
{
	public class JSONSerializer : JsonSerializer
	{
		public JSONSerializer () : base()
		{
			NullValueHandling = NullValueHandling.Ignore;
			Converters.Add (new JSONEnumConverter ());
			Converters.Add (new JSONEventConverter ());
		}
	}
	public class JSONEnumConverter : JsonConverter{
		public JSONEnumConverter() : base(){

		}

		public override bool CanRead {
			get {
				return false;
			}
		}

		public override void WriteJson (JsonWriter writer, object value, JsonSerializer serializer)
		{
			Type t = value.GetType ();
			string name = Enum.GetName (t, value).ToLower ();
			JToken.FromObject (name).WriteTo (writer);
		}

		public override bool CanConvert (Type objectType)
		{
			return objectType.IsEnum;
		}

		public override object ReadJson (JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			throw new NotImplementedException ();
		}
	}
}


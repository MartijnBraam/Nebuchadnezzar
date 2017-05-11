using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.Linq;
namespace MatrixSDK.Structures
{
	/// <summary>
	/// http://matrix.org/docs/spec/r0.0.1/client_server.html#m-receipt
	/// 
	/// </summary>
	public class MatrixMReceipt : MatrixEventContent
	{
		public Dictionary<string,MatrixReceipts> receipts;

		public void ParseJObject(JObject obj){
			receipts = new Dictionary<string, MatrixReceipts> ();
			foreach(JProperty prop in obj.Children()){
				MatrixReceipts reciepts = new MatrixReceipts ();
				reciepts.ParseJObject((JObject)prop.Value);
				receipts.Add (prop.Name, reciepts);
			}
		}
	}

	public class MatrixReceipts{
		public Dictionary<string,MatrixReceipt> m_read;

		public void ParseJObject(JObject obj){
			m_read = new Dictionary<string, MatrixReceipt> ();
			foreach(JProperty prop in obj.GetValue("m.read").Children()){
				MatrixReceipt reciept = new MatrixReceipt ();
				reciept.ts = ((JObject)prop.Value)["ts"].ToObject<Int64> ();
				m_read.Add(prop.Name,reciept);
			}
		}
	}

	public class MatrixReceipt{
		public Int64 ts;
	}
}
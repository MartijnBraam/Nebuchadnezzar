using System;

namespace MatrixSDK.Structures
{
	public class MatrixMRoomMessage : MatrixEventContent
	{
		public virtual string msgtype { get; set;}
		public string body;

        public override string ToString ()
        {
            return body;
        }
	}

	public class MatrixMRoomEncrypted : MatrixEventContent
	{
		public virtual string msgtype { get; set;}
		public string body;

		public override string ToString ()
		{
			return body;
		}
	}
		
	public class MMessageNotice : MatrixMRoomMessage
	{
		public override string msgtype { get { return "m.notice"; }}
	}

	public class MMessageText : MatrixMRoomMessage
	{
		public override string msgtype { get { return "m.text"; }}
	}

	public class MMessageEmote : MatrixMRoomMessage
	{
		public override string msgtype { get { return "m.emote"; }}
	}

	public class MMessageImage : MatrixMRoomMessage
	{
		public override string msgtype { get { return "m.image"; }}
		public MatrixImageInfo info;
		public MatrixImageInfo thumbnail_info;
		public string url;
		public string thumbnail_url;
	}

	public class MMessageFile : MatrixMRoomMessage
	{
		public override string msgtype { get { return "m.file"; }}
		public MatrixFileInfo info;
		public MatrixImageInfo thumbnail_info;
		public string url;
		public string thumbnail_url;
		public string filename;
	}

	public class MMessageLocation : MatrixMRoomMessage{
		public override string msgtype { get { return "m.location"; }}
		public string geo_url;
		public string thumbnail_url;
		public MatrixImageInfo thumbnail_info;
	}
    public class MMessageCustomHTML : MatrixMRoomMessage
    {
        public override string msgtype { get { return "m.notice"; }}
        public string format { get { return "org.matrix.custom.html"; }}
        public string formatted_body;
    }


}


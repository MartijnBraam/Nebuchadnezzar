using System;
using MatrixSDK.Structures;
namespace MatrixSDK.Client
{
	public class MatrixMediaFile
	{
		private string baseurl;
		private string mxcurl;
		private string contenttype;
		private MatrixFileInfo fileInfo;
		public  MatrixMediaFile (MatrixAPI api,string MXCUrl,string ContentType)
		{
			baseurl = api.BaseURL;
			mxcurl = MXCUrl;
			contenttype = ContentType;
		}

		public string GetMXCUrl(){
			return mxcurl;
		}

		public string GetThumbnailURL(int width,int height,string method = "crop"){
			return String.Format("{0}/_matrix/media/r0/thumbnail/{1}?width={2}&height={3}&method={4}",baseurl,mxcurl.Substring(6),width,height,method);
		}

		public string GetUrl(){
			return String.Format("{0}/_matrix/media/r0/download/{1}",baseurl,mxcurl.Substring(6));
		}
	}
}


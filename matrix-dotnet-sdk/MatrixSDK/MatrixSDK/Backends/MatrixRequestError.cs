using System;
using System.Net;
namespace MatrixSDK.Backends
{
	public class MatrixRequestError
	{
		public readonly string MatrixError;
		public readonly MatrixErrorCode MatrixErrorCode;
		public readonly HttpStatusCode Status;
		public bool IsOk{ get{return MatrixErrorCode == MatrixErrorCode.CL_NONE && Status == HttpStatusCode.OK;}}

		public MatrixRequestError(string merror,MatrixErrorCode code,HttpStatusCode status){
			MatrixError = merror;
			MatrixErrorCode = code;
			Status = status;
		}

		public string GetErrorString(){
			if (Status != HttpStatusCode.OK) {
				return "Got a Http Error :" + Status + " during request.";
			} else {
				return "Got a Matrix Error: " + MatrixErrorCode + " '" + MatrixError + "'";
			}
		}

		public override string ToString ()
		{
			return GetErrorString ();
		}

		public readonly static MatrixRequestError NO_ERROR = new MatrixRequestError(
			"",
			MatrixErrorCode.CL_NONE,
			HttpStatusCode.OK 
		);
	}
}


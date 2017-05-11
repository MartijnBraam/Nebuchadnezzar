using System;

namespace MatrixSDK
{
	[AttributeUsage(AttributeTargets.Method)]
	public class MatrixSpec : Attribute
	{
		const string MATRIX_SPEC_URL = "http://matrix.org/docs/spec/";
		public readonly string URL;
		public MatrixSpec(string url){
			URL = MATRIX_SPEC_URL + url;
		}

		public override string ToString ()
		{
			return string.Format (URL);
		}
	}
}


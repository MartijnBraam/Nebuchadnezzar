using System;

namespace MatrixSDK.Structures
{
	public class MatrixFileInfo
	{
		public string mimetype;
		public int size;	
	}

	public class MatrixImageInfo : MatrixFileInfo
	{
		public int h;
		public int w;
	}
}


using System;
using MatrixSDK.Client;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Nebuchadnezzar
{
	public class Cache
	{
		private MatrixClient matrixClient;
		private Storage storage;

		public Cache (MatrixClient matrixClient)
		{
			this.matrixClient = matrixClient;
			this.storage = new Storage ();
		}

		public MemoryStream GetAvatarObject(string url)
		{
			if (storage.CacheAvatars) {
				return HandleCachedDownload (url);
			} else {
				return this.matrixClient.DownloadMatrixContent (url);
			}
		}

		public MemoryStream GetContentObject(string url)
		{
			if (storage.CacheChatData) {
				return HandleCachedDownload (url);
			} else {
				return this.matrixClient.DownloadMatrixContent (url);
			}
		}

		public MemoryStream GetEncryptedContentObject(string url)
		{
			if (storage.CacheEncrypted) {
				return HandleCachedDownload (url);
			} else {
				return this.matrixClient.DownloadMatrixContent (url);
			}
		}

		private MemoryStream HandleCachedDownload(string url)
		{
			var homedir = System.Environment.GetEnvironmentVariable ("HOME");
			var cachedir = System.Environment.GetEnvironmentVariable ("XDG_CACHE_DIR");
			if (cachedir == null) {
				cachedir = Path.Combine (homedir, ".cache");
			}
			var objectCache = Path.Combine (cachedir, "nebuchadnezzar");
			if (!Directory.Exists (objectCache)) {
				Directory.CreateDirectory (objectCache);
			}

			var cacheKey = BitConverter.ToString(SHA1.Create ().ComputeHash (Encoding.UTF8.GetBytes (url)));
			var cacheFile = Path.Combine (objectCache, cacheKey);
			if (File.Exists (cacheFile)) {
				FileStream file = new FileStream (cacheFile, FileMode.Open, FileAccess.Read);
				var buffer = new MemoryStream ();
				file.CopyTo (buffer);
				file.Close ();
				return buffer;
			} else {
				var result = this.matrixClient.DownloadMatrixContent (url);

				FileStream file = new FileStream (cacheFile, FileMode.Create, FileAccess.Write);
				result.Position = 0;
				result.CopyTo (file);
				file.Close ();
				result.Position = 0;

				return result;
			}
		}
	}
}


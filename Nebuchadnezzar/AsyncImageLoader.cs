using System;
using MatrixSDK.Structures;
using Gtk;
using System.Drawing;
using Gdk;
using System.Drawing.Imaging;
using MatrixSDK.Client;
using System.Threading;

namespace Nebuchadnezzar
{
	public class AsyncImageLoader
	{
		private MMessageImage message;
		private int width;
		private int height;
		private Gtk.Image widget;
		private MatrixClient client;

		private System.Drawing.Size thumbSize;

		public AsyncImageLoader (MMessageImage imageMessage, MatrixClient client)
		{
			this.message = imageMessage;
			this.width = this.message.info.w;
			this.height = this.message.info.h;
			this.client = client;
		}

		public Gtk.Image GetImageWidget(int maxWidth, int maxHeight)
		{
			var bounds = GetBounds (new System.Drawing.Size (maxWidth, maxHeight));
			this.thumbSize = bounds;

			Bitmap bmp = new Bitmap(bounds.Width, bounds.Height, PixelFormat.Format32bppPArgb);
			bmp.MakeTransparent ();

			using (Graphics g = Graphics.FromImage (bmp)) 
			{
				g.Clear (System.Drawing.Color.Gray);
			}

			this.widget = new Gtk.Image (Utils.bitmapToPixbuf (bmp));

			Thread thr = new Thread (new ThreadStart (LoadingRoutine));
			thr.Start ();

			return this.widget;
		}

		private System.Drawing.Size GetBounds(System.Drawing.Size canvas)
		{
			var originalAspect = (float)this.width / (float)this.height;
			var canvasAspect = (float)canvas.Width / (float)canvas.Height;

			if (originalAspect > canvasAspect) {
				canvas.Height =(int)( canvas.Width / originalAspect);
			} else {
				canvas.Width = (int)( canvas.Height / originalAspect);
			}
			return canvas;
		}

		private void LoadingRoutine(){
			var imageData = this.client.DownloadMatrixContent (this.message.url);
			var scaled = Utils.resizeImage (System.Drawing.Image.FromStream (imageData), this.thumbSize.Width, this.thumbSize.Height);
			if (this.widget != null) {
				Gtk.Application.Invoke (delegate {
					this.widget.Pixbuf = Utils.bitmapToPixbuf (scaled);
				});
			}
		}
	}
}


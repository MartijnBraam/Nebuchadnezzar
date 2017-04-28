using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Resources;
using System.Drawing.Drawing2D;

namespace Nebuchadnezzar
{
	public class AvatarGenerator
	{
		public static Gdk.Pixbuf createRoomAvatar(string name, bool encrypted=false, System.Drawing.Image avatar=null){
			string letter = name.Substring (0, 1);
			if (letter == "@") {
				letter = name.Substring (1, 1);
			}

			var letterIndex = (int)(letter.ToLower ().ToCharArray () [0]) - 96;
			var hue = (float)letterIndex / 26.0 * 360;

			Bitmap bmp = new Bitmap(32, 32, PixelFormat.Format32bppPArgb);
			bmp.MakeTransparent ();

			Color background = Utils.ColorFromHSV (hue, 0.3, 0.9);

			using (Graphics g = Graphics.FromImage(bmp))
			{
				Font font = new Font("Arial", 20, FontStyle.Bold, GraphicsUnit.Point);
				if (avatar == null) {
					g.Clear (background);
				} else {
					g.DrawImage (avatar, new Point (0, 0));
				}

				g.CompositingQuality = CompositingQuality.GammaCorrected;
				g.CompositingMode = CompositingMode.SourceOver;

				if (encrypted) {
					System.Reflection.Assembly thisExe;
					thisExe = System.Reflection.Assembly.GetExecutingAssembly();
					System.IO.Stream file = 
						thisExe.GetManifestResourceStream("Nebuchadnezzar.Assets.Encrypted_Overlay.png");
					
					var overlay = Image.FromStream(file);
					g.DrawImage (overlay, new Point (0, 0));
				}
				if (avatar == null) {
					g.DrawString (letter, font, Brushes.White, 0, 0);
				}
			}

			return Utils.bitmapToPixbuf (bmp);
		}
	}
}
using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace AssetManager.Common
{
    public class Font : Shared, IDisposable
    {
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int RemoveFontResourceEx(string lpszFilename, int fl, IntPtr pdv);

        private bool isDisposed = false;

        PrivateFontCollection fonts;
        FontFamily family;
        System.Drawing.Font theFont;

        public Font(FileInfo file) : base(file)
        { }

        public static FontFamily LoadFontFamily(string fileName, out PrivateFontCollection fontCollection)
        {
            fontCollection = new PrivateFontCollection();
            fontCollection.AddFontFile(fileName);
            return fontCollection.Families[0];
        }

        public System.Drawing.Font GetFont()
        {
            Directory.CreateDirectory("Temp");
            if (!File.Exists(System.IO.Path.Combine("Temp", Name)))
                File.Copy(FullName, System.IO.Path.Combine("Temp", Name));

            family = LoadFontFamily(System.IO.Path.Combine("Temp", Name), out fonts);
            theFont = new System.Drawing.Font(family, 20.0f);
            return theFont;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    theFont.Dispose();
                    family.Dispose();
                    fonts.Dispose();

                    theFont = null;
                    family = null;
                    fonts = null;
                }

                RemoveFontResourceEx(FullName, 16, IntPtr.Zero);
            }

            isDisposed = true;
        }

        ~Font()
        {
            Dispose(false);
        }
    }
}

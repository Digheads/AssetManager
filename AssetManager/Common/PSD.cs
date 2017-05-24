using System.IO;

namespace AssetManager.Common
{
    public class PSD : Shared
    {
        private Texture previewFile;
        public Texture PreviewFile
        {
            get { return previewFile; }
            set { previewFile = value; }
        }


        public PSD(FileInfo file) : base(file)
        {

        }

        public PSD(FileInfo file, FileInfo previewFile) : base(file)
        {
            PreviewFile = new Texture(previewFile);
        }
    }
}

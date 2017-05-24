using System.IO;

namespace AssetManager.Common
{
    public class Substance : Shared
    {
        private Texture previewFile;
        public Texture PreviewFile
        {
            get { return previewFile; }
            set { previewFile = value; }
        }


        public Substance(FileInfo file) : base(file)
        {

        }

        public Substance(FileInfo file, FileInfo previewFile) : base(file)
        {
            PreviewFile = new Texture(previewFile);
        }
    }
}

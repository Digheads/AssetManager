using System.IO;

namespace AssetManager.Common
{
    public class Package : Shared
    {
        private Script readmeFile;
        public Script ReadmeFile
        {
            get { return readmeFile; }
            set { readmeFile = value; }
        }

        public Package(FileInfo file) : base(file)
        {
        }

        public Package(FileInfo file, FileInfo readmeFile) : base(file)
        {
            ReadmeFile = new Script(readmeFile);
        }
    }
}

using System.IO;

namespace AssetManager.Common
{
    public class Script : Shared
    {
        public Script(FileInfo file) : base(file)
        {
        }

        public string ReadFile()
        {
            return File.ReadAllText(FullName);
        }
    }
}

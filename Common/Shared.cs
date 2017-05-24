using System.IO;
using System.Windows.Forms;

namespace AssetManager.Common
{
    public abstract class Shared
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string fullName;
        public string FullName
        {
            get { return fullName; }
            set { fullName = value; }
        }

        private string path;
        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        private string extension;
        public string Extension
        {
            get { return extension; }
            set { extension = value; }
        }

        private string size;
        public string Size
        {
            get { return size; }
            set { size = value; }
        }

        public Shared(FileInfo file)
        {
            Name = file.Name;
            FullName = file.FullName;
            Path = file.DirectoryName;
            Extension = file.Extension;
            Size = file.Length.ToString("N0") + " bytes";
        }

        public void ExportFile(string outputName, string separator)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog()
            {
                ShowNewFolderButton = true,
                Description = "Select folder for " + outputName + separator + Name + " file:"
            };
            DialogResult result = fbd.ShowDialog();

            if (result == DialogResult.OK)
            {
                File.Copy(FullName, System.IO.Path.Combine(fbd.SelectedPath, outputName + separator + Name), true);
            }
        }


    }
}

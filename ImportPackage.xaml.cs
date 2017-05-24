using AssetManager.Common;
using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace AssetManager
{
    public partial class ImportPackage : Window
    {
        private Package addedPackage;
        private Script addedReadMe;

        public ImportPackage()
        {
            InitializeComponent();
            DataContext = this;

            DirectoryInfo dirInfo = new DirectoryInfo("Packages");
            PackageTypes.ItemsSource = dirInfo.EnumerateDirectories();
        }

        private void BrowsePackage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog()
            {
                Multiselect = false,
                DefaultExt = ".unitypackage",
                Filter = "Unity 3D Package (.unitypackage)|*.unitypackage"
            };
            bool? result = openfile.ShowDialog();

            if (result == true)
            {
                addedPackage = new Package(new FileInfo(openfile.FileName));
                BrowsedPackage.Text = openfile.FileName;
            }
        }

        private void BrowseReadMe_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog()
            {
                Multiselect = false,
                DefaultExt = ".jpg",
                Filter = "Text File (.txt)|*.txt"
            };
            bool? result = openfile.ShowDialog();

            if (result == true)
            {
                addedReadMe = new Script(new FileInfo(openfile.FileName));

                BrowsedReadMe.Text = addedReadMe.ReadFile();
            }
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (addedPackage != null)
            {
                string newDir = string.Empty;
                if (!string.IsNullOrWhiteSpace(PackageTypes.Text) && !string.IsNullOrWhiteSpace(PackageName.Text))
                {
                    newDir = PackageTypes.Text;
                }
                else
                {
                    MessageBox.Show("Package type and name cannot be empty!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                DirectoryInfo dirInfo = new DirectoryInfo(Path.Combine("Packages", newDir, PackageName.Text));
                if (!dirInfo.Exists)
                    Directory.CreateDirectory(Path.Combine("Packages", newDir, PackageName.Text));
                else
                {
                    MessageBoxResult result = MessageBox.Show("Package already exists: " + PackageName.Text + "! Do you want to overwrite it's content?", "Import Package", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
                    if (result == MessageBoxResult.Yes)
                    {
                        dirInfo.Delete(true);
                        dirInfo.Create();
                    }
                    else
                        return;
                }

                File.Copy(addedPackage.FullName, Path.Combine(dirInfo.FullName, addedPackage.Name));
                if (addedReadMe != null)
                    File.Copy(addedReadMe.FullName, Path.Combine(dirInfo.FullName, addedReadMe.Name));

                this.Hide();
            }
            else
            {
                MessageBox.Show("No Package was selected. Please, fix it!", "Import Package", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}

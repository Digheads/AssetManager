using AssetManager.Common;
using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace AssetManager
{
    public partial class ImportPSD : Window
    {
        private PSD addedPSD;
        private Texture addedPreview;

        public ImportPSD()
        {
            InitializeComponent();
            DataContext = this;

            DirectoryInfo dirInfo = new DirectoryInfo("PSDs");
            PSDTypes.ItemsSource = dirInfo.EnumerateDirectories();
        }

        private void BrowsePSD_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog()
            {
                Multiselect = false,
                DefaultExt = ".psd",
                Filter = "PhotoShop File (.psd)|*.psd"
            };
            bool? result = openfile.ShowDialog();

            if (result == true)
            {
                addedPSD = new PSD(new FileInfo(openfile.FileName));
                BrowsedPSD.Text = openfile.FileName;
            }
        }

        private void BrowsePreview_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog()
            {
                Multiselect = false,
                DefaultExt = ".jpg",
                Filter = "JPG images (.jpg)|*.jpg|JPEG images (.jpeg)|*.jpeg|PNG images (.png)|*.png|Bitmap images (.bmp)|*.bmp"
            };
            bool? result = openfile.ShowDialog();

            if (result == true)
            {
                addedPreview = new Texture(new FileInfo(openfile.FileName));

                BrowsedPreview.Source = addedPreview.GetImage();
            }
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (addedPSD != null)
            {
                string newDir = string.Empty;
                if (!string.IsNullOrWhiteSpace(PSDTypes.Text) && !string.IsNullOrWhiteSpace(PSDName.Text))
                {
                    newDir = PSDTypes.Text;
                }
                else
                {
                    MessageBox.Show("PSD type and name cannot be empty!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                DirectoryInfo dirInfo = new DirectoryInfo(Path.Combine("PSDs", newDir, PSDName.Text));
                if (!dirInfo.Exists)
                    Directory.CreateDirectory(Path.Combine("PSDs", newDir, PSDName.Text));
                else
                {
                    MessageBoxResult result = MessageBox.Show("PSD already exists: " + PSDName.Text + "! Do you want to overwrite it's content?", "Import PSD", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
                    if (result == MessageBoxResult.Yes)
                    {
                        dirInfo.Delete(true);
                        dirInfo.Create();
                    }
                    else
                        return;
                }

                File.Copy(addedPSD.FullName, Path.Combine(dirInfo.FullName, addedPSD.Name));
                if (addedPreview != null)
                    File.Copy(addedPreview.FullName, Path.Combine(dirInfo.FullName, addedPreview.Name));

                this.Hide();
            }
            else
            {
                MessageBox.Show("No PSD was selected. Please, fix it!", "Import PSD", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}

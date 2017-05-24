using AssetManager.Common;
using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace AssetManager
{
    public partial class ImportSubstance : Window
    {
        private Substance addedSubstance;
        private Texture addedPreview;

        public ImportSubstance()
        {
            InitializeComponent();
            DataContext = this;

            DirectoryInfo dirInfo = new DirectoryInfo("Substances");
            SubstanceTypes.ItemsSource = dirInfo.EnumerateDirectories();
        }

        private void BrowseSubstance_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog()
            {
                Multiselect = false,
                DefaultExt = ".sbsar",
                Filter = "Substance (.sbsar)|*.sbsar"
            };
            bool? result = openfile.ShowDialog();

            if (result == true)
            {
                addedSubstance = new Substance(new FileInfo(openfile.FileName));
                BrowsedSubstance.Text = openfile.FileName;
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
            if (addedSubstance != null)
            {
                string newDir = string.Empty;
                if (!string.IsNullOrWhiteSpace(SubstanceTypes.Text) && !string.IsNullOrWhiteSpace(SubstanceName.Text))
                {
                    newDir = SubstanceTypes.Text;
                }
                else
                {
                    MessageBox.Show("Substance type and name cannot be empty!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                DirectoryInfo dirInfo = new DirectoryInfo(Path.Combine("Substances", newDir, SubstanceName.Text));
                if (!dirInfo.Exists)
                    Directory.CreateDirectory(Path.Combine("Substances", newDir, SubstanceName.Text));
                else
                {
                    MessageBoxResult result = MessageBox.Show("Substance already exists: " + SubstanceName.Text + "! Do you want to overwrite it's content?", "Import substance", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
                    if (result == MessageBoxResult.Yes)
                    {
                        dirInfo.Delete(true);
                        dirInfo.Create();
                    }
                    else
                        return;
                }

                File.Copy(addedSubstance.FullName, Path.Combine(dirInfo.FullName, addedSubstance.Name));
                if (addedPreview != null)
                    File.Copy(addedPreview.FullName, Path.Combine(dirInfo.FullName, addedPreview.Name));

                this.Hide();
            }
            else
            {
                MessageBox.Show("No substance was selected. Please, fix it!", "Import substance", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}

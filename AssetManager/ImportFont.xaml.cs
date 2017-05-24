using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace AssetManager
{
    public partial class ImportFont : Window
    {
        private Common.Font addedFont;

        public ImportFont()
        {
            InitializeComponent();
            DataContext = this;

            DirectoryInfo dirInfo = new DirectoryInfo("Fonts");
            FontTypes.ItemsSource = dirInfo.EnumerateDirectories();
        }

        private void BrowseFont_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog()
            {
                Multiselect = false,
                DefaultExt = ".ttf",
                Filter = "TrueType Format (.ttf)|*.ttf"
            };
            bool? result = openfile.ShowDialog();

            if (result == true)
            {
                addedFont = new Common.Font(new FileInfo(openfile.FileName));
                BrowsedFont.Text = openfile.FileName;
                BrowsedPreview.Font = addedFont.GetFont();
            }
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (addedFont != null)
            {
                string newDir = string.Empty;
                if (!string.IsNullOrWhiteSpace(FontTypes.Text) && !string.IsNullOrWhiteSpace(FontName.Text))
                {
                    newDir = FontTypes.Text;
                }
                else
                {
                    MessageBox.Show("Font type and name cannot be empty!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                DirectoryInfo dirInfo = new DirectoryInfo(Path.Combine("Fonts", newDir, FontName.Text));
                if (!dirInfo.Exists)
                    Directory.CreateDirectory(Path.Combine("Fonts", newDir, FontName.Text));
                else
                {
                    MessageBoxResult result = MessageBox.Show("Font already exists: " + FontName.Text + "! Do you want to overwrite it's content?", "Import Font", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
                    if (result == MessageBoxResult.Yes)
                    {
                        dirInfo.Delete(true);
                        dirInfo.Create();
                    }
                    else
                        return;
                }

                File.Copy(addedFont.FullName, Path.Combine(dirInfo.FullName, addedFont.Name));
                addedFont.Dispose();
                this.Close();
            }
            else
            {
                MessageBox.Show("No Font was selected. Please, fix it!", "Import Font", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}

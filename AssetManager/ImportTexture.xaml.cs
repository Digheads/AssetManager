using AssetManager.Common;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;

namespace AssetManager
{
    public partial class ImportTexture : Window
    {
        public ObservableCollection<Texture> AddedItems = new ObservableCollection<Texture>();

        public ImportTexture()
        {
            InitializeComponent();
            DataContext = this;
            FileList.ItemsSource = AddedItems;

            DirectoryInfo dirInfo = new DirectoryInfo("Textures");
            TextureTypes.ItemsSource = dirInfo.EnumerateDirectories();
        }

        private void AddFiles_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog()
            {
                Multiselect = true,
                DefaultExt = ".jpg",
                Filter = "JPG images (.jpg)|*.jpg|JPEG images (.jpeg)|*.jpeg|PNG images (.png)|*.png|Bitmap images (.bmp)|*.bmp"
            };
            bool? result = openfile.ShowDialog();

            if (result == true)
            {
                string[] fileName = openfile.FileNames;

                foreach (string item in fileName)
                {
                    AddedItems.Add(new Texture(new FileInfo(item)));
                }
            }
        }

        private void RemoveFiles_Click(object sender, RoutedEventArgs e)
        {
            for (int i = FileList.SelectedItems.Count - 1; i >= 0; i--)
                AddedItems.Remove((Texture)FileList.SelectedItems[i]);
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (AddedItems.Count(x => x.Type == Texture.TextureTypes.NotSet) == 0)
            {
                if (AddedItems.Count(x => x.Type == Texture.TextureTypes.Diffuse) <= 1 && AddedItems.Count(x => x.Type == Texture.TextureTypes.Specular) <= 1 && AddedItems.Count(x => x.Type == Texture.TextureTypes.Metallic) <= 1 && AddedItems.Count(x => x.Type == Texture.TextureTypes.Bump) <= 1 && AddedItems.Count(x => x.Type == Texture.TextureTypes.Normal) <= 1 && AddedItems.Count(x => x.Type == Texture.TextureTypes.Height) <= 1 && AddedItems.Count(x => x.Type == Texture.TextureTypes.Occlusion) <= 1 && AddedItems.Count(x => x.Type == Texture.TextureTypes.Emission) <= 1 && AddedItems.Count(x => x.Type == Texture.TextureTypes.Opacity) <= 1)
                {
                    string newDir = string.Empty;
                    if (!string.IsNullOrWhiteSpace(TextureTypes.Text) && !string.IsNullOrWhiteSpace(TextureName.Text))
                    {
                        newDir = TextureTypes.Text;
                    }
                    else
                    {
                        MessageBox.Show("Texture type and name cannot be empty!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    DirectoryInfo dirInfo = new DirectoryInfo(Path.Combine("Textures", newDir, TextureName.Text));
                    if (!dirInfo.Exists)
                        Directory.CreateDirectory(Path.Combine("Textures", newDir, TextureName.Text));
                    else
                    {
                        MessageBoxResult result = MessageBox.Show("Texture already exists: " + TextureName.Text + "! Do you want to overwrite it's content?", "Import texture", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
                        if (result == MessageBoxResult.Yes)
                        {
                            dirInfo.Delete(true);
                            dirInfo.Create();
                        }
                        else
                            return;
                    }

                    foreach (var file in AddedItems)
                    {
                        switch (file.Type)
                        {
                            case Texture.TextureTypes.Diffuse:
                                File.Copy(file.FullName, Path.Combine(dirInfo.FullName, "d" + file.Extension));
                                break;
                            case Texture.TextureTypes.Specular:
                                File.Copy(file.FullName, Path.Combine(dirInfo.FullName, "s" + file.Extension), true);
                                break;
                            case Texture.TextureTypes.Metallic:
                                File.Copy(file.FullName, Path.Combine(dirInfo.FullName, "m" + file.Extension), true);
                                break;
                            case Texture.TextureTypes.Bump:
                                File.Copy(file.FullName, Path.Combine(dirInfo.FullName, "b" + file.Extension), true);
                                break;
                            case Texture.TextureTypes.Normal:
                                File.Copy(file.FullName, Path.Combine(dirInfo.FullName, "n" + file.Extension), true);
                                break;
                            case Texture.TextureTypes.Height:
                                File.Copy(file.FullName, Path.Combine(dirInfo.FullName, "h" + file.Extension), true);
                                break;
                            case Texture.TextureTypes.Occlusion:
                                File.Copy(file.FullName, Path.Combine(dirInfo.FullName, "o" + file.Extension), true);
                                break;
                            case Texture.TextureTypes.Emission:
                                File.Copy(file.FullName, Path.Combine(dirInfo.FullName, "e" + file.Extension), true);
                                break;
                            case Texture.TextureTypes.Opacity:
                                File.Copy(file.FullName, Path.Combine(dirInfo.FullName, "p" + file.Extension), true);
                                break;
                        }
                    }

                    AddedItems.Clear();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("One or more textures have duplicate flags. Please, fix it!", "Import texture", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("One of the textures doesn't have a flag. Please, fix it!", "Import texture", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}

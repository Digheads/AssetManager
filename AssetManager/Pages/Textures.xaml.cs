using AssetManager.Common;
using FirstFloor.ModernUI.Presentation;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AssetManager.Pages
{
    public partial class Textures : UserControl
    {
        private List<Texture> textures = new List<Texture>();
        DirectoryInfo dirInfo = new DirectoryInfo("Textures");
        int oldVal;

        public Textures()
        {
            InitializeComponent();

            if (!dirInfo.Exists)
                dirInfo.Create();

            Tipp.Text = "Choose texture type from combo-box!";
            TextureTypes.ItemsSource = dirInfo.EnumerateDirectories();
        }

        private void TextureTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TextureTypes.SelectedIndex == -1)
                TextureTypes.SelectedIndex = oldVal;

            Preview.Source = null;
            Tipp.Text = "Choose texture from list!";
            DirectoryInfo info = new DirectoryInfo(Path.Combine("Textures", TextureTypes.SelectedValue.ToString()));
            TexturesList.ItemsSource = info.GetDirectories();

            Export.IsEnabled = false;
            ExportAll.IsEnabled = false;
            ShowIn3D.IsEnabled = false;
        }

        private void TexturesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            textures.Clear();
            oldVal = TextureTypes.SelectedIndex;

            if (TexturesList.SelectedItems.Count != 0)
            {
                Preview.Source = null;
                Tipp.Text = "Choose available texture!";

                Diffuse.IsChecked = false;
                Specular.IsChecked = false;
                Metallic.IsChecked = false;
                Bump.IsChecked = false;
                Normal.IsChecked = false;
                Height.IsChecked = false;
                Occlusion.IsChecked = false;
                Emission.IsChecked = false;
                Opacity.IsChecked = false;

                DeleteTexture.IsEnabled = true;

                if (TexturesList.SelectedItems.Count != 0 && TextureTypes.SelectedItem != null)
                {
                    DirectoryInfo info = new DirectoryInfo(Path.Combine("Textures", TextureTypes.SelectedValue.ToString(), TexturesList.SelectedValue.ToString()));

                    foreach (var file in info.GetFiles().Where(x => x.Extension.ToLower() == ".jpg" || x.Extension.ToLower() == ".png" || x.Extension.ToLower() == ".jpeg" || x.Extension.ToLower() == ".bmp"))
                    {
                        textures.Add(new Texture(file));
                    }

                    int i = 0;
                    if (textures.Exists(x => x.Type == Texture.TextureTypes.Diffuse))
                    {
                        Diffuse.IsEnabled = true;
                    }
                    else
                    {
                        Diffuse.IsEnabled = false;
                        i++;
                    }
                    if (textures.Exists(x => x.Type == Texture.TextureTypes.Specular))
                    {
                        Specular.IsEnabled = true;
                    }
                    else
                    {
                        Specular.IsEnabled = false;
                        i++;
                    }
                    if (textures.Exists(x => x.Type == Texture.TextureTypes.Metallic))
                    {
                        Metallic.IsEnabled = true;
                    }
                    else
                    {
                        Metallic.IsEnabled = false;
                        i++;
                    }
                    if (textures.Exists(x => x.Type == Texture.TextureTypes.Bump))
                    {
                        Bump.IsEnabled = true;
                    }
                    else
                    {
                        Bump.IsEnabled = false;
                        i++;
                    }
                    if (textures.Exists(x => x.Type == Texture.TextureTypes.Normal))
                    {
                        Normal.IsEnabled = true;
                    }
                    else
                    {
                        Normal.IsEnabled = false;
                        i++;
                    }
                    if (textures.Exists(x => x.Type == Texture.TextureTypes.Height))
                    {
                        Height.IsEnabled = true;
                    }
                    else
                    {
                        Height.IsEnabled = false;
                        i++;
                    }
                    if (textures.Exists(x => x.Type == Texture.TextureTypes.Occlusion))
                    {
                        Occlusion.IsEnabled = true;
                    }
                    else
                    {
                        Occlusion.IsEnabled = false;
                        i++;
                    }
                    if (textures.Exists(x => x.Type == Texture.TextureTypes.Emission))
                    {
                        Emission.IsEnabled = true;
                    }
                    else
                    {
                        Emission.IsEnabled = false;
                        i++;
                    }
                    if (textures.Exists(x => x.Type == Texture.TextureTypes.Opacity))
                    {
                        Opacity.IsEnabled = true;
                    }
                    else
                    {
                        Opacity.IsEnabled = false;
                        i++;
                    }
                    if (i == 9)
                    {
                        Tipp.Text = "No texture available in this type!";
                    }

                    if (Diffuse.IsEnabled)
                    { Diffuse.IsChecked = true; }
                    else if (Specular.IsEnabled)
                    { Specular.IsChecked = true; }
                    else if (Metallic.IsEnabled)
                    { Metallic.IsChecked = true; }
                    else if (Bump.IsEnabled)
                    { Bump.IsChecked = true; }
                    else if (Normal.IsEnabled)
                    { Normal.IsChecked = true; }
                    else if (Height.IsEnabled)
                    { Height.IsChecked = true; }
                    else if (Occlusion.IsEnabled)
                    { Occlusion.IsChecked = true; }
                    else if (Emission.IsEnabled)
                    { Emission.IsChecked = true; }
                    else if (Opacity.IsEnabled)
                    { Opacity.IsChecked = true; }

                    Export.IsEnabled = true;
                    ExportAll.IsEnabled = true;
                    ShowIn3D.IsEnabled = true;
                }
            }
            else
                DeleteTexture.IsEnabled = false;
        }

        private void LoadImage(Texture.TextureTypes type)
        {
            Texture tx = textures.Find(x => x.Type == type);

            Tipp.Text = string.Empty;
            Preview.Source = tx.GetImage();
            Dimension.Text = tx.Resolution;
            Size.Text = tx.Size;
        }

        private void Diffuse_Checked(object sender, RoutedEventArgs e)
        {
            LoadImage(Texture.TextureTypes.Diffuse);
        }
        private void Specular_Checked(object sender, RoutedEventArgs e)
        {
            LoadImage(Texture.TextureTypes.Specular);
        }
        private void Metallic_Checked(object sender, RoutedEventArgs e)
        {
            LoadImage(Texture.TextureTypes.Metallic);
        }
        private void Bump_Checked(object sender, RoutedEventArgs e)
        {
            LoadImage(Texture.TextureTypes.Bump);
        }
        private void Normal_Checked(object sender, RoutedEventArgs e)
        {
            LoadImage(Texture.TextureTypes.Normal);
        }
        private void Height_Checked(object sender, RoutedEventArgs e)
        {
            LoadImage(Texture.TextureTypes.Height);
        }
        private void Occlusion_Checked(object sender, RoutedEventArgs e)
        {
            LoadImage(Texture.TextureTypes.Occlusion);
        }
        private void Emission_Checked(object sender, RoutedEventArgs e)
        {
            LoadImage(Texture.TextureTypes.Emission);
        }
        private void Opacity_Checked(object sender, RoutedEventArgs e)
        {
            LoadImage(Texture.TextureTypes.Opacity);
        }

        private void ExportImage(Texture.TextureTypes type)
        {
            Texture tx = textures.Find(x => x.Type == type);
            tx.ExportFile(((DirectoryInfo)TexturesList.SelectedItem).Name, "_");
        }
        private void Export_Click(object sender, RoutedEventArgs e)
        {
            if (Diffuse.IsChecked == true)
            {
                ExportImage(Texture.TextureTypes.Diffuse);
            }
            if (Specular.IsChecked == true)
            {
                ExportImage(Texture.TextureTypes.Specular);
            }
            if (Metallic.IsChecked == true)
            {
                ExportImage(Texture.TextureTypes.Metallic);
            }
            if (Bump.IsChecked == true)
            {
                ExportImage(Texture.TextureTypes.Bump);
            }
            if (Normal.IsChecked == true)
            {
                ExportImage(Texture.TextureTypes.Normal);
            }
            if (Height.IsChecked == true)
            {
                ExportImage(Texture.TextureTypes.Height);
            }
            if (Occlusion.IsChecked == true)
            {
                ExportImage(Texture.TextureTypes.Occlusion);
            }
            if (Emission.IsChecked == true)
            {
                ExportImage(Texture.TextureTypes.Emission);
            }
            if (Opacity.IsChecked == true)
            {
                ExportImage(Texture.TextureTypes.Opacity);
            }
        }
        private void ExportAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (var tx in textures)
            {
                tx.ExportFile(((DirectoryInfo)TexturesList.SelectedItem).Name, "_");
            }
        }

        private void ShowIn3D_Click(object sender, RoutedEventArgs e)
        {
            File.Copy(Path.Combine("Common", "box.3DS"), Path.Combine(textures.First().Path, "box.3DS"), true);
            File.Copy(Path.Combine("Common", "sphere.3DS"), Path.Combine(textures.First().Path, "sphere.3DS"), true);
            File.Copy(Path.Combine("Common", "tube.3DS"), Path.Combine(textures.First().Path, "tube.3DS"), true);
            File.Copy(Path.Combine("Common", "teapot.3DS"), Path.Combine(textures.First().Path, "teapot.3DS"), true);

            Show3D ddd = new Show3D(textures.First().Path);
            ddd.Show();
        }

        private void ImportTexture_Click(object sender, RoutedEventArgs e)
        {
            AppearanceManager.Current.ThemeSource = new Link { DisplayName = "light", Source = AppearanceManager.LightThemeSource }.Source;

            Preview.Source = null;
            oldVal = TextureTypes.SelectedIndex;

            ImportTexture text = new ImportTexture();
            text.ShowDialog();

            TextureTypes.ItemsSource = dirInfo.EnumerateDirectories();

            AppearanceManager.Current.ThemeSource = new Link { DisplayName = "dark", Source = AppearanceManager.DarkThemeSource }.Source;
        }
        private void DeleteTexture_Click(object sender, RoutedEventArgs e)
        {
            Preview.Source = null;
            DirectoryInfo dir = new DirectoryInfo(Path.Combine("Textures", TextureTypes.SelectedValue.ToString(), TexturesList.SelectedValue.ToString()) + "\\");
            if (dir.Exists)
            {
                SetAttributesNormal(dir);
                dir.Delete(true);
            }
            TextureTypes.ItemsSource = dirInfo.EnumerateDirectories();
            TexturesList_SelectionChanged(this, null);
        }

        private void SetAttributesNormal(DirectoryInfo dir)
        {
            foreach (var subDir in dir.GetDirectories())
                SetAttributesNormal(subDir);
            foreach (var file in dir.GetFiles())
            {
                file.Attributes = FileAttributes.Normal;
            }

        }
    }
}

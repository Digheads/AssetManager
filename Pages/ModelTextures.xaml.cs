using AssetManager.Common;
using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Navigation;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace AssetManager.Pages
{
    public partial class ModelTextures : UserControl, IContent
    {
        List<Texture> textures = new List<Texture>();

        public ModelTextures()
        {
            InitializeComponent();
        }

        private void TexturesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TexturesList.SelectedItems.Count > 0)
            {
                Texture tx = TexturesList.SelectedItem as Texture;

                Tipp.Text = string.Empty;
                Preview.Source = tx.GetImage();
                Dimension.Text = tx.Resolution;
                Size.Text = tx.Size;

                Export.IsEnabled = true;
                ExportAll.IsEnabled = true;
            }
        }

        private void ExportAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (var tx in textures)
            {
                tx.ExportFile(((DirectoryInfo)TexturesList.SelectedItem).Name, "_");
            }
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            Texture tx = TexturesList.SelectedItem as Texture;
            tx.ExportFile(((DirectoryInfo)TexturesList.SelectedItem).Name, "_");
        }

        public void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {

        }
        public void OnNavigatedFrom(NavigationEventArgs e)
        {
            Preview.Source = null;
            textures.Clear();
            Export.IsEnabled = false;
            ExportAll.IsEnabled = false;
            Dimension.Text = string.Empty;
            Size.Text = string.Empty;
        }
        public void OnNavigatedTo(NavigationEventArgs e)
        {
            Tipp.Text = "Choose available texture!";

            if (Global.Model != null)
            {
                foreach (var mat in Global.Model.Material)
                {
                    textures.AddRange(mat.Textures);
                }

                TexturesList.ItemsSource = textures;
                TexturesList.DisplayMemberPath = "Name";
            }
            else
            {
                Tipp.Text = "No texture available!";
            }
        }
        public void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {

        }
    }
}

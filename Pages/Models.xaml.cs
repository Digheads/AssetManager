using AssetManager.Common;
using FirstFloor.ModernUI.Presentation;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AssetManager.Pages
{
    public partial class Models : UserControl
    {
        DirectoryInfo dirInfo = new DirectoryInfo("Models");
        int oldVal;

        public Models()
        {
            InitializeComponent();

            if (!dirInfo.Exists)
                dirInfo.Create();

            Tipp.Text = "Choose model type from combo-box!";
            ModelTypes.ItemsSource = dirInfo.EnumerateDirectories();
        }

        private void ModelTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ModelTypes.SelectedIndex == -1)
                ModelTypes.SelectedIndex = oldVal;

            Mesh.Content = null;
            Tipp.Text = "Choose model from list!";
            DirectoryInfo info = new DirectoryInfo(Path.Combine("Models", ModelTypes.SelectedValue.ToString()));
            ModelsList.ItemsSource = info.GetDirectories();

            Export.IsEnabled = false;
        }
        private void ModelsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            oldVal = ModelTypes.SelectedIndex;

            Global.Model = null;

            geoVert.Text = string.Empty;
            textVert.Text = string.Empty;
            vertNorm.Text = string.Empty;
            paramVert.Text = string.Empty;
            point.Text = string.Empty;
            line.Text = string.Empty;
            face.Text = string.Empty;
            size.Text = string.Empty;

            if (ModelsList.SelectedItems.Count != 0)
            {
                Mesh.Content = null;
                Tipp.Text = "Choose available model!";

                DeleteModel.IsEnabled = true;
                Export.IsEnabled = true;

                if (ModelsList.SelectedItems.Count != 0 && ModelTypes.SelectedItem != null)
                {
                    Tipp.Text = string.Empty;
                    DirectoryInfo info = new DirectoryInfo(Path.Combine("Models", ModelTypes.SelectedValue.ToString(), ModelsList.SelectedValue.ToString()));

                    if (info.GetFiles().Count(x => x.Extension.ToLower() == ".obj") > 0)
                    {
                        try
                        {
                            Global.Model = new Model(info.GetFiles().FirstOrDefault(x => x.Extension.ToLower() == ".obj"));
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }

                        geoVert.Text = Global.Model.GeomatryVertices;
                        textVert.Text = Global.Model.TextureVertices;
                        vertNorm.Text = Global.Model.VertexNormals;
                        paramVert.Text = Global.Model.ParameterSpaceVertices;
                        point.Text = Global.Model.Points;
                        line.Text = Global.Model.Lines;
                        face.Text = Global.Model.Faces;
                        size.Text = Global.Model.Size;

                        Mesh.Content = Global.Model.GetModel();
                    }
                    else
                    {
                        Tipp.Text = "No models available in this type!";
                    }
                }
            }
            else
            {
                DeleteModel.IsEnabled = false;
                Export.IsEnabled = false;
            }
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            Global.Model.ExportFile(ModelsList.SelectedValue.ToString(), "_");
            foreach (var material in Global.Model.Material)
            {
                material.ExportFile(string.Empty, string.Empty);
                foreach (var texture in material.Textures)
                {
                    texture.ExportFile(string.Empty, string.Empty);
                }
            }
        }

        private void ImportModel_Click(object sender, RoutedEventArgs e)
        {
            AppearanceManager.Current.ThemeSource = new Link { DisplayName = "light", Source = AppearanceManager.LightThemeSource }.Source;

            Mesh.Content = null;
            oldVal = ModelTypes.SelectedIndex;

            ImportModel mod = new ImportModel();
            mod.ShowDialog();

            ModelTypes.ItemsSource = dirInfo.EnumerateDirectories();

            AppearanceManager.Current.ThemeSource = new Link { DisplayName = "dark", Source = AppearanceManager.DarkThemeSource }.Source;
        }
        private void DeleteModel_Click(object sender, RoutedEventArgs e)
        {
            Mesh.Content = null;
            DirectoryInfo dir = new DirectoryInfo(Path.Combine("Models", ModelTypes.SelectedValue.ToString(), ModelsList.SelectedValue.ToString()) + "\\");
            if (dir.Exists)
            {
                SetAttributesNormal(dir);
                dir.Delete(true);
            }
            ModelTypes.ItemsSource = dirInfo.EnumerateDirectories();
            ModelsList_SelectionChanged(this, null);
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

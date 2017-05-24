using HelixToolkit.Wpf;
using System.IO;
using System.Windows;
using System.Windows.Media.Media3D;

namespace AssetManager
{
    public partial class Show3D : Window
    {
        Model3DGroup loadedModel;
        private string path { get; set; }

        public Show3D(string path)
        {
            InitializeComponent();
            DataContext = this;
            this.path = path;

            BoxRadio.IsChecked = true;
        }

        public void LoadModel(string radio)
        {
            ModelImporter importer = new ModelImporter();
            loadedModel = new Model3DGroup();
            Model3D mod = importer.Load(Path.Combine(path, radio));
            loadedModel.Children.Add(mod);

            mesh.Content = loadedModel;
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            File.Delete(Path.Combine(path, "box.3DS"));
            File.Delete(Path.Combine(path, "sphere.3DS"));
            File.Delete(Path.Combine(path, "tube.3DS"));
            File.Delete(Path.Combine(path, "teapot.3DS"));
        }

        private void BoxRadio_Checked(object sender, RoutedEventArgs e)
        {
            LoadModel("box.3DS");
        }

        private void SphereRadio_Checked(object sender, RoutedEventArgs e)
        {
            LoadModel("sphere.3DS");
        }

        private void TubeRadio_Checked(object sender, RoutedEventArgs e)
        {
            LoadModel("tube.3DS");
        }

        private void TeapotRadio_Checked(object sender, RoutedEventArgs e)
        {
            LoadModel("teapot.3DS");
        }
    }
}

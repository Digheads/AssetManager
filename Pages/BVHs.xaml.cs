using AssetManager.Common;
using FirstFloor.ModernUI.Presentation;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AssetManager.Pages
{
    public partial class BVHs : UserControl
    {
        DirectoryInfo dirInfo = new DirectoryInfo("Motions");
        int oldVal;

        public BVHs()
        {
            InitializeComponent();

            if (!dirInfo.Exists)
                dirInfo.Create();

            Tipp.Text = "Choose motion type from combo-box!";
            BVHTypes.ItemsSource = dirInfo.EnumerateDirectories();

            DataContext = new AppVM();

            viewport.Children.Add(((AppVM)DataContext).RootVisual3D);
            ((AppVM)DataContext).RootVisual3D.Children.Clear();
        }

        private void BVHTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Global.Motion = null;

            if (BVHTypes.SelectedIndex == -1)
                BVHTypes.SelectedIndex = oldVal;

            Tipp.Text = "Choose motion from list!";
            DirectoryInfo info = new DirectoryInfo(Path.Combine("Motions", BVHTypes.SelectedValue.ToString()));
            BVHsList.ItemsSource = info.GetDirectories();

            Export.IsEnabled = false;
        }
        private void BVHsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            oldVal = BVHTypes.SelectedIndex;

            if (BVHsList.SelectedItems.Count != 0)
            {
                Tipp.Text = "Choose available motion!";

                DeleteBVH.IsEnabled = true;
                Export.IsEnabled = true;

                if (BVHsList.SelectedItems.Count != 0 && BVHTypes.SelectedItem != null)
                {
                    Tipp.Text = string.Empty;
                    DirectoryInfo info = new DirectoryInfo(Path.Combine("Motions", BVHTypes.SelectedValue.ToString(), BVHsList.SelectedValue.ToString()));

                    if (info.GetFiles().Count(x => x.Extension.ToLower() == ".bvh") > 0)
                    {
                        ((AppVM)DataContext).LoadBVHFileCommand.Execute(info.GetFiles().FirstOrDefault(x => x.Extension.ToLower() == ".bvh"));
                        Global.Motion = new Motion(info.GetFiles().FirstOrDefault(x => x.Extension.ToLower() == ".bvh"));
                    }
                    else
                    {
                        Tipp.Text = "No motions available in this type!";
                    }
                }
            }
            else
            {
                DeleteBVH.IsEnabled = false;
                Export.IsEnabled = false;
            }
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            Global.Motion.ExportFile(BVHsList.SelectedValue.ToString(), "_");
        }

        private void ImportBVH_Click(object sender, RoutedEventArgs e)
        {
            AppearanceManager.Current.ThemeSource = new Link { DisplayName = "light", Source = AppearanceManager.LightThemeSource }.Source;

            oldVal = BVHTypes.SelectedIndex;

            ImportMotion mot = new ImportMotion();
            mot.ShowDialog();

            BVHTypes.ItemsSource = dirInfo.EnumerateDirectories();

            AppearanceManager.Current.ThemeSource = new Link { DisplayName = "dark", Source = AppearanceManager.DarkThemeSource }.Source;
        }
        private void DeleteBVH_Click(object sender, RoutedEventArgs e)
        {
            Global.Motion = null;
            DirectoryInfo dir = new DirectoryInfo(Path.Combine("Motions", BVHTypes.SelectedValue.ToString(), BVHsList.SelectedValue.ToString()) + "\\");
            if (dir.Exists)
            {
                SetAttributesNormal(dir);
                dir.Delete(true);
            }
            BVHTypes.ItemsSource = dirInfo.EnumerateDirectories();
            BVHsList_SelectionChanged(this, null);
            ((AppVM)DataContext).Animator.ClearCommand.Execute(null);
            ((AppVM)DataContext).RootVisual3D.Children.Clear();
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

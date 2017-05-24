using AssetManager.Common;
using FirstFloor.ModernUI.Presentation;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AssetManager.Pages
{
    public partial class Packages : UserControl
    {
        DirectoryInfo dirInfo = new DirectoryInfo("Packages");
        int oldVal;

        public Packages()
        {
            InitializeComponent();

            if (!dirInfo.Exists)
                dirInfo.Create();

            Tipp.Text = "Choose Package type from combo-box!";
            PackageTypes.ItemsSource = dirInfo.EnumerateDirectories();
        }

        private void PackageTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PackageTypes.SelectedIndex == -1)
                PackageTypes.SelectedIndex = oldVal;

            Preview.Text = string.Empty;
            Tipp.Text = "Choose Package from list!";
            DirectoryInfo info = new DirectoryInfo(Path.Combine("Packages", PackageTypes.SelectedValue.ToString()));
            PackagesList.ItemsSource = info.GetDirectories();

            Export.IsEnabled = false;
        }

        private void PackagesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            oldVal = PackageTypes.SelectedIndex;

            if (PackagesList.SelectedItems.Count != 0)
            {
                Preview.Text = string.Empty;
                Tipp.Text = "Choose available Package!";

                DeletePackage.IsEnabled = true;

                if (PackagesList.SelectedItems.Count != 0 && PackageTypes.SelectedItem != null)
                {
                    DirectoryInfo info = new DirectoryInfo(Path.Combine("Packages", PackageTypes.SelectedValue.ToString(), PackagesList.SelectedValue.ToString()));
                    if (info.GetFiles().Count(x => x.Extension.ToLower() == ".unitypackage") > 0)
                    {
                        if (info.GetFiles().Count(x => x.Extension.ToLower() == ".txt") > 0)
                        {
                            Global.Package = new Package(info.GetFiles().FirstOrDefault(x => x.Extension.ToLower() == ".unitypackage"), info.GetFiles().FirstOrDefault(x => x.Extension.ToLower() == ".txt"));

                            Tipp.Text = string.Empty;
                            Preview.Text = Global.Package.ReadmeFile.ReadFile();
                        }
                        else
                        {
                            Global.Package = new Package(info.GetFiles().FirstOrDefault(x => x.Extension.ToLower() == ".unitypackage"));
                            Tipp.Text = "Preview not available!";
                        }
                    }
                    else
                    {
                        Tipp.Text = "No Package available in this type!";
                    }

                    Export.IsEnabled = true;
                }
            }
            else
                DeletePackage.IsEnabled = false;
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            Global.Package.ExportFile(PackagesList.SelectedValue.ToString(), "_");
        }

        private void ImportPackage_Click(object sender, RoutedEventArgs e)
        {
            AppearanceManager.Current.ThemeSource = new Link { DisplayName = "light", Source = AppearanceManager.LightThemeSource }.Source;

            Preview.Text = string.Empty;
            oldVal = PackageTypes.SelectedIndex;

            ImportPackage pack = new ImportPackage();
            pack.ShowDialog();

            PackageTypes.ItemsSource = dirInfo.EnumerateDirectories();

            AppearanceManager.Current.ThemeSource = new Link { DisplayName = "dark", Source = AppearanceManager.DarkThemeSource }.Source;
        }
        private void DeletePackage_Click(object sender, RoutedEventArgs e)
        {
            Preview.Text = string.Empty;
            DirectoryInfo dir = new DirectoryInfo(Path.Combine("Packages", PackageTypes.SelectedValue.ToString(), PackagesList.SelectedValue.ToString()) + "\\");
            if (dir.Exists)
            {
                SetAttributesNormal(dir);
                dir.Delete(true);
            }
            PackageTypes.ItemsSource = dirInfo.EnumerateDirectories();
            PackagesList_SelectionChanged(this, null);
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

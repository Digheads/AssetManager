using AssetManager.Common;
using FirstFloor.ModernUI.Presentation;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AssetManager.Pages
{
    public partial class PSDs : UserControl
    {
        DirectoryInfo dirInfo = new DirectoryInfo("PSDs");
        int oldVal;

        public PSDs()
        {
            InitializeComponent();

            if (!dirInfo.Exists)
                dirInfo.Create();

            Tipp.Text = "Choose PSD type from combo-box!";
            PSDTypes.ItemsSource = dirInfo.EnumerateDirectories();
        }

        private void PSDTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PSDTypes.SelectedIndex == -1)
                PSDTypes.SelectedIndex = oldVal;

            Preview.Source = null;
            Tipp.Text = "Choose PSD from list!";
            DirectoryInfo info = new DirectoryInfo(Path.Combine("PSDs", PSDTypes.SelectedValue.ToString()));
            PSDsList.ItemsSource = info.GetDirectories();

            Export.IsEnabled = false;
        }

        private void PSDsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            oldVal = PSDTypes.SelectedIndex;

            if (PSDsList.SelectedItems.Count != 0)
            {
                Preview.Source = null;
                Tipp.Text = "Choose available PSD!";

                DeletePSD.IsEnabled = true;

                if (PSDsList.SelectedItems.Count != 0 && PSDTypes.SelectedItem != null)
                {
                    DirectoryInfo info = new DirectoryInfo(Path.Combine("PSDs", PSDTypes.SelectedValue.ToString(), PSDsList.SelectedValue.ToString()));
                    if (info.GetFiles().Count(x => x.Extension.ToLower() == ".psd") > 0)
                    {
                        if (info.GetFiles().Count(x => x.Extension.ToLower() == ".jpg" || x.Extension.ToLower() == ".png" || x.Extension.ToLower() == ".jpeg" || x.Extension.ToLower() == ".bmp") > 0)
                        {
                            Global.PSD = new PSD(info.GetFiles().FirstOrDefault(x => x.Extension.ToLower() == ".psd"), info.GetFiles().FirstOrDefault(x => x.Extension.ToLower() == ".jpg" || x.Extension.ToLower() == ".png" || x.Extension.ToLower() == ".jpeg" || x.Extension.ToLower() == ".bmp"));

                            Tipp.Text = string.Empty;
                            Preview.Source = Global.PSD.PreviewFile.GetImage();
                        }
                        else
                        {
                            Global.PSD = new PSD(info.GetFiles().FirstOrDefault(x => x.Extension.ToLower() == ".psd"));
                            Tipp.Text = "Preview not available!";
                        }
                    }
                    else
                    {
                        Tipp.Text = "No PSD available in this type!";
                    }

                    Export.IsEnabled = true;
                }
            }
            else
                DeletePSD.IsEnabled = false;
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            Global.PSD.ExportFile(PSDsList.SelectedValue.ToString(), "_");
        }

        private void ImportPSD_Click(object sender, RoutedEventArgs e)
        {
            AppearanceManager.Current.ThemeSource = new Link { DisplayName = "light", Source = AppearanceManager.LightThemeSource }.Source;

            Preview.Source = null;
            oldVal = PSDTypes.SelectedIndex;

            ImportPSD subs = new ImportPSD();
            subs.ShowDialog();

            PSDTypes.ItemsSource = dirInfo.EnumerateDirectories();

            AppearanceManager.Current.ThemeSource = new Link { DisplayName = "dark", Source = AppearanceManager.DarkThemeSource }.Source;
        }
        private void DeletePSD_Click(object sender, RoutedEventArgs e)
        {
            Preview.Source = null;
            DirectoryInfo dir = new DirectoryInfo(Path.Combine("PSDs", PSDTypes.SelectedValue.ToString(), PSDsList.SelectedValue.ToString()) + "\\");
            if (dir.Exists)
            {
                SetAttributesNormal(dir);
                dir.Delete(true);
            }
            PSDTypes.ItemsSource = dirInfo.EnumerateDirectories();
            PSDsList_SelectionChanged(this, null);
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

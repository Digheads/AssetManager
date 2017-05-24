using AssetManager.Common;
using FirstFloor.ModernUI.Presentation;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AssetManager.Pages
{
    public partial class Fonts : UserControl
    {
        DirectoryInfo dirInfo = new DirectoryInfo("Fonts");
        int oldVal;

        public Fonts()
        {
            InitializeComponent();

            if (!dirInfo.Exists)
                dirInfo.Create();

            Tipp.Text = "Choose Font type from combo-box!";
            FontTypes.ItemsSource = dirInfo.EnumerateDirectories();
        }

        private void FontTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FontTypes.SelectedIndex == -1)
                FontTypes.SelectedIndex = oldVal;

            Tipp.Text = "Choose Font from list!";
            DirectoryInfo info = new DirectoryInfo(Path.Combine("Fonts", FontTypes.SelectedValue.ToString()));
            FontsList.ItemsSource = info.GetDirectories();

            Export.IsEnabled = false;
        }

        private void FontsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            oldVal = FontTypes.SelectedIndex;

            if (Global.Font != null)
                Global.Font.Dispose();

            if (FontsList.SelectedItems.Count != 0)
            {
                Tipp.Text = "Choose available Font!";

                DeleteFont.IsEnabled = true;

                if (FontsList.SelectedItems.Count != 0 && FontTypes.SelectedItem != null)
                {
                    DirectoryInfo info = new DirectoryInfo(Path.Combine("Fonts", FontTypes.SelectedValue.ToString(), FontsList.SelectedValue.ToString()));
                    if (info.GetFiles().Count(x => x.Extension.ToLower() == ".ttf") > 0)
                    {
                        Global.Font = new Font(info.GetFiles().FirstOrDefault(x => x.Extension.ToLower() == ".ttf"));

                        Preview.Font = Global.Font.GetFont();
                        Preview.Text = "TEST TEXT";
                        Tipp.Text = string.Empty;
                    }
                    else
                    {
                        Tipp.Text = "No Font available in this type!";
                    }

                    Export.IsEnabled = true;
                }
            }
            else
            {
                DeleteFont.IsEnabled = false;
                Export.IsEnabled = false;
            }
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            Global.Font.ExportFile(FontsList.SelectedValue.ToString(), "_");
        }

        private void ImportFont_Click(object sender, RoutedEventArgs e)
        {
            AppearanceManager.Current.ThemeSource = new Link { DisplayName = "light", Source = AppearanceManager.LightThemeSource }.Source;

            oldVal = FontTypes.SelectedIndex;

            ImportFont font = new ImportFont();
            font.ShowDialog();

            FontTypes.ItemsSource = dirInfo.EnumerateDirectories();

            AppearanceManager.Current.ThemeSource = new Link { DisplayName = "dark", Source = AppearanceManager.DarkThemeSource }.Source;
        }
        private void DeleteFont_Click(object sender, RoutedEventArgs e)
        {
            Global.Font.Dispose();

            DirectoryInfo dir = new DirectoryInfo(Path.Combine("Fonts", FontTypes.SelectedValue.ToString(), FontsList.SelectedValue.ToString()) + "\\");
            if (dir.Exists)
            {
                SetAttributesNormal(dir);
                dir.Delete(true);
            }
            FontTypes.ItemsSource = dirInfo.EnumerateDirectories();
            FontsList_SelectionChanged(this, null);
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

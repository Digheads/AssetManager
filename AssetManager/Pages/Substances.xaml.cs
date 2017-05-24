using AssetManager.Common;
using FirstFloor.ModernUI.Presentation;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AssetManager.Pages
{
    public partial class Substances : UserControl
    {
        DirectoryInfo dirInfo = new DirectoryInfo("Substances");
        int oldVal;

        public Substances()
        {
            InitializeComponent();

            if (!dirInfo.Exists)
                dirInfo.Create();

            Tipp.Text = "Choose substance type from combo-box!";
            SubstanceTypes.ItemsSource = dirInfo.EnumerateDirectories();
        }

        private void SubstanceTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SubstanceTypes.SelectedIndex == -1)
                SubstanceTypes.SelectedIndex = oldVal;

            Preview.Source = null;
            Tipp.Text = "Choose substance from list!";
            DirectoryInfo info = new DirectoryInfo(Path.Combine("Substances", SubstanceTypes.SelectedValue.ToString()));
            SubstancesList.ItemsSource = info.GetDirectories();

            Export.IsEnabled = false;
        }

        private void SubstancesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            oldVal = SubstanceTypes.SelectedIndex;

            if (SubstancesList.SelectedItems.Count != 0)
            {
                Preview.Source = null;
                Tipp.Text = "Choose available substance!";

                DeleteSubstance.IsEnabled = true;

                if (SubstancesList.SelectedItems.Count != 0 && SubstanceTypes.SelectedItem != null)
                {
                    DirectoryInfo info = new DirectoryInfo(Path.Combine("Substances", SubstanceTypes.SelectedValue.ToString(), SubstancesList.SelectedValue.ToString()));
                    if (info.GetFiles().Count(x => x.Extension.ToLower() == ".sbsar") > 0)
                    {
                        if (info.GetFiles().Count(x => x.Extension.ToLower() == ".jpg" || x.Extension.ToLower() == ".png" || x.Extension.ToLower() == ".jpeg" || x.Extension.ToLower() == ".bmp") > 0)
                        {
                            Global.Substance = new Substance(info.GetFiles().FirstOrDefault(x => x.Extension.ToLower() == ".sbsar"), info.GetFiles().FirstOrDefault(x => x.Extension.ToLower() == ".jpg" || x.Extension.ToLower() == ".png" || x.Extension.ToLower() == ".jpeg" || x.Extension.ToLower() == ".bmp"));

                            Tipp.Text = string.Empty;
                            Preview.Source = Global.Substance.PreviewFile.GetImage();
                        }
                        else
                        {
                            Global.Substance = new Substance(info.GetFiles().FirstOrDefault(x => x.Extension.ToLower() == ".sbsar"));
                            Tipp.Text = "Preview not available!";
                        }
                    }
                    else
                    {
                        Tipp.Text = "No substance available in this type!";
                    }

                    Export.IsEnabled = true;
                }
            }
            else
                DeleteSubstance.IsEnabled = false;
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            Global.Substance.ExportFile(SubstancesList.SelectedValue.ToString(), "_");
        }

        private void ImportSubstance_Click(object sender, RoutedEventArgs e)
        {
            AppearanceManager.Current.ThemeSource = new Link { DisplayName = "light", Source = AppearanceManager.LightThemeSource }.Source;

            Preview.Source = null;
            oldVal = SubstanceTypes.SelectedIndex;

            ImportSubstance subs = new ImportSubstance();
            subs.ShowDialog();

            SubstanceTypes.ItemsSource = dirInfo.EnumerateDirectories();

            AppearanceManager.Current.ThemeSource = new Link { DisplayName = "dark", Source = AppearanceManager.DarkThemeSource }.Source;
        }
        private void DeleteSubstance_Click(object sender, RoutedEventArgs e)
        {
            Preview.Source = null;
            DirectoryInfo dir = new DirectoryInfo(Path.Combine("Substances", SubstanceTypes.SelectedValue.ToString(), SubstancesList.SelectedValue.ToString()) + "\\");
            if (dir.Exists)
            {
                SetAttributesNormal(dir);
                dir.Delete(true);
            }
            SubstanceTypes.ItemsSource = dirInfo.EnumerateDirectories();
            SubstancesList_SelectionChanged(this, null);
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

using AssetManager.Common;
using FirstFloor.ModernUI.Presentation;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AssetManager.Pages
{
    public partial class Scripts : UserControl
    {
        DirectoryInfo dirInfo = new DirectoryInfo("Scripts");
        int oldVal;

        public Scripts()
        {
            InitializeComponent();

            if (!dirInfo.Exists)
                dirInfo.Create();

            Tipp.Text = "Choose Script type from combo-box!";
            ScriptTypes.ItemsSource = dirInfo.EnumerateDirectories();
        }

        private void ScriptTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ScriptTypes.SelectedIndex == -1)
                ScriptTypes.SelectedIndex = oldVal;

            ScriptData.Text = string.Empty;
            Tipp.Text = "Choose Script from list!";
            DirectoryInfo info = new DirectoryInfo(Path.Combine("Scripts", ScriptTypes.SelectedValue.ToString()));
            ScriptsList.ItemsSource = info.GetDirectories();

            Export.IsEnabled = false;
        }

        private void ScriptsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            oldVal = ScriptTypes.SelectedIndex;

            if (ScriptsList.SelectedItems.Count != 0)
            {
                ScriptData.Text = string.Empty;
                Tipp.Text = "Choose available Script!";

                DeleteScript.IsEnabled = true;

                if (ScriptsList.SelectedItems.Count != 0 && ScriptTypes.SelectedItem != null)
                {
                    DirectoryInfo info = new DirectoryInfo(Path.Combine("Scripts", ScriptTypes.SelectedValue.ToString(), ScriptsList.SelectedValue.ToString()));
                    if (info.GetFiles().Count(x => x.Extension.ToLower() == ".cs" || x.Extension.ToLower() == ".js" || x.Extension.ToLower() == ".cpp" || x.Extension.ToLower() == ".pas") > 0)
                    {
                        Global.Script = new Script(info.GetFiles().FirstOrDefault(x => x.Extension.ToLower() == ".cs" || x.Extension.ToLower() == ".js" || x.Extension.ToLower() == ".cpp" || x.Extension.ToLower() == ".pas"));

                        Tipp.Text = string.Empty;
                        ScriptData.Text = Global.Script.ReadFile();
                    }
                    else
                    {
                        Tipp.Text = "No Script available in this type!";
                    }

                    Export.IsEnabled = true;
                }
                else
                    DeleteScript.IsEnabled = false;
            }
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            Global.Script.ExportFile(ScriptsList.SelectedValue.ToString(), "_");
        }

        private void ImportScript_Click(object sender, RoutedEventArgs e)
        {
            AppearanceManager.Current.ThemeSource = new Link { DisplayName = "light", Source = AppearanceManager.LightThemeSource }.Source;

            ScriptData.Text = string.Empty;
            oldVal = ScriptTypes.SelectedIndex;

            ImportScript scr = new ImportScript();
            scr.ShowDialog();

            ScriptTypes.ItemsSource = dirInfo.EnumerateDirectories();

            AppearanceManager.Current.ThemeSource = new Link { DisplayName = "dark", Source = AppearanceManager.DarkThemeSource }.Source;
        }
        private void DeleteScript_Click(object sender, RoutedEventArgs e)
        {
            ScriptData.Text = string.Empty;
            DirectoryInfo dir = new DirectoryInfo(Path.Combine("Scripts", ScriptTypes.SelectedValue.ToString(), ScriptsList.SelectedValue.ToString()) + "\\");
            if (dir.Exists)
            {
                SetAttributesNormal(dir);
                dir.Delete(true);
            }
            ScriptTypes.ItemsSource = dirInfo.EnumerateDirectories();
            ScriptsList_SelectionChanged(this, null);
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

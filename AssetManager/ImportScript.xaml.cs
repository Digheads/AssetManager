using AssetManager.Common;
using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace AssetManager
{
    public partial class ImportScript : Window
    {
        private Script addedScript;

        public ImportScript()
        {
            InitializeComponent();
            DataContext = this;

            DirectoryInfo dirInfo = new DirectoryInfo("Scripts");
            ScriptTypes.ItemsSource = dirInfo.EnumerateDirectories();
        }

        private void BrowseScript_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog()
            {
                Multiselect = false,
                DefaultExt = ".cs",
                Filter = "C Sharp Script (.cs)|*.cs|JavaScript (.js)|*.js|C Plus Plus (.cpp)|*.cpp|Pascal Script (.pas)|*.pas"
            };
            bool? result = openfile.ShowDialog();

            if (result == true)
            {
                addedScript = new Script(new FileInfo(openfile.FileName));
                BrowsedScript.Text = openfile.FileName;
                BrowsedPreview.Text = addedScript.ReadFile();
            }
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (addedScript != null)
            {
                string newDir = string.Empty;
                if (!string.IsNullOrWhiteSpace(ScriptTypes.Text) && !string.IsNullOrWhiteSpace(ScriptName.Text))
                {
                    newDir = ScriptTypes.Text;
                }
                else
                {
                    MessageBox.Show("Script type and name cannot be empty!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                DirectoryInfo dirInfo = new DirectoryInfo(Path.Combine("Scripts", newDir, ScriptName.Text));
                if (!dirInfo.Exists)
                    Directory.CreateDirectory(Path.Combine("Scripts", newDir, ScriptName.Text));
                else
                {
                    MessageBoxResult result = MessageBox.Show("Script already exists: " + ScriptName.Text + "! Do you want to overwrite it's content?", "Import Script", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
                    if (result == MessageBoxResult.Yes)
                    {
                        dirInfo.Delete(true);
                        dirInfo.Create();
                    }
                    else
                        return;
                }

                File.Copy(addedScript.FullName, Path.Combine(dirInfo.FullName, addedScript.Name));

                this.Hide();
            }
            else
            {
                MessageBox.Show("No Script was selected. Please, fix it!", "Import Script", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}

using AssetManager.Common;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace AssetManager
{
    public partial class ImportModel : Window
    {
        private Model AddedItem;

        public ImportModel()
        {
            InitializeComponent();
            DataContext = this;

            DirectoryInfo dirInfo = new DirectoryInfo("Models");
            ModelTypes.ItemsSource = dirInfo.EnumerateDirectories();
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog()
            {
                DefaultExt = ".obj",
                Filter = "Wavefront OBJ models (.obj)|*.obj"
            };
            bool? result = openfile.ShowDialog();

            if (result == true)
            {
                AddedItem = new Model(new FileInfo(openfile.FileName));
                FilePath.Text = openfile.FileName;

                try
                {
                    Mesh.Content = AddedItem.GetModel();
                    Ok.IsEnabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Ok.IsEnabled = false;
                }
            }
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            string newDir = string.Empty;
            if (!string.IsNullOrWhiteSpace(ModelTypes.Text) && !string.IsNullOrWhiteSpace(ModelName.Text))
            {
                newDir = ModelTypes.Text;
            }
            else
            {
                MessageBox.Show("Model type and name cannot be empty!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DirectoryInfo dirInfo = new DirectoryInfo(Path.Combine("Models", newDir, ModelName.Text));
            if (!dirInfo.Exists)
            {
                Directory.CreateDirectory(Path.Combine("Models", newDir, ModelName.Text));
                Directory.CreateDirectory(Path.Combine("Models", newDir, ModelName.Text, "textures"));
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Model already exists: " + ModelName.Text + "! Do you want to overwrite it's content?", "Import Model", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
                if (result == MessageBoxResult.Yes)
                {
                    dirInfo.Delete(true);
                    dirInfo.Create();
                }
                else
                    return;
            }

            File.Copy(AddedItem.FullName, Path.Combine(dirInfo.FullName, AddedItem.Name));
            ReplacePath(dirInfo.FullName, Path.Combine(dirInfo.FullName, AddedItem.Name), FileType.Material);

            foreach (var mat in AddedItem.Material)
            {
                File.Copy(mat.FullName, Path.Combine(dirInfo.FullName, mat.Name));

                foreach (var text in mat.Textures)
                {
                    File.Copy(text.FullName, Path.Combine(dirInfo.FullName, "textures", text.Name));
                }

                ReplacePath(dirInfo.FullName, Path.Combine(dirInfo.FullName, mat.Name), FileType.Texture);
            }

            this.Closed += ImportModel_Closed;
            this.Close();
        }

        private void ImportModel_Closed(object sender, EventArgs e)
        {
            GC.Collect();
        }

        private void ReplacePath(string mainPath, string objPath, FileType type)
        {
            string toFind = string.Empty;
            string toFind2 = string.Empty;
            string dirName = string.Empty;

            switch (type)
            {
                case FileType.Model: return;
                //break;
                case FileType.Material:
                    toFind = "mtllib ";
                    break;
                case FileType.Texture:
                    toFind = "map_";
                    toFind = "bump";
                    dirName = "textures";
                    break;
                default: return;
                    //break;
            }

            FileInfo file = new FileInfo(objPath);
            List<string> lines = new List<string>();

            using (StreamReader stream = file.OpenText())
            {
                lines.AddRange(stream.ReadToEnd().Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
            }

            File.WriteAllText(file.FullName, string.Empty);
            using (StreamWriter writer = new StreamWriter(file.OpenWrite()))
            {
                foreach (string item in lines)
                {
                    string fullname = item.Trim();
                    if (fullname.StartsWith(toFind) || fullname.StartsWith(toFind2))
                    {
                        string str = RemoveLeading(fullname);
                        for (int i = 0; i < fullname.Count(x => x == ' '); i++)
                        {
                            if (File.Exists(Path.Combine(mainPath, dirName, str)))
                            {
                                fullname = item.Remove(item.IndexOf(str)) + Path.Combine(dirName, str);
                                break;
                            }
                            else
                            {
                                str = RemoveLeading(str);
                            }
                        }
                    }

                    writer.WriteLine(fullname);
                    writer.Flush();
                }
            }
        }

        private string RemoveLeading(string s)
        {
            return s.Remove(0, s.IndexOf(' ') + 1);
        }
    }
}

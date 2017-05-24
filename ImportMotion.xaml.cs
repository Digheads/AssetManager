using AssetManager.Common;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;

namespace AssetManager
{
    public partial class ImportMotion : Window
    {
        private Motion AddedItem;

        public ImportMotion()
        {
            InitializeComponent();
            DataContext = this;

            DirectoryInfo dirInfo = new DirectoryInfo("Motions");
            MotionTypes.ItemsSource = dirInfo.EnumerateDirectories();

            DataContext = new AppVM();

            viewport.Children.Add(((AppVM)DataContext).RootVisual3D);
        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog()
            {
                DefaultExt = ".bvh",
                Filter = "Biovision Hierarchy (.bvh)|*.bvh"
            };
            bool? result = openfile.ShowDialog();

            if (result == true)
            {
                AddedItem = new Motion(new FileInfo(openfile.FileName));
                FilePath.Text = openfile.FileName;

                try
                {
                    ((AppVM)DataContext).LoadBVHFileCommand.Execute(new FileInfo(FilePath.Text));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            string newDir = string.Empty;
            if (!string.IsNullOrWhiteSpace(MotionTypes.Text) && !string.IsNullOrWhiteSpace(MotionName.Text))
            {
                newDir = MotionTypes.Text;
            }
            else
            {
                MessageBox.Show("Motion type and name cannot be empty!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DirectoryInfo dirInfo = new DirectoryInfo(Path.Combine("Motions", newDir, MotionName.Text));
            if (!dirInfo.Exists)
                Directory.CreateDirectory(Path.Combine("Motions", newDir, MotionName.Text));
            else
            {
                MessageBoxResult result = MessageBox.Show("Motion already exists: " + MotionName.Text + "! Do you want to overwrite it's content?", "Import Motion", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
                if (result == MessageBoxResult.Yes)
                {
                    dirInfo.Delete(true);
                    dirInfo.Create();
                }
                else
                    return;
            }

            File.Copy(AddedItem.FullName, Path.Combine(dirInfo.FullName, AddedItem.Name));

            this.Closed += ImportMotion_Closed;
            this.Close();
        }

        private void ImportMotion_Closed(object sender, EventArgs e)
        {
            GC.Collect();
        }
    }
}

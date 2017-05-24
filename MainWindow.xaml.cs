using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.IO;
using System.Windows;

namespace AssetManager
{
    public partial class MainWindow : ModernWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            DirectoryInfo dir = new DirectoryInfo("Temp");
            if (dir.Exists)
            {
                SetAttributesNormal(dir);
                dir.Delete(true);
            }
        }

        private void ModernWindow_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
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

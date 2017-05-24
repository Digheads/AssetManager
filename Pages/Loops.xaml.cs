using AssetManager.Common;
using FirstFloor.ModernUI.Presentation;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace AssetManager.Pages
{
    public partial class Loops : UserControl
    {
        DirectoryInfo dirInfo = new DirectoryInfo("Loops");
        int oldVal;

        public Loops()
        {
            InitializeComponent();

            if (!dirInfo.Exists)
                dirInfo.Create();

            Tipp.Text = "Choose loop type from combo-box!";
            LoopTypes.ItemsSource = dirInfo.EnumerateDirectories();
        }

        private void LoopTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Global.Loop != null)
            {
                Global.Loop.Stop();
                Global.Loop.Dispose();
            }

            if (Global.OneShot != null)
            {
                Global.OneShot.Stop();
                Global.OneShot.Dispose();
            }

            if (LoopTypes.SelectedIndex == -1)
                LoopTypes.SelectedIndex = oldVal;

            Tipp.Text = "Choose loop from list!";
            DirectoryInfo info = new DirectoryInfo(Path.Combine("Loops", LoopTypes.SelectedValue.ToString()));
            LoopsList.ItemsSource = info.GetDirectories();

            Export.IsEnabled = false;
            TimeLine.IsEnabled = false;
            PlayButton.IsEnabled = false;
            StopButton.IsEnabled = false;
            IsLooping.IsEnabled = false;
        }

        private void LoopsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            oldVal = LoopTypes.SelectedIndex;

            if (Global.Loop != null)
            {
                Global.Loop.Stop();
                Global.Loop.Dispose();
            }

            if (Global.OneShot != null)
            {
                Global.OneShot.Stop();
                Global.OneShot.Dispose();
            }

            if (LoopsList.SelectedItems.Count != 0)
            {
                Tipp.Text = "Choose available loop!";

                DeleteLoop.IsEnabled = true;

                if (LoopsList.SelectedItems.Count != 0 && LoopTypes.SelectedItem != null)
                {
                    DirectoryInfo info = new DirectoryInfo(Path.Combine("Loops", LoopTypes.SelectedValue.ToString(), LoopsList.SelectedValue.ToString()));
                    if (info.GetFiles().Count(x => x.Extension.ToLower() == ".wav") > 0)
                    {
                        Global.Loop = new Sound(info.GetFiles().FirstOrDefault(x => x.Extension.ToLower() == ".wav"));
                        Tipp.Text = LoopsList.SelectedValue.ToString();

                        if (Global.Loop.IsOpened)
                        {
                            TimeLine.Maximum = Global.Loop.Duration.Seconds;
                            Duration.Text = Global.Loop.Duration.ToString(@"mm\:ss");
                        }
                    }
                    else
                    {
                        Tipp.Text = "No loop available in this type!";
                    }

                    Export.IsEnabled = true;
                    TimeLine.IsEnabled = true;
                    PlayButton.IsEnabled = true;
                    IsLooping.IsEnabled = true;
                }
            }
            else
            {
                DeleteLoop.IsEnabled = false;
                Export.IsEnabled = false;
                TimeLine.IsEnabled = false;
                PlayButton.IsEnabled = false;
                StopButton.IsEnabled = false;
                IsLooping.IsEnabled = false;
            }
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            Global.Loop.ExportFile(LoopsList.SelectedValue.ToString(), "_");
        }

        private void ImportLoop_Click(object sender, RoutedEventArgs e)
        {
            AppearanceManager.Current.ThemeSource = new Link { DisplayName = "light", Source = AppearanceManager.LightThemeSource }.Source;

            oldVal = LoopTypes.SelectedIndex;

            ImportSound sou = new ImportSound(SoundType.Loops);
            sou.ShowDialog();

            LoopTypes.ItemsSource = dirInfo.EnumerateDirectories();

            AppearanceManager.Current.ThemeSource = new Link { DisplayName = "dark", Source = AppearanceManager.DarkThemeSource }.Source;
        }
        private void DeleteLoop_Click(object sender, RoutedEventArgs e)
        {
            Global.Loop.Dispose();

            DirectoryInfo dir = new DirectoryInfo(Path.Combine("Loops", LoopTypes.SelectedValue.ToString(), LoopsList.SelectedValue.ToString()) + "\\");
            if (dir.Exists)
            {
                SetAttributesNormal(dir);
                dir.Delete(true);
            }
            LoopTypes.ItemsSource = dirInfo.EnumerateDirectories();
            LoopsList_SelectionChanged(this, null);
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

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            Global.Loop.Play();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick; ;
            timer.Start();
            PlayButton.IsEnabled = false;
            StopButton.IsEnabled = true;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            TimeLine.Value = Global.Loop.Position.TotalSeconds;
            CurrentPosition.Text = Global.Loop.Position.ToString(@"mm\:ss");

            if (Global.Loop.IsOpened)
            {
                TimeLine.Maximum = Global.Loop.Duration.TotalSeconds;
                Duration.Text = Global.Loop.Duration.ToString(@"mm\:ss");
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            Global.Loop.Stop();

            PlayButton.IsEnabled = true;
            StopButton.IsEnabled = false;
        }

        private void IsLooping_Checked(object sender, RoutedEventArgs e)
        {
            Global.Loop.IsLooping = true;
        }

        private void TimeLine_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Global.Loop.Position = TimeSpan.FromSeconds(TimeLine.Value);
        }

        private void IsLooping_Unchecked(object sender, RoutedEventArgs e)
        {
            Global.Loop.IsLooping = false;
        }
    }
}

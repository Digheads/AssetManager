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
    public partial class OneShots : UserControl
    {
        DirectoryInfo dirInfo = new DirectoryInfo("OneShots");
        int oldVal;

        public OneShots()
        {
            InitializeComponent();

            if (!dirInfo.Exists)
                dirInfo.Create();

            Tipp.Text = "Choose OneShot type from combo-box!";
            OneShotTypes.ItemsSource = dirInfo.EnumerateDirectories();
        }

        private void OneShotTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

            if (OneShotTypes.SelectedIndex == -1)
                OneShotTypes.SelectedIndex = oldVal;

            Tipp.Text = "Choose OneShot from list!";
            DirectoryInfo info = new DirectoryInfo(Path.Combine("OneShots", OneShotTypes.SelectedValue.ToString()));
            OneShotsList.ItemsSource = info.GetDirectories();

            Export.IsEnabled = false;
            TimeLine.IsEnabled = false;
            PlayButton.IsEnabled = false;
            StopButton.IsEnabled = false;
            IsLooping.IsEnabled = false;
        }

        private void OneShotsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            oldVal = OneShotTypes.SelectedIndex;

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

            if (OneShotsList.SelectedItems.Count != 0)
            {
                Tipp.Text = "Choose available OneShot!";

                DeleteOneShot.IsEnabled = true;

                if (OneShotsList.SelectedItems.Count != 0 && OneShotTypes.SelectedItem != null)
                {
                    DirectoryInfo info = new DirectoryInfo(Path.Combine("OneShots", OneShotTypes.SelectedValue.ToString(), OneShotsList.SelectedValue.ToString()));
                    if (info.GetFiles().Count(x => x.Extension.ToLower() == ".wav") > 0)
                    {
                        Global.OneShot = new Sound(info.GetFiles().FirstOrDefault(x => x.Extension.ToLower() == ".wav"));
                        Tipp.Text = OneShotsList.SelectedValue.ToString();

                        if (Global.OneShot.IsOpened)
                        {
                            TimeLine.Maximum = Global.OneShot.Duration.Seconds;
                            Duration.Text = Global.OneShot.Duration.ToString(@"mm\:ss");
                        }
                    }
                    else
                    {
                        Tipp.Text = "No OneShot available in this type!";
                    }

                    Export.IsEnabled = true;
                    TimeLine.IsEnabled = true;
                    PlayButton.IsEnabled = true;
                    IsLooping.IsEnabled = true;
                }
            }
            else
            {
                DeleteOneShot.IsEnabled = false;
                Export.IsEnabled = false;
                TimeLine.IsEnabled = false;
                PlayButton.IsEnabled = false;
                StopButton.IsEnabled = false;
                IsLooping.IsEnabled = false;
            }
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            Global.OneShot.ExportFile(OneShotsList.SelectedValue.ToString(), "_");
        }

        private void ImportOneShot_Click(object sender, RoutedEventArgs e)
        {
            AppearanceManager.Current.ThemeSource = new Link { DisplayName = "light", Source = AppearanceManager.LightThemeSource }.Source;

            oldVal = OneShotTypes.SelectedIndex;

            ImportSound sou = new ImportSound(SoundType.OneShots);
            sou.ShowDialog();

            OneShotTypes.ItemsSource = dirInfo.EnumerateDirectories();

            AppearanceManager.Current.ThemeSource = new Link { DisplayName = "dark", Source = AppearanceManager.DarkThemeSource }.Source;

        }
        private void DeleteOneShot_Click(object sender, RoutedEventArgs e)
        {
            Global.OneShot.Dispose();

            DirectoryInfo dir = new DirectoryInfo(Path.Combine("OneShots", OneShotTypes.SelectedValue.ToString(), OneShotsList.SelectedValue.ToString()) + "\\");
            if (dir.Exists)
            {
                SetAttributesNormal(dir);
                dir.Delete(true);
            }
            OneShotTypes.ItemsSource = dirInfo.EnumerateDirectories();
            OneShotsList_SelectionChanged(this, null);
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
            Global.OneShot.Play();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick; ;
            timer.Start();
            PlayButton.IsEnabled = false;
            StopButton.IsEnabled = true;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            TimeLine.Value = Global.OneShot.Position.TotalSeconds;
            CurrentPosition.Text = Global.OneShot.Position.ToString(@"mm\:ss");

            if (Global.OneShot.IsOpened)
            {
                TimeLine.Maximum = Global.OneShot.Duration.TotalSeconds;
                Duration.Text = Global.OneShot.Duration.ToString(@"mm\:ss");
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            Global.OneShot.Stop();

            PlayButton.IsEnabled = true;
            StopButton.IsEnabled = false;
        }

        private void IsLooping_Checked(object sender, RoutedEventArgs e)
        {
            Global.OneShot.IsLooping = true;
        }

        private void TimeLine_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Global.OneShot.Position = TimeSpan.FromSeconds(TimeLine.Value);
        }

        private void IsLooping_Unchecked(object sender, RoutedEventArgs e)
        {
            Global.OneShot.IsLooping = false;
        }
    }
}

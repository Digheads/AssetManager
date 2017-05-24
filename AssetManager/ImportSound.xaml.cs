using AssetManager.Common;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Threading;

namespace AssetManager
{
    public partial class ImportSound : Window
    {
        private Sound addedSound;
        string type;

        public ImportSound(SoundType type)
        {
            InitializeComponent();
            DataContext = this;

            this.type = type.ToString();
            DirectoryInfo dirInfo = new DirectoryInfo(this.type);
            SoundTypes.ItemsSource = dirInfo.EnumerateDirectories();
        }

        private void BrowseSound_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog()
            {
                Multiselect = false,
                DefaultExt = ".wav",
                Filter = "Waveform Audio File (.wav)|*.wav"
            };
            bool? result = openfile.ShowDialog();

            if (result == true)
            {
                addedSound = new Sound(new FileInfo(openfile.FileName));
                BrowsedSound.Text = openfile.FileName;

                TimeLine.IsEnabled = true;
                PlayButton.IsEnabled = true;
                IsLooping.IsEnabled = true;
            }
            else
            {
                TimeLine.IsEnabled = false;
                PlayButton.IsEnabled = false;
                StopButton.IsEnabled = false;
                IsLooping.IsEnabled = false;
            }
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (addedSound != null)
            {
                string newDir = string.Empty;
                if (!string.IsNullOrWhiteSpace(SoundTypes.Text) && !string.IsNullOrWhiteSpace(SoundName.Text))
                {
                    newDir = SoundTypes.Text;
                }
                else
                {
                    MessageBox.Show("Sound type and name cannot be empty!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                DirectoryInfo dirInfo = new DirectoryInfo(Path.Combine(type, newDir, SoundName.Text));
                if (!dirInfo.Exists)
                    Directory.CreateDirectory(Path.Combine(type, newDir, SoundName.Text));
                else
                {
                    MessageBoxResult result = MessageBox.Show("Sound already exists: " + SoundName.Text + "! Do you want to overwrite it's content?", "Import sound", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
                    if (result == MessageBoxResult.Yes)
                    {
                        dirInfo.Delete(true);
                        dirInfo.Create();
                    }
                    else
                        return;
                }

                File.Copy(addedSound.FullName, Path.Combine(dirInfo.FullName, addedSound.Name));

                addedSound.Stop();
                addedSound.Dispose();
                this.Close();
            }
            else
            {
                MessageBox.Show("No sound was selected. Please, fix it!", "Import sound", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            addedSound.Play();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick; ;
            timer.Start();
            PlayButton.IsEnabled = false;
            StopButton.IsEnabled = true;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            TimeLine.Value = addedSound.Position.TotalSeconds;
            CurrentPosition.Text = addedSound.Position.ToString(@"mm\:ss");

            if (addedSound.IsOpened)
            {
                TimeLine.Maximum = addedSound.Duration.TotalSeconds;
                Duration.Text = addedSound.Duration.ToString(@"mm\:ss");
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            addedSound.Stop();

            PlayButton.IsEnabled = true;
            StopButton.IsEnabled = false;
        }

        private void IsLooping_Checked(object sender, RoutedEventArgs e)
        {
            addedSound.IsLooping = true;
        }

        private void TimeLine_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            addedSound.Position = TimeSpan.FromSeconds(TimeLine.Value);
        }

        private void IsLooping_Unchecked(object sender, RoutedEventArgs e)
        {
            addedSound.IsLooping = false;
        }
    }
}

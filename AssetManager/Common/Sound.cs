using System;
using System.IO;
using System.Windows.Media;

namespace AssetManager.Common
{
    public class Sound : Shared, IDisposable
    {
        MediaPlayer player;

        public TimeSpan Duration { get { if (player.NaturalDuration.HasTimeSpan) return player.NaturalDuration.TimeSpan; else return new TimeSpan(0); } }
        public TimeSpan Position { get { return player.Position; } set { player.Position = value; } }
        public bool? IsLooping { get; set; } = false;
        public bool IsOpened { get; private set; }

        public Sound(FileInfo file) : base(file)
        {
            player = new MediaPlayer();
            player.Open(new Uri(FullName));
            player.MediaOpened += Player_MediaOpened;
            player.MediaEnded += Player_MediaEnded;
        }

        private void Player_MediaOpened(object sender, EventArgs e)
        {
            IsOpened = true;
        }

        public void Play()
        {
            if (player.HasAudio)
                player.Play();
        }

        public void Stop()
        {
            player.Stop();
        }

        private void Player_MediaEnded(object sender, EventArgs e)
        {
            if (IsLooping == true && player.HasAudio)
            {
                Stop();
                Play();
            }
            else
                Stop();
        }

        public void Dispose()
        {
            player.Close();
        }
    }
}

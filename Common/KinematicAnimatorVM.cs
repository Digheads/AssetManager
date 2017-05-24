using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using System.Windows.Threading;

namespace AssetManager.Common
{
    public class KinematicAnimatorVM : INotifyPropertyChanged
    {
        public enum State
        {
            Paused = 0,
            Playback = 1,
        }
        private DispatcherTimer timer;
        private State animatorState = State.Paused;
        private int playbackPosition;

        public double FPS
        {
            get { return MotionData.FPS; }
            set
            {
                if (MotionData.FPS != value)
                {
                    MotionData.FPS = value;

                    value = Math.Max(1, value);
                    TimeSpan interval = TimeSpan.FromSeconds(1.0 / value);
                    timer.Interval = interval;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FPS)));
                }
            }
        }

        public int PlaybackPosition
        {
            get { return playbackPosition; }
            set
            {
                value = Math.Min(Length, Math.Max(1, value));

                if (playbackPosition != value)
                {
                    playbackPosition = value;

                    PlaybackPositionChanged();

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PlaybackPosition)));
                }
            }
        }

        public int Length
        {
            get
            {
                if (MotionData.Data.Count == 0)
                    return 0;

                return MotionData.FrameCount;
            }
        }

        public KinematicVM Kinematic { get; }

        public ICommand PlayCommand { get; }

        public ICommand PauseCommand { get; }

        public ICommand RecordCommand { get; }

        public ICommand ClearCommand { get; }

        public State AnimatorState
        {
            get { return animatorState; }
            set
            {
                if (animatorState != value)
                {
                    animatorState = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AnimatorState)));
                }
            }

        }

        public MotionData MotionData { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public KinematicAnimatorVM(KinematicVM kinematic, MotionData motionData)
        {
            this.Kinematic = kinematic;
            this.MotionData = motionData;

            PlayCommand = new RelayCommand(Play, CanPlay);
            PauseCommand = new RelayCommand(Pause, CanPause);
            ClearCommand = new RelayCommand(ClearData, CanClear);

            timer = new DispatcherTimer(DispatcherPriority.Normal);
            TimeSpan interval = TimeSpan.FromSeconds(1.0 / motionData.FPS);
            timer.Interval = interval;
            timer.Tick += OnTimerTick;
        }


        private void PlaybackPositionChanged()
        {
            if (PlaybackPosition >= Length)
            {
                Pause();
            }

            Dictionary<Bone, Quaternion> currentFramePose = new Dictionary<Bone, Quaternion>();
            foreach (var item in MotionData.Data)
            {
                currentFramePose.Add(item.Key, item.Value[playbackPosition - 1]);
            }

            Kinematic.Model.ApplyLocalRotation(currentFramePose);
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            if (AnimatorState != State.Playback)
                throw new InvalidOperationException("Player state is not Playback/Recording");

            ++PlaybackPosition;
        }

        private void Play()
        {
            AnimatorState = State.Playback;

            timer.Start();
        }

        private bool CanPlay()
        {
            return AnimatorState == State.Paused;
        }

        private void Pause()
        {
            AnimatorState = State.Paused;

            timer.Stop();
        }

        private bool CanPause()
        {
            return AnimatorState == State.Playback;
        }

        private void ClearData()
        {
            MotionData.Data.Clear();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Length)));
        }

        private bool CanClear()
        {
            return AnimatorState == State.Paused;
        }
    }
}

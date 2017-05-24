using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using System.Windows.Threading;

namespace AssetManager.Common
{
    public class AppVM : INotifyPropertyChanged
    {
        private DispatcherTimer refreshTimer;

        private KinematicVM kinematic;

        private KinematicAnimatorVM animator;

        public KinematicVM Kinematic
        {
            get { return kinematic; }
            set
            {
                if (kinematic != value)
                {
                    if (kinematic != null)
                    {
                        RootVisual3D.Children.Remove(kinematic.Root.Visual);
                    }

                    kinematic = value;

                    if (kinematic != null)
                    {
                        RootVisual3D.Children.Add(kinematic.Root.Visual);
                    }

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Kinematic)));
                }
            }
        }

        public KinematicAnimatorVM Animator
        {
            get { return animator; }
            set
            {
                if (animator != value)
                {
                    animator = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Animator)));
                }
            }
        }

        public ModelVisual3D RootVisual3D { get; } = new ModelVisual3D();

        public ICommand LoadBVHFileCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public AppVM()
        {
            // setup kinematic chain
            Kinematic = new KinematicVM(new KinematicStructure());

            // setup animator
            Animator = new KinematicAnimatorVM(Kinematic, new MotionData());

            LoadBVHFileCommand = new RelayCommand<FileInfo>(LoadBVHFile);

            refreshTimer = new DispatcherTimer(DispatcherPriority.Background);
            refreshTimer.Interval = TimeSpan.FromMilliseconds(30);
            refreshTimer.Start();
            refreshTimer.Tick += OnRefreshTick;
        }

        private void LoadBVHFile(FileInfo file)
        {
            BVHMotionData bvhMotionData;
            BVHNode bvhRoot = BVHNode.ReadBVH(file, out bvhMotionData);

            MotionData motionData = new MotionData();
            motionData.FPS = 1.0 / bvhMotionData.FrameTime;

            Bone rootBone = BVHNode.ToBones(bvhRoot, null, bvhMotionData, motionData);
            Kinematic = new KinematicVM(new KinematicStructure(rootBone));
            Animator = new KinematicAnimatorVM(Kinematic, motionData);
        }
        private void OnRefreshTick(object sender, EventArgs e)
        {
            Kinematic.Refresh();
        }

        public static AppVM GetCurrent()
        {
            return (AppVM)Application.Current.FindResource("AppVM");
        }
    }
}

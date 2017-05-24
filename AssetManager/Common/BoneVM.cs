using HelixToolkit.Wpf;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace AssetManager.Common
{
    public class BoneVM : INotifyPropertyChanged, IEnumerable<BoneVM>
    {
        public static double LinkThickness = 3;
        public static Color LinkColor = Colors.DarkGray;

        private CSysVisual3D coordinateSystemVisual;

        private Dictionary<BoneVM, LinesVisual3D> childLinkVisualMap = new Dictionary<BoneVM, LinesVisual3D>();

        public Bone Model { get; set; }
        public BoneVM Parent { get; }

        public string Name
        {
            get { return Model.Name; }
            set
            {
                if (Model.Name != value)
                {
                    Model.Name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                }
            }
        }

        public bool IsEndSite { get { return Model.IsEndSite; } }
        public Vector3D Offset
        {
            get { return Model.Offset; }
            set
            {
                Model.Offset = value;
                Parent?.UpdateLinkVisual(this);
            }
        }

        public Quaternion LocalRotation { get { return Model.JointRotation; } set { Model.JointRotation = value; } }

        public ModelVisual3D Visual { get; }

        public ObservableCollection<BoneVM> Children { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public BoneVM(Bone model, BoneVM parent)
        {
            Model = model;
            Parent = parent;

            DisplaySettings.Get.PropertyChanged += OnDisplaySettingsPropertyChanged;

            Visual = new ModelVisual3D();
            coordinateSystemVisual = new CSysVisual3D();
            coordinateSystemVisual.Length = DisplaySettings.Get.CSysSize;

            Visual.Children.Add(coordinateSystemVisual);

            Children = new ObservableCollection<BoneVM>();
            Children.CollectionChanged += OnChildrenChanged;
            foreach (var item in model.Children)
            {
                Children.Add(new BoneVM(item, this));
            }
        }

        private void OnChildrenChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (BoneVM child in e.NewItems)
                {
                    Visual.Children.Add(child.Visual);
                    LinesVisual3D linkVisual = new LinesVisual3D();
                    linkVisual.Points = new Point3DCollection(new[] { new Point3D(0, 0, 0), child.Offset.ToPoint3D() });
                    childLinkVisualMap.Add(child, linkVisual);

                    Visual.Children.Add(linkVisual);
                    UpdateLinkVisual(child);

                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (BoneVM child in e.OldItems)
                {
                    Model.Children.Remove(child.Model);

                    Visual.Children.Remove(child.Visual);
                    Visual.Children.Remove(childLinkVisualMap[child]);
                }
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsEndSite)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
        }

        private void OnDisplaySettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateVisuals();
        }

        private void UpdateLinkVisual(BoneVM child)
        {
            var lineVisual = childLinkVisualMap[child];

            lineVisual.Points[1] = child.Offset.ToPoint3D();

            lineVisual.Thickness = LinkThickness;
            lineVisual.Color = LinkColor;
        }

        private void UpdateVisuals()
        {
            coordinateSystemVisual.Length = DisplaySettings.Get.CSysSize;
            foreach (var item in childLinkVisualMap)
            {
                UpdateLinkVisual(item.Key);
            }
        }

        public void Refresh()
        {
            Visual.Transform = new MatrixTransform3D(Model.LocalTransform);
            foreach (var item in Children)
            {
                item.Refresh();
            }
        }

        private void Collect(List<BoneVM> items)
        {
            items.Add(this);
            foreach (var item in Children)
            {
                item.Collect(items);
            }
        }

        public IEnumerator<BoneVM> GetEnumerator()
        {
            List<BoneVM> items = new List<BoneVM>();
            Collect(items);
            return EnumerableEx.Concat(items).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

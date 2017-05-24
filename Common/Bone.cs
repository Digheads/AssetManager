using System;
using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace AssetManager.Common
{
    public class Bone
    {
        private Quaternion jointRotation = Quaternion.Identity;
        private Vector3D offset = new Vector3D();

        private string name;

        public Matrix3D LocalTransform { get; private set; }

        public Quaternion JointRotation
        {
            get { return jointRotation; }
            set
            {
                jointRotation = value;
                UpdateLocalTransform();
            }
        }

        public Vector3D Offset
        {
            get { return offset; }
            set
            {
                offset = value;
                UpdateLocalTransform();
            }
        }

        public string Name
        {
            get
            {
                if (IsEndSite)
                    return "End Site";
                else
                    return name;
            }
            set { name = value; }
        }

        public bool IsEndSite { get { return Children.Count == 0; } }
        public Bone Parent { get; private set; }
        public List<Bone> Children { get; } = new List<Bone>();

        public Bone(Bone parent, string name = "bone", Vector3D offset = default(Vector3D))
        {
            Parent = parent;
            Name = name;
            Offset = offset;
        }

        private void UpdateLocalTransform()
        {
            var mat = Matrix3D.Identity;
            mat.Rotate(JointRotation);
            mat.Translate(Offset);

            LocalTransform = mat;
        }

        public Quaternion GetRootOrientation()
        {
            Quaternion quat = Quaternion.Identity;
            var current = this;
            do
            {
                quat *= current.JointRotation;
                current = current.Parent;
            } while (current != null);

            return quat;
        }

        public Matrix3D GetRootTransform()
        {
            Matrix3D transform = Matrix3D.Identity;
            var current = this;
            do
            {
                transform.Append(current.LocalTransform);
                current = current.Parent;
            } while (current != null);

            return transform;
        }

        public Bone Clone(Bone parentClone)
        {
            Bone clone = new Bone(parentClone, Name, Offset);
            clone.JointRotation = JointRotation;

            foreach (var child in Children)
                clone.Children.Add(child.Clone(this));

            return clone;
        }

        public void Traverse(Action<Bone, Quaternion> action, Quaternion worldRotation)
        {
            action(this, worldRotation);
            worldRotation *= jointRotation;

            foreach (var child in Children)
            {
                child.Traverse(action, worldRotation);
            }
        }

        public void Traverse(Action<Bone> action)
        {
            action(this);

            foreach (var child in Children)
            {
                child.Traverse(action);
            }
        }
    }
}

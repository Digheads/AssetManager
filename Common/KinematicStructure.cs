﻿using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace AssetManager.Common
{
    public class KinematicStructure
    {
        public Bone Root { get; }

        public KinematicStructure(Bone root = null)
        {
            if (root == null)
                Root = new Bone(parent: null) { Name = "Root" };
            else
                Root = root;
        }

        public void ApplyWorldRotations(Dictionary<Bone, Quaternion> jointRotations)
        {
            Root.Traverse((bone, worldRotation) =>
            {
                if (jointRotations.ContainsKey(bone))
                {
                    bone.JointRotation = worldRotation.Inverted() * jointRotations[bone];
                }
            }, Quaternion.Identity);
        }

        public void ApplyLocalRotation(Dictionary<Bone, Quaternion> jointRotations)
        {
            Root.Traverse((bone) =>
            {
                if (jointRotations.ContainsKey(bone))
                {
                    bone.JointRotation = jointRotations[bone];
                }
            });
        }

        public Dictionary<Bone, Quaternion> CollectLocalOrientations()
        {
            var result = new Dictionary<Bone, Quaternion>();
            Root.Traverse(bone => result.Add(bone, bone.JointRotation));
            return result;
        }
    }
}

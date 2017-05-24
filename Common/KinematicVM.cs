using System.Collections.Generic;

namespace AssetManager.Common
{
    public class KinematicVM
    {
        public Dictionary<Bone, BoneVM> BoneVMMap { get; } = new Dictionary<Bone, BoneVM>();

        public KinematicStructure Model { get; }

        public BoneVM[] Roots { get; } = new BoneVM[1];

        public BoneVM Root { get { return Roots[0]; } }

        public KinematicVM(KinematicStructure model)
        {
            Model = model;

            // create the bone ViewModel tree
            Roots[0] = new BoneVM(model.Root, null);

            foreach (BoneVM item in Root)
            {
                BoneVMMap.Add(item.Model, item);
            }
        }

        public void Refresh()
        {
            foreach (var item in Roots)
                item.Refresh();
        }
    }
}

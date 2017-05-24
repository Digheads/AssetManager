using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Media3D;

namespace AssetManager.Common
{
    public class MotionData
    {
        public double FPS { get; set; } = 120;

        public Dictionary<Bone, List<Quaternion>> Data { get; } = new Dictionary<Bone, List<Quaternion>>();

        public int FrameCount { get { return Data.First().Value.Count; } }
    }
}

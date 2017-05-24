using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace AssetManager.Common
{
    public class BVHMotionData
    {
        public double FrameTime { get; }
        public Dictionary<BVHNode, List<Quaternion>> Data { get; }

        public BVHMotionData(double frameTime, Dictionary<BVHNode, List<Quaternion>> motionData)
        {
            this.FrameTime = frameTime;
            this.Data = motionData;
        }
    }
}

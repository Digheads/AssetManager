using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Media.Media3D;

namespace AssetManager.Common
{
    public class BVHNode
    {
        public BVHNodeTypes Type { get; set; }

        public string Name { get; set; }

        public Vector3D Offset { get; set; }

        public BVHChannels[] Channels { get; set; }

        public List<BVHNode> Children { get; } = new List<BVHNode>();

        public static BVHNode ReadBVH(FileInfo file, out BVHMotionData motionData)
        {
            BVHNode root;
            BVHMotionData motion;
            using (var fr = file.OpenRead())
            {
                using (var reader = new StreamReader(fr))
                {
                    var line = reader.ReadLine().ToLower().Trim();
                    if (line != "hierarchy")
                        throw new FileFormatException("File has to start with HIERARCHY keyword");

                    root = ReadNode(reader, reader.ReadLine(), 0);
                    motion = ReadMotionData(reader, root);
                }
            }

            motionData = motion;
            return root;
        }

        private static BVHNode ReadNode(StreamReader reader, string idLine, int depth)
        {
            BVHNode node = new BVHNode();

            var line = idLine.ToLower().Trim();
            string[] tokens = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            string nodeType = tokens[0];
            string nodeName = tokens[1];

            BVHNodeTypes type;
            if (nodeType == "end" && nodeName == "site")
            {
                type = BVHNodeTypes.EndSite;
            }
            else
            {
                if (!Enum.TryParse(nodeType, true, out type))
                    throw new FileFormatException($"Invalid BVH Node Type: {nodeType}");
            }

            node.Type = type;
            node.Name = nodeName;

            reader.ReadLine();

            node.Offset = ReadOffset(reader);

            if (node.Type != BVHNodeTypes.EndSite)
            {
                node.Channels = ReadChannels(reader);
            }

            while (true)
            {
                line = reader.ReadLine().ToLower().Trim();

                if (line == "}")
                {
                    return node;
                }
                else
                {
                    node.Children.Add(ReadNode(reader, line, depth + 1));
                }
            }
        }

        public static BVHMotionData ReadMotionData(StreamReader reader, BVHNode root)
        {
            var line = reader.ReadLine().ToLower().Trim();
            if (line != "motion")
                throw new FileFormatException("Expected MOTION keyword");

            line = reader.ReadLine().ToLower().Trim();
            var tokens = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            if (!int.TryParse(tokens[1], out int numFrames))
            {
                throw new FileFormatException("Could not read number of frames");
            }

            line = reader.ReadLine().ToLower().Trim();
            tokens = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            if (!double.TryParse(tokens[2], NumberStyles.Number, CultureInfo.InvariantCulture, out double frameTime))
            {
                throw new FileFormatException("Could not read frame time");
            }

            Dictionary<BVHNode, List<Quaternion>> motionData = new Dictionary<BVHNode, List<Quaternion>>();
            while (!reader.EndOfStream)
            {
                line = reader.ReadLine().ToLower().Trim();
                if (String.IsNullOrWhiteSpace(line))
                    continue;

                double[] frameData = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries).Select(t => double.Parse(t, NumberStyles.Float, CultureInfo.InvariantCulture)).ToArray();

                int offset = 0;
                ReadFrameData(root, motionData, frameData, ref offset);
            }

            return new BVHMotionData(frameTime, motionData);
        }

        private static void ReadFrameData(BVHNode node, Dictionary<BVHNode, List<Quaternion>> motionData, double[] values, ref int offset)
        {
            if (node.Type == BVHNodeTypes.EndSite)
                return;

            double[] nodevalues = new double[node.Channels.Length];
            Array.Copy(values, offset, nodevalues, 0, node.Channels.Length);
            offset += node.Channels.Length;

            if (!motionData.ContainsKey(node))
            {
                motionData.Add(node, new List<Quaternion>());
            }

            int ignoredOffset = 0;
            if (node.Channels[0] == BVHChannels.Xposition)
                ignoredOffset += 3;

            var q1 = new Quaternion(GetAxisFromChannelType(node.Channels[ignoredOffset]), nodevalues[ignoredOffset]);
            var q2 = new Quaternion(GetAxisFromChannelType(node.Channels[ignoredOffset + 1]), nodevalues[ignoredOffset + 1]);
            var q3 = new Quaternion(GetAxisFromChannelType(node.Channels[ignoredOffset + 2]), nodevalues[ignoredOffset + 2]);

            Quaternion quat = q1 * q2 * q3;

            motionData[node].Add(quat);

            foreach (var item in node.Children)
            {
                ReadFrameData(item, motionData, values, ref offset);
            }
        }

        private static Vector3D GetAxisFromChannelType(BVHChannels channel)
        {
            switch (channel)
            {
                case BVHChannels.Xrotation:
                    return new Vector3D(1, 0, 0);
                case BVHChannels.Yrotation:
                    return new Vector3D(0, 1, 0);
                case BVHChannels.Zrotation:
                    return new Vector3D(0, 0, 1);
                default:
                    throw new InvalidOperationException($"Channel type {channel} not supported");
            }
        }

        private static BVHChannels[] ReadChannels(StreamReader reader)
        {
            string line = reader.ReadLine().ToLower().Trim();
            string[] tokens = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            if (tokens[0] != "channels")
                throw new FileFormatException("Expected CHANNELS keyword");

            int numChannels = Int32.Parse(tokens[1]);

            if (tokens.Length != numChannels + 2)
                throw new FileFormatException(
                    $"Invalid CHANNELs Definition: {numChannels} expected, but {tokens.Length - 2} found");

            BVHChannels[] channels = new BVHChannels[numChannels];
            for (int i = 0; i < numChannels; i++)
            {
                if (!Enum.TryParse<BVHChannels>(tokens[i + 2], true, out channels[i]))
                    throw new FileFormatException($"Invalid channel: {tokens[i + 2]}");
            }

            return channels;
        }

        private static Vector3D ReadOffset(StreamReader reader)
        {
            string line = reader.ReadLine().ToLower().Trim();
            string[] tokens = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            if (tokens[0] != "offset")
                throw new FileFormatException("Expected OFFSET keyword");
            if (tokens.Length != 4)
                throw new FileFormatException("OFFSET Definition: Invalid number of values");

            if (!Double.TryParse(tokens[1], NumberStyles.Number, CultureInfo.InvariantCulture, out double x))
                throw new FileFormatException("Could not parse OFFSET definition x-component");
            if (!Double.TryParse(tokens[2], NumberStyles.Number, CultureInfo.InvariantCulture, out double y))
                throw new FileFormatException("Could not parse OFFSET definition y-component");
            if (!Double.TryParse(tokens[3], NumberStyles.Number, CultureInfo.InvariantCulture, out double z))
                throw new FileFormatException("Could not parse OFFSET definition z-component");


            return new Vector3D(x, y, z);
        }

        public static Bone ToBones(BVHNode bvhNode, Bone parent, BVHMotionData bvhMotionData, MotionData resultMotionData)
        {
            Bone result = new Bone(parent, name: bvhNode.Name, offset: bvhNode.Offset);
            if (bvhNode.Type != BVHNodeTypes.EndSite)
                resultMotionData.Data.Add(result, bvhMotionData.Data[bvhNode].ToList());

            foreach (BVHNode item in bvhNode.Children)
            {
                result.Children.Add(ToBones(item, result, bvhMotionData, resultMotionData));
            }

            return result;
        }
    }
}

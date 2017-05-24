namespace AssetManager.Common
{
    public enum FileType
    {
        Model,
        Material,
        Texture
    }

    public enum SoundType
    {
        Loops,
        OneShots
    }

    public enum BVHChannels
    {
        Xposition = 0,
        Yposition = 1,
        Zposition = 2,

        Xrotation = 3,
        Yrotation = 4,
        Zrotation = 5,
    }

    public enum BVHNodeTypes
    {
        Root,
        Joint,
        EndSite,
    }

    public static class Global
    {
        public static Model Model { get; internal set; }
        public static Motion Motion { get; internal set; }
        public static Substance Substance { get; internal set; }
        public static Sound Loop { get; internal set; }
        public static Sound OneShot { get; internal set; }
        public static Script Script { get; internal set; }
        public static Package Package { get; internal set; }
        public static PSD PSD { get; internal set; }
        public static Font Font { get; internal set; }
    }
}

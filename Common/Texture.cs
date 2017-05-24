using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace AssetManager.Common
{
    public class Texture : Shared, IEquatable<Texture>
    {
        public enum TextureTypes { NotSet, Diffuse, Specular, Metallic, Bump, Normal, Height, Occlusion, Emission, Opacity };

        private double width;
        public double Width
        {
            get { return width; }
            set { width = value; }
        }

        private double height;
        public double Height
        {
            get { return height; }
            set { height = value; }
        }

        private string resolution;
        public string Resolution
        {
            get { return resolution; }
            set { resolution = value; }
        }

        private TextureTypes type;
        public TextureTypes Type
        {
            get { return type; }
            set { type = value; }
        }

        public Texture(FileInfo file) : base(file)
        {
            BitmapImage image = GetImage();
            this.Width = image.Width;
            this.Height = image.Height;
            this.Resolution = Width.ToString() + "x" + Height.ToString();
            this.Type = DetermineType();
        }

        private TextureTypes DetermineType()
        {
            if (Name[0] == 'd')
                return TextureTypes.Diffuse;
            else if (Name[0] == 's')
                return TextureTypes.Specular;
            else if (Name[0] == 'm')
                return TextureTypes.Metallic;
            else if (Name[0] == 'b')
                return TextureTypes.Bump;
            else if (Name[0] == 'n')
                return TextureTypes.Normal;
            else if (Name[0] == 'h')
                return TextureTypes.Height;
            else if (Name[0] == 'o')
                return TextureTypes.Occlusion;
            else if (Name[0] == 'e')
                return TextureTypes.Emission;
            else if (Name[0] == 'p')
                return TextureTypes.Occlusion;
            else
                return TextureTypes.NotSet;
        }

        public BitmapImage GetImage()
        {
            BitmapImage b = new BitmapImage();
            using (var stream = new FileStream(FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {               
                b.BeginInit();
                b.CacheOption = BitmapCacheOption.OnLoad;
                b.UriSource = new Uri(FullName);
                b.EndInit();
            }

            return b;
        }

        public bool Equals(Texture other)
        {
            if (Object.ReferenceEquals(other, null)) return false;
            if (Object.ReferenceEquals(this, other)) return true;

            return FullName.Equals(other.FullName);
        }

        public override int GetHashCode()
        {
            int hashProductCode = FullName.GetHashCode();
            return hashProductCode;
        }
    }
}

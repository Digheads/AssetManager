using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AssetManager.Common
{
    public class Material : Shared
    {
        private List<Texture> textures = new List<Texture>();
        public List<Texture> Textures
        {
            get { return textures; }
            set { textures = value; }
        }

        public Material(FileInfo file) : base(file)
        {
            List<string> lines = new List<string>();

            using (StreamReader stream = file.OpenText())
            {
                lines.AddRange(stream.ReadToEnd().Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
            }

            foreach (var item in lines.Where(x => x.Trim().StartsWith("map_") || x.Trim().StartsWith("bump")))
            {
                string str = RemoveLeading(item);
                for (int i = 0; i < item.Count(x => x == ' '); i++)
                {
                    FileInfo fi = new FileInfo(System.IO.Path.Combine(Path, str));
                    if (fi.Exists)
                    {
                        Textures.Add(new Texture(fi));
                        break;
                    }
                    else
                    {
                        str = RemoveLeading(str);
                    }
                }
            }

            Textures = Textures.Distinct().ToList();
        }

        private string RemoveLeading(string s)
        {
            return s.Remove(0, s.IndexOf(' ') + 1);
        }
    }
}

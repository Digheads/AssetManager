using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media.Media3D;

namespace AssetManager.Common
{
    public class Model : Shared
    {
        private string geometryVertices;
        public string GeomatryVertices
        {
            get { return geometryVertices; }
            set { geometryVertices = value; }
        }

        private string vertexNormals;
        public string VertexNormals
        {
            get { return vertexNormals; }
            set { vertexNormals = value; }
        }

        private string textureVertices;
        public string TextureVertices
        {
            get { return textureVertices; }
            set { textureVertices = value; }
        }

        private string parameterSpaceVertices;
        public string ParameterSpaceVertices
        {
            get { return parameterSpaceVertices; }
            set { parameterSpaceVertices = value; }
        }

        private string points;
        public string Points
        {
            get { return points; }
            set { points = value; }
        }

        private string lines;
        public string Lines
        {
            get { return lines; }
            set { lines = value; }
        }

        private string faces;
        public string Faces
        {
            get { return faces; }
            set { faces = value; }
        }

        private string curves;
        public string Curves
        {
            get { return curves; }
            set { curves = value; }
        }

        private string curves2D;
        public string Curves2D
        {
            get { return curves2D; }
            set { curves2D = value; }
        }

        private string surfaces;
        public string Surfaces
        {
            get { return surfaces; }
            set { surfaces = value; }
        }

        private List<Material> material = new List<Common.Material>();
        public List<Material> Material
        {
            get { return material; }
            set { material = value; }
        }

        public Model(FileInfo file) : base(file)
        {
            List<string> lines = new List<string>();

            using (StreamReader stream = file.OpenText())
            {
                lines.AddRange(stream.ReadToEnd().Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
            }

            GeomatryVertices = "Geometric vertices: " + lines.Count(x => x.StartsWith("v "));
            TextureVertices = "Texture vertices: " + lines.Count(x => x.StartsWith("vt "));
            VertexNormals = "Vertex normals: " + lines.Count(x => x.StartsWith("vn "));
            ParameterSpaceVertices = "Space vertices: " + lines.Count(x => x.StartsWith("vp "));
            Points = "Points: " + lines.Count(x => x.StartsWith("p "));
            Lines = "Lines: " + lines.Count(x => x.StartsWith("l "));
            Faces = "Faces: " + lines.Count(x => x.StartsWith("f "));
            Curves = "Curves: " + lines.Count(x => x.StartsWith("curv "));
            Curves2D = "2D curves: " + lines.Count(x => x.StartsWith("curv2 "));
            Surfaces = "Surfaces: " + lines.Count(x => x.StartsWith("surf "));

            foreach (var item in lines.Where(x => x.StartsWith("mtllib ")))
            {
                Material.Add(new Common.Material(new FileInfo(System.IO.Path.Combine(Path, item.Remove(0, 7)))));
            }
        }

        public Model3DGroup GetModel()
        {
            ModelImporter importer = new ModelImporter();
            return importer.Load(FullName);
        }
    }
}

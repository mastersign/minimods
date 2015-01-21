#region MiniMod
// <MiniMod>
//   <Name>Wavefront OBJ Reader</Name>
//   <Author>Tobias Kiertscher &lt;dev@mastersign.de&gt;</Author>
//   <LastChanged>2015-01-21</LastChanged>
//   <Version>1.1.0</Version>
//   <Url>https://gist.github.com/mastersign/c3d4d1a4bb552cdbfa53/raw/Mastersign.Minimods.ObjReader.cs</Url>
//   <Description>
//     Wavefront OBJ Reader parses an OBJ file with 3D geometry data
//     and provides a model tree with the geometry as triangles.
//   </Description>
// </MiniMod>
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Mastersign.Minimods.ObjReader
{
    /// <summary>
    /// This class represents a reader for Wavefront Object files.
    /// </summary>
    public class WavefrontObjReader : IVertexDictionary
    {
        private ModelRoot modelRoot;

        private int currentLineNumber = -1;
        private string currentLine = null;
        private int currentSmoothIndex = -1;

        public ModelRoot Parse(TextReader reader)
        {
            modelRoot = new ModelRoot();
            currentLineNumber = 0;
            currentLine = null;
            while ((currentLine = reader.ReadLine()) != null)
            {
                currentLineNumber++;
                ProcessLine(currentLine);
            }
            return modelRoot;
        }

        #region IVertexDictionary

        private readonly List<Position> positions = new List<Position>();
        private readonly List<Normal> normals = new List<Normal>();
        private readonly List<TextureCoords> textureCoords = new List<TextureCoords>();
        private readonly List<ParameterCoords> parameterCoords = new List<ParameterCoords>();

        public Position GetPosition(int index)
        {
            return positions[index - 1];
        }

        public Normal GetNormal(int index)
        {
            return normals[index - 1];
        }

        public TextureCoords GetTextureCoords(int index)
        {
            return textureCoords[index - 1];
        }

        public ParameterCoords GetParameterCoords(int index)
        {
            return parameterCoords[index - 1];
        }

        #endregion

        #region Helper

        private int NormalizeIndex(ICollection list, int index)
        {
            return index > 0 ? index : list.Count - index;
        }

        private double ParseFloatingpoint(string expr)
        {
            try
            {
                return double.Parse(expr, NumberStyles.Float, CultureInfo.InvariantCulture);
            }
            catch (FormatException fe)
            {
                throw new ObjParseException(currentLineNumber, currentLine, "Failed parsing floating point value.", fe);
            }
            catch (OverflowException oe)
            {
                throw new ObjParseException(currentLineNumber, currentLine, "Failed parsing floating point value.", oe);
            }
        }

        private int ParseInteger(string expr)
        {
            try
            {
                return int.Parse(expr, NumberStyles.Integer, CultureInfo.InvariantCulture);
            }
            catch (FormatException fe)
            {
                throw new ObjParseException(currentLineNumber, currentLine, "Failed parsing integer value.", fe);
            }
            catch (OverflowException oe)
            {
                throw new ObjParseException(currentLineNumber, currentLine, "Failed parsing integer value.", oe);
            }
        }

        #endregion

        #region Processing

        private void ProcessLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line)) return; // ignore empty lines
            if (line.StartsWith("#")) return; // ignore comments

            line = line.Trim(); // remove whitespace
            var cwPos = line.IndexOf(' '); // find first space
            var cw = cwPos > 0 ? line.Substring(0, cwPos) : line; // extract command word
            var expr = cwPos > 0 ? line.Substring(cwPos).TrimStart() : string.Empty; // extract command arguments

            var exprCmPos = expr.IndexOf('#'); // find comment at end of line
            if (exprCmPos >= 0) expr = expr.Substring(0, exprCmPos); // remove comment from end of line

            switch (cw) // process commands
            {
                case "v": ProcessPosition(expr); break;
                case "vt": ProcessTextureCoords(expr); break;
                case "vn": ProcessNormal(expr); break;
                case "vp": ProcessParameterCoords(expr); break;
                case "s": ProcessSmoothing(expr); break;
                case "f": ProcessFace(expr); break;
                case "o": ProcessObject(expr); break;
                case "g": ProcessGroup(expr); break;
                case "usemtl": ProcessUseMaterial(expr); break;
            }
        }

        private void ProcessPosition(string expr)
        {
            var parts = expr.Split(' ');
            if (parts.Length < 3)
            {
                throw new ObjParseException(currentLineNumber, currentLine,
                    "Invalid number of arguments.");
            }
            positions.Add(new Position(
                ParseFloatingpoint(parts[0]),
                ParseFloatingpoint(parts[1]),
                ParseFloatingpoint(parts[2]),
                parts.Length >= 4 ? ParseFloatingpoint(parts[3]) : 1.0));
        }

        private void ProcessNormal(string expr)
        {
            var parts = expr.Split(' ');
            if (parts.Length < 3)
            {
                throw new ObjParseException(currentLineNumber, currentLine,
                    "Invalid number of arguments.");
            }
            normals.Add(new Normal(
                ParseFloatingpoint(parts[0]),
                ParseFloatingpoint(parts[1]),
                ParseFloatingpoint(parts[2])));
        }

        private void ProcessTextureCoords(string expr)
        {
            var parts = expr.Split(' ');
            if (parts.Length < 2)
            {
                throw new ObjParseException(currentLineNumber, currentLine,
                    "Invalid number of arguments.");
            }
            textureCoords.Add(new TextureCoords(
                ParseFloatingpoint(parts[0]),
                ParseFloatingpoint(parts[1]),
                parts.Length >= 3 ? ParseFloatingpoint(parts[2]) : 0.0));
        }

        private void ProcessParameterCoords(string expr)
        {
            var parts = expr.Split(' ');
            if (parts.Length < 1)
            {
                throw new ObjParseException(currentLineNumber, currentLine,
                    "Invalid number of arguments.");
            }
            normals.Add(new Normal(
                ParseFloatingpoint(parts[0]),
                parts.Length >= 2 ? ParseFloatingpoint(parts[1]) : 0.0,
                parts.Length >= 3 ? ParseFloatingpoint(parts[2]) : 0.0));
        }

        private void ProcessSmoothing(string expr)
        {
            if (string.Equals("off", expr))
            {
                currentSmoothIndex = -1;
            }
            else
            {
                currentSmoothIndex = ParseInteger(expr);
            }
        }

        private void ProcessFace(string expr)
        {
            var indexSeparator = new[] { '/' };
            var parts = expr.Split(' ');
            if (parts.Length < 3)
            {
                throw new ObjParseException(currentLineNumber, currentLine,
                    "Failed to parse face description. Invalid number of arguments.");
            }
            var face = new Face();
            var viList = new VertexIndices[parts.Length];
            var hasTextureCoords = true;
            var hasNormals = true;
            for (var i = 0; i < parts.Length; i++)
            {
                var part = parts[i];
                var indices = part.Split(indexSeparator, StringSplitOptions.None);
                if (indices.Length < 1 || indices.Length > 3)
                {
                    throw new ObjParseException(currentLineNumber, currentLine,
                        "Failed to parse vertex indices. Invalid number of indices.");
                }
                if (hasTextureCoords && (indices.Length < 2 || string.IsNullOrWhiteSpace(indices[1])))
                {
                    hasTextureCoords = false;
                }
                if (hasNormals && (indices.Length < 3 || string.IsNullOrWhiteSpace(indices[2])))
                {
                    hasNormals = false;
                }
                viList[i] = new VertexIndices(
                    NormalizeIndex(positions, ParseInteger(indices[0])),
                    hasTextureCoords ? NormalizeIndex(textureCoords, ParseInteger(indices[1])) : 0,
                    hasNormals ? NormalizeIndex(normals, ParseInteger(indices[2])) : 0);
            }
            face.Indices = viList;
            face.HasTextureCoords = hasTextureCoords;
            face.HasNormals = hasNormals;
            face.SmoothingGroup = currentSmoothIndex;
            modelRoot.AddFace(face);
        }

        private void ProcessObject(string expr)
        {
            modelRoot.AssureGroup();
            modelRoot.CurrentGroup.StartObject(expr);
            currentSmoothIndex = -1;
        }

        private void ProcessGroup(string expr)
        {
            modelRoot.StartGroup(expr);
            currentSmoothIndex = -1;
        }

        private void ProcessUseMaterial(string expr)
        {
            if (string.IsNullOrWhiteSpace(expr))
            {
                throw new ObjParseException(currentLineNumber, currentLine, "Missing material name.");
            }
            modelRoot.AssureGroup();
            modelRoot.CurrentGroup.AssureObject();
            modelRoot.CurrentGroup.CurrentObject.Material = expr;
        }

        #endregion
    }

    public class ModelRoot : IEnumerable<ModelGroup>, IFaceContext
    {
        public List<ModelGroup> Groups { get; private set; }

        public ModelGroup CurrentGroup { get; set; }

        public ModelRoot()
        {
            Groups = new List<ModelGroup>();
        }

        public void StartGroup(string name = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                name = "default";
            }
            Groups.Add(CurrentGroup = new ModelGroup(name));
        }

        public void AssureGroup()
        {
            if (CurrentGroup == null) StartGroup();
        }

        public IEnumerable<Face> AllFaces { get { return Groups.SelectMany(g => g.AllFaces); } }

        public void AddFace(Face face)
        {
            AssureGroup();
            CurrentGroup.AddFace(face);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<ModelGroup> GetEnumerator()
        {
            return Groups.GetEnumerator();
        }
    }

    public class ModelGroup : IEnumerable<ModelObject>, IFaceContext
    {
        public string Name { get; set; }

        public string Material { get; set; }

        public List<ModelObject> Objects { get; private set; }

        public ModelObject CurrentObject { get; set; }

        public ModelGroup()
        {
            Objects = new List<ModelObject>();
        }

        public void StartObject(string name = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                name = "default";
            }
            Objects.Add(CurrentObject = new ModelObject(name));
        }

        public void AssureObject()
        {
            if (CurrentObject == null) StartObject();
        }

        public ModelGroup(string name)
            : this()
        {
            Name = name;
        }

        public IEnumerable<Face> AllFaces { get { return Objects.SelectMany(o => o.AllFaces); } }

        public void AddFace(Face face)
        {
            AssureObject();
            CurrentObject.AddFace(face);
        }

        public IEnumerator<ModelObject> GetEnumerator()
        {
            return Objects.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class ModelObject : IFaceContext
    {
        public string Name { get; set; }

        public string Material { get; set; }

        public List<Face> Faces { get; private set; }

        public ModelObject()
        {
            Faces = new List<Face>();
        }

        public ModelObject(string name)
            : this()
        {
            Name = name;
        }

        public IEnumerable<Face> AllFaces { get { return Faces; } }

        public IEnumerable<IGrouping<int, Face>> GetGroupedFaces()
        {
            return Faces.GroupBy(f => f.SmoothingGroup);
        }

        public void AddFace(Face face)
        {
            Faces.Add(face);
        }
    }

    public class Face
    {
        //public IVertexDictionary VertexDictionary { get; private set; }

        //public Face(IVertexDictionary vertexDictionary)
        //{
        //    VertexDictionary = vertexDictionary;
        //}

        public int SmoothingGroup { get; set; }

        public bool HasNormals { get; set; }

        public bool HasTextureCoords { get; set; }

        public VertexIndices[] Indices { get; set; }

        //public IEnumerable<Position> GetVertexPositions()
        //{
        //    return Indices.Select(vi => VertexDictionary.GetPosition(vi.PositionIndex));
        //}

        //public IEnumerable<Normal> GetVertexNormals()
        //{
        //    return Indices.Select(vi => VertexDictionary.GetNormal(vi.NormalIndex));
        //}

        //public IEnumerable<TextureCoords> GetVertexTextureCoords()
        //{
        //    return Indices.Select(vi => VertexDictionary.GetTextureCoords(vi.TextureCoordsIndex));
        //}

        public Triangle[] GetTriangles()
        {
            var list = new List<Triangle>();
            if (Indices.Length < 3)
            {
                throw new InvalidOperationException("The face has less than three vertices.");
            }

            var lastButTwo = Indices[0];
            var lastButOne = Indices[1];
            for (var i = 2; i < Indices.Length; i++)
            {
                list.Add(new Triangle(lastButTwo, lastButOne, Indices[i]));
                lastButTwo = Indices[i - 1];
                lastButOne = Indices[i];
            }
            return list.ToArray();
        }
    }

    public struct Triangle
    {
        private VertexIndices v1, v2, v3;

        public VertexIndices V1
        {
            get { return v1; }
            set { v1 = value; }
        }

        public VertexIndices V2
        {
            get { return v2; }
            set { v2 = value; }
        }

        public VertexIndices V3
        {
            get { return v3; }
            set { v3 = value; }
        }

        public Triangle(VertexIndices v1, VertexIndices v2, VertexIndices v3)
        {
            this.v1 = v1;
            this.v2 = v2;
            this.v3 = v3;
        }
    }

    public struct VertexIndices
    {
        private int positionIndex;
        private int textureCoordsIndex;
        private int normalIndex;

        public int PositionIndex
        {
            get { return positionIndex; }
            set { positionIndex = value; }
        }

        public int TextureCoordsIndex
        {
            get { return textureCoordsIndex; }
            set { textureCoordsIndex = value; }
        }

        public int NormalIndex
        {
            get { return normalIndex; }
            set { normalIndex = value; }
        }

        public VertexIndices(int posIndex, int textCoordsIndex, int normIndex)
        {
            positionIndex = posIndex;
            textureCoordsIndex = textCoordsIndex;
            normalIndex = normIndex;
        }
    }

    public interface IVertexDictionary
    {
        Position GetPosition(int index);
        Normal GetNormal(int index);
        TextureCoords GetTextureCoords(int index);
        ParameterCoords GetParameterCoords(int index);
    }

    public interface IFaceContext
    {
        void AddFace(Face face);

        IEnumerable<Face> AllFaces { get; }
    }

    public struct Position
    {
        public static readonly Position Default = new Position(0, 0, 0, 1);

        public double X, Y, Z, W;

        public Position(double x, double y, double z, double w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }
    }

    public struct TextureCoords
    {
        public static readonly TextureCoords Default = new TextureCoords(0, 0, 0);

        public double U, V, W;

        public TextureCoords(double u, double v, double w)
        {
            U = u;
            V = v;
            W = w;
        }
    }

    public struct Normal
    {
        public static readonly Normal Default = new Normal(1, 0, 0);

        public double X, Y, Z;

        public Normal(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }

    public struct ParameterCoords
    {
        public static readonly ParameterCoords Default = new ParameterCoords(0, 0, 0);

        public double U, V, W;

        public ParameterCoords(double u, double v, double w)
        {
            U = u;
            V = v;
            W = w;
        }
    }

    public struct FullVertex
    {
        public Position Position;

        public TextureCoords TextureCoords;

        public Normal Normal;

        public ParameterCoords ParameterCoords;

        public FullVertex(Position position, TextureCoords textureCoords, Normal normal, ParameterCoords parameterCoords)
        {
            Position = position;
            TextureCoords = textureCoords;
            Normal = normal;
            ParameterCoords = parameterCoords;
        }
    }

    public class ObjParseException : Exception
    {
        public int LineNumber { get; private set; }
        public string Line { get; private set; }

        public ObjParseException(int lineNumber, string line, string message, Exception innerException)
            : base(message + Environment.NewLine + string.Format("[{0}] {1}", lineNumber, line), innerException)
        {
            LineNumber = lineNumber;
            Line = line;
        }

        public ObjParseException(int lineNumber, string line, string message)
            : base(message + Environment.NewLine + string.Format("[{0}] {1}", lineNumber, line))
        {
            LineNumber = lineNumber;
            Line = line;
        }
    }
}
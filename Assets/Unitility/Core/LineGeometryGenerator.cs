using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Unitility.Core
{
    public struct LineData
    {
        public Vector3[] Points;
        public float Alpha;
        public float Width;
        public float CapLength;
        public Color Color;
        public LineCapType Cap;
    }

    public enum LineCapType
    {
        None,
        Sharp,
        Arrow,
        Round
    }

    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class LineGeometryGenerator : MonoBehaviour
    {
        private List<LineData> _lines;
        private MeshFilter _filter;
        private MeshRenderer _renderer;
 
        public LineCapType LineCap;
        public float CapLength;
        public string SortingLayer;

        public float LineWidth;

        // Use this for initialization
        void Start ()
        {
            _filter = GetComponent<MeshFilter>();
            _filter.mesh = new Mesh();
            _renderer = GetComponent<MeshRenderer>();
        }

        public void SetLines(IEnumerable<LineData> lines)
        {
            _lines = lines.ToList();
            RecalculateGeometry();
        }

        private void RecalculateGeometry()
        {
            _renderer.sortingLayerName = SortingLayer;

            var vertices = new List<Vector3>();
            var normals = new List<Vector3>();
            var uvs = new List<Vector2>();
            var triangles = new List<int>();

            foreach (var line in _lines)
            {
                var edges = new Vector3[line.Points.Length-1];
                for (int i = 0; i < line.Points.Length - 1; i++)
                {
                    edges[i] = line.Points[i + 1] - line.Points[i];
                }
                Vector3 previousLocalRight;
                for (int i = 0; i < line.Points.Length; i++)
                {
                    if (i == 0)
                    {
                        previousLocalRight = Vector3.Cross(edges[0], Vector3.up);
                        continue;
                    }
                    var localRight = i < line.Points.Length - 1 
                        ? Vector3.Cross(edges[i-1]+edges[i], Vector3.up)
                        : Vector3.Cross(edges[i], Vector3.up);

                    vertices.Add(line.Points[i-1]);

                    previousLocalRight = localRight;
                }
            }
        }

        // Update is called once per frame
        void Update () {
    
        }
    }
}

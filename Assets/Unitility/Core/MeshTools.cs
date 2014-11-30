using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Unitility.Core
{
    public class MeshProxy
    {
        private readonly Mesh _mesh;

        public MeshProxy(Mesh mesh)
        {
            _mesh = mesh;
            vertices = new List<Vector3>();
            uv = new List<Vector2>();
            triangles = new List<int>();
        }

        public List<Vector3> vertices { get; set; }
        public List<Vector2> uv { get; set; }
        public List<int> triangles { get; set; }

        //public void AddTri(Vector3 bottomLeft, Vector3 bottomRight, Vector3 top, Vector2 uv1, Vector2 uv2, Vector2 uv3)
        //{
        //    var offset = vertices.Count;
        //    vertices.Add(bottomLeft);
        //    vertices.Add(bottomRight);
        //    vertices.Add(top);
        //    uv.Add(uv1);
        //    uv.Add(uv2);
        //    uv.Add(uv3);
        //    triangles.AddRange(new[] { offset, offset + 2, offset + 1 });
        //}

        public void AddQuad(Vector3 bottomLeft, Vector3 bottomRight, Vector3 topLeft, Vector3 topRight, Vector2 uv1, Vector2 uv2, Vector2 uv3,
            Vector2 uv4)
        {
            var offset = vertices.Count;
            vertices.Add(bottomLeft);
            vertices.Add(bottomRight);
            vertices.Add(topLeft);
            vertices.Add(topRight);
            uv.Add(uv1);
            uv.Add(uv2);
            uv.Add(uv3);
            uv.Add(uv4);
            triangles.AddRange(new[]{offset, offset+2, offset+3, offset, offset+3, offset+1});
        }

        public void Commit(MeshFilter filter)
        {
            _mesh.vertices = vertices.ToArray();
            _mesh.uv = uv.ToArray();
            //_mesh.normals = vertices.Select(v => Vector3.up).ToArray();
            _mesh.triangles = triangles.ToArray();
            _mesh.RecalculateBounds();
            _mesh.RecalculateNormals();
            filter.mesh = _mesh;
            //_mesh.UploadMeshData(false);
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Constraints;
using UnityEngine;

namespace Assets.Unitility.Core
{
    public struct LineData
    {
        public float Alpha;
        public CornerStyle Corners;
        public float CornerRadius;
        public LineCapType StartCap;
        public LineCapType EndCap;
        public float StartCapSize;
        public float EndCapSize;
        public Color Color;
        public Vector3[] Points;
        public float Width;
    }

    public enum CornerStyle
    {
        Sharp,
        Rounded
    }

    public enum LineCapType
    {
        None,
        Sharp,
        Arrow,
        Round
    }

    [RequireComponent(typeof (MeshFilter))]
    [RequireComponent(typeof (MeshRenderer))]
    public class LineGeometryGenerator : MonoBehaviour
    {
        private MeshFilter _filter;
        private List<LineData> _lines;
        private MeshRenderer _renderer;
        public string SortingLayer;
        public float LineWidth;
        // Use this for initialization
        private void Start()
        {
            _filter = GetComponent<MeshFilter>();
            _filter.mesh = new Mesh();
            _renderer = GetComponent<MeshRenderer>();
            _renderer.material.mainTexture.wrapMode = TextureWrapMode.Repeat;

            StartCoroutine(MeshUpdate());
        }

        private IEnumerator MeshUpdate()
        {
            while (true)
            {
                SetLinesFromChildren();
                for (int i = 0; i < 1; i++)
                {
                    yield return null;
                }
            }
        }

        private void SetLinesFromChildren()
        {
            var test = new LineData
            {
                Points = Enumerable.Range(0, transform.childCount).Select(i => transform.GetChild(i).position).ToArray(),
                Width = LineWidth,
                CornerRadius = 0.6f * LineWidth,
                Corners = CornerStyle.Rounded,
                StartCap = LineCapType.Arrow,
                EndCap = LineCapType.Arrow,
                EndCapSize = 1.5f * LineWidth,
                StartCapSize = LineWidth
            };
            SetLines(new[] { test });
        }

        public void SetLines(IEnumerable<LineData> lines)
        {
            _lines = lines.ToList();
            RecalculateGeometry();
        }

        private void AddSegment(MeshProxy proxy, Vector3 p1, Vector3 p2, Vector3 t1, 
                                Vector3? t2n = null)
        {
            var t2 = t2n ?? t1;

            //Debug.DrawLine(p1 - t1, p1 - t1 + 0.1f * Vector3.up, Color.white, 12);
            //yield return new WaitForSeconds(0.25f);
            //Debug.DrawLine(p1 + t1, p1 + t1 + 0.1f * Vector3.up, Color.cyan, 11.75f);
            //yield return new WaitForSeconds(0.25f);
            //Debug.DrawLine(p2 - t2, p2 - t2 + 0.1f * Vector3.up, Color.black, 11.5f);
            //yield return new WaitForSeconds(0.25f);
            //Debug.DrawLine(p2 + t2, p2 + t2 + 0.1f * Vector3.up, Color.red, 11.25f);
            //yield return new WaitForSeconds(0.25f);
            proxy.AddQuad(
                p1 - t1,
                p1 + t1,
                p2 - t2,
                p2 + t2,
                Vector2.zero,
                Vector2.right,
                Vector2.up,
                Vector2.one);
        }

        private void AddCircleSegment(MeshProxy proxy, Vector3 center, Vector3 startPointer, float angle, int subdivisions, bool ccw = false)
        {
            var angleStep = angle / subdivisions;
            var axis = ccw ? Vector3.down : Vector3.up;
            for (int i = 0; i < subdivisions; i++)
            {
                proxy.AddQuad(center, center + Quaternion.AngleAxis(i*angleStep, axis)*startPointer,
                    center,
                    center + Quaternion.AngleAxis((i + 1)*angleStep, axis)*startPointer, 0.5f*Vector2.right,
                    Vector2.right, new Vector2(0.5f, 1), Vector2.one);
            }
        }

        private void AddRingSegment(MeshProxy proxy, Vector3 center, Vector3 normal, Vector3 startPointer,
            float innerRadius, float outerRadius, float angle)
        {
            var subdivisions = (int)(angle/8);
            var angleStep = angle/subdivisions;

            if (normal.y < 0)
            {
                center += (innerRadius+outerRadius)* startPointer;
                startPointer *= -1;
            }

            for (int i = 0; i < subdivisions; i++)
            {
                if (normal.y > 0)
                {
                    var firstPointer = Quaternion.AngleAxis(i * angleStep, normal) * startPointer;
                    var secondPointer = Quaternion.AngleAxis((i + 1) * angleStep, normal) * startPointer;
                    proxy.AddQuad(center + outerRadius*firstPointer, center + innerRadius*firstPointer,
                        center + outerRadius*secondPointer, center + innerRadius*secondPointer, Vector2.zero,
                        Vector2.right, Vector2.up, Vector2.one);
                }
                else
                {
                    var firstPointer = Quaternion.AngleAxis(i * angleStep, normal) * startPointer;
                    var secondPointer = Quaternion.AngleAxis((i + 1) * angleStep, normal) * startPointer;
                    proxy.AddQuad(center + innerRadius * firstPointer, center + outerRadius * firstPointer,
                        center + innerRadius * secondPointer, center + outerRadius * secondPointer, Vector2.zero,
                        Vector2.right, Vector2.up, Vector2.one);
                }
            }
        }

        private void AddCap(MeshProxy proxy, LineData line, Vector3 point, Vector3 direction, Vector3 t, bool endCap)
        {
            Vector3 end;
            var capType = endCap ? line.EndCap : line.StartCap;
            var capSize = endCap ? line.EndCapSize : line.StartCapSize;
            switch (capType)
            {
                case LineCapType.None:
                    break;
                case LineCapType.Sharp:
                    end = point + capSize * direction;
                    proxy.AddQuad(point - t, point, end, point,
                        Vector2.zero, 0.5f * Vector2.right, Vector2.up, new Vector2(0.5f, 1));
                    proxy.AddQuad(point, point + t, point, end, 
                        0.5f * Vector2.right, Vector2.right, new Vector2(0.5f, 1), Vector2.one);
                    break;
                case LineCapType.Arrow:
                    end = point + capSize * direction;
                    var center = point + 0.5f*capSize*direction;
                    var tNormalized = t.normalized;
                    proxy.AddQuad(point - t, point, point - capSize * tNormalized, center,
                        Vector2.zero, 0.5f * Vector2.right, Vector2.up, new Vector2(0.5f, 1));
                    proxy.AddQuad(point - capSize * tNormalized, center, end, center,
                        Vector2.zero, 0.5f*Vector2.right, Vector2.up, new Vector2(0.5f, 1));
                    proxy.AddQuad(center, end, center, point+capSize*tNormalized,
                        0.5f * Vector2.right, Vector2.right, new Vector2(0.5f, 1), Vector2.one);
                    proxy.AddQuad(point + t, point + capSize * tNormalized, point, center,
                       Vector2.right, Vector2.one, 0.5f * Vector2.right, new Vector2(0.5f, 1));
                    break;
                case LineCapType.Round:
                    AddCircleSegment(proxy, point, t, 180, 10, true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
        }

        private void ExpandCorner(MeshProxy proxy, LineData line, Vector3 point, Vector3 previousEdge, Vector3 nextEdge, Vector3 segmentStart, bool firstSegment)
        {
            var w = line.Width / 2;
            var r = Mathf.Max(w, line.CornerRadius);
            var angle = Vector3.Angle(previousEdge, nextEdge);
            var sin = Mathf.Sin(Mathf.Deg2Rad*angle/2);
            var cos = Mathf.Cos(Mathf.Deg2Rad*angle/2);
            var hypothenuse = r / cos;
            var slide = hypothenuse * sin;

            //Debug.DrawLine(point, point + 0.25f*Vector3.up, Color.red, 2);
            //yield return new WaitForSeconds(.05f);
            //Debug.DrawLine(point, point - previousEdge, Color.blue, 2);
            //Debug.DrawLine(point, point + nextEdge, Color.cyan, 2);
            //yield return new WaitForSeconds(.1f);

            var normalizedPrevious = previousEdge.normalized;

            var normalizedNext = nextEdge.normalized;
            var initial = point - slide * normalizedPrevious;
            var initialRight = Vector3.Cross(Vector3.up, normalizedPrevious);
            //Debug.DrawLine(initial, initial + 0.25f * Vector3.up, Color.yellow, 2);
            //Debug.DrawLine(initial, initial + w * initialRight, Color.green, 2);
            //yield return new WaitForSeconds(.05f);

            // add connection between previous and current corner
            
            AddSegment(proxy, segmentStart, initial, w * initialRight);
            if (firstSegment)
            {
                AddCap(proxy, line, segmentStart, -normalizedPrevious, -w * initialRight, false);
            }

            switch (line.Corners)
            {
                case CornerStyle.Sharp:
                    var medianLength = hypothenuse*w/r;
                    var final = point + slide*normalizedNext;
                    var finalRight = Vector3.Cross(Vector3.up, normalizedNext);

                    var medianRight = (initialRight + finalRight).normalized;
                    //Debug.DrawLine(final, final + 0.25f * Vector3.up, Color.green, 2);
                    //Debug.DrawLine(final, final + w * finalRight, Color.yellow, 2);
                    //yield return new WaitForSeconds(.05f);
                    //Debug.DrawLine(point, point + 0.25f * Vector3.up, Color.black, 2);
                    //Debug.DrawLine(point, point + medianLength * medianRight, Color.blue, 2);
                    //yield return new WaitForSeconds(.1f);
                    AddSegment(proxy, initial, point, w * initialRight, medianLength * medianRight);
                    AddSegment(proxy, point, final, medianLength * medianRight, w * finalRight);
                    break;
                case CornerStyle.Rounded:
                    var cornerAngle = angle;
                    var center = initial + r*initialRight;
                    var delta = r - w;
                    AddRingSegment(proxy, center, Vector3.Cross(normalizedPrevious, normalizedNext).normalized,
                        -initialRight, delta, delta + line.Width, cornerAngle);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void RecalculateGeometry()
        {
            _renderer.sortingLayerName = SortingLayer;

            var proxy = new MeshProxy(new Mesh());

            foreach (var line in _lines)
            {
                var edges = new Vector3[line.Points.Length - 1];
                for (var i = 0; i < line.Points.Length - 1; i++)
                {
                    edges[i] = line.Points[i + 1] - line.Points[i];
                }
                for (var i = 1; i < line.Points.Length; i++)
                {
                    if (i == line.Points.Length-1)
                    {
                        var start = 0.5f * (proxy.vertices[proxy.vertices.Count - 1] + proxy.vertices[proxy.vertices.Count - 2]);
                        var t = Vector3.Cross(Vector3.up, edges[i - 1]).normalized*line.Width/2;
                        AddSegment(proxy, start, line.Points[i], t);
                        AddCap(proxy, line, line.Points[i], edges[i - 1].normalized,
                           t , true);
                    }
                    else
                    {
                        ExpandCorner(proxy, line, line.Points[i], edges[i - 1], edges[i],
                            i == 1
                                ? line.Points[0]
                                : 0.5f*
                                  (proxy.vertices[proxy.vertices.Count - 2] +
                                   proxy.vertices[proxy.vertices.Count - 1]), i == 1);
                    }
                }
            }

            proxy.Commit(_filter);
        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SetLinesFromChildren();
            }
        }
    }
}
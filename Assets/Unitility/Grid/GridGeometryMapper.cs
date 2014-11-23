using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Unitility.Core;
using UnityEngine;

namespace Assets.Unitility.Grid
{
    public abstract class GridGeometryMapper
    {
        protected readonly Transform Reference;
        protected abstract IntVector3[] OrthogonalsFor(IntVector3 index);
        protected abstract IntVector3[] DiagonalsFor(IntVector3 index);
        public abstract Vector3 Scale { get; set; }
        public abstract Vector3 Spacing { get; set; }

        protected GridGeometryMapper(Transform reference)
        {
            Reference = reference;
        }

        public abstract Vector3 ToWorld(IntVector3 v);
        public abstract IntVector3 ToGrid(Vector3 v);
        public abstract IEnumerable<Vector3> Corners(IntVector3 v, bool includeSpacing = true);
        public IEnumerable<IntVector3> Neighbors(IntVector3 index, bool includeDiagonals = false)
        {
            var result = new List<IntVector3>();
            result.AddRange(OrthogonalsFor(index).Select(v => v + index));
            if (includeDiagonals)
            {
                result.AddRange(DiagonalsFor(index).Select(v => v + index));
            }
            return result;
        }
    }

    public class PlanarRectangularMapper : GridGeometryMapper
    {
        public override Vector3 Spacing { get; set; }
        public override Vector3 Scale { get; set; }

        public static readonly IntVector3[] DiagonalArray =
        {
            new IntVector2(1, 1),
            new IntVector2(-1, 1),
            new IntVector2(1, -1),
            new IntVector2(-1, -1)
        };

        public static readonly IntVector3[] OrthogonalArray =
        {
            IntVector2.up, IntVector2.right, IntVector2.down, IntVector2.left
        };

        protected override IntVector3[] DiagonalsFor(IntVector3 index)
        {
           return DiagonalArray;
        }

        protected override IntVector3[] OrthogonalsFor(IntVector3 index)
        {
            return OrthogonalArray;
        }

        public override Vector3 ToWorld(IntVector3 v)
        {
            return Reference.TransformPoint(v.x  * (1 + Spacing.x)  * Scale.x, v.y * (1 + Spacing.y)  * Scale.y, 0);
        }

        public override IntVector3 ToGrid(Vector3 v)
        {
            v = Reference.InverseTransformPoint(v);
            return new IntVector3(Mathf.RoundToInt(v.x / ((1 + Spacing.x) * Scale.x)), Mathf.RoundToInt(v.y / ((1 + Spacing.y) * Scale.y)), 0);
        }

        public override IEnumerable<Vector3> Corners(IntVector3 v, bool includeSpacing = true)
        {
            var center = ToWorld(v);
            var directions = new[]
            {
                0.5f*new Vector3(Scale.x, Scale.y),
                0.5f*new Vector3(Scale.x, -Scale.y),
                0.5f*new Vector3(-Scale.x, -Scale.y),
                0.5f*new Vector3(-Scale.x, Scale.y)
            };
            return directions.Select(d => center + Reference.TransformDirection(includeSpacing ? new Vector3((1 + Spacing.x) * d.x, (1 + Spacing.y) * d.y ) : d));
        }

        public PlanarRectangularMapper(Transform reference) : base(reference)
        {
        }
    }

    public class PlanarSquareMapper : GridGeometryMapper
    {
        protected override IntVector3[] DiagonalsFor(IntVector3 index)
        {
            return PlanarRectangularMapper.DiagonalArray;
        }

        protected override IntVector3[] OrthogonalsFor(IntVector3 index)
        {
            return PlanarRectangularMapper.OrthogonalArray;
        }

        public override Vector3 Spacing { get; set; }
        public override Vector3 Scale { get; set; }

        public override Vector3 ToWorld(IntVector3 v)
        {
            return Reference.TransformPoint(v.x * (1 + Spacing.x) * Scale.x, v.y * (1 + Spacing.x) * Scale.x, 0);
        }

        public override IntVector3 ToGrid(Vector3 v)
        {
            v = Reference.InverseTransformPoint(v);
            return new IntVector3(Mathf.RoundToInt(v.x / ((1 + Spacing.x) * Scale.x)), Mathf.RoundToInt(v.y / ((1 + Spacing.x) * Scale.x)), 0);
        }

        public override IEnumerable<Vector3> Corners(IntVector3 v, bool includeSpacing = true)
        {
            var center = ToWorld(v);
            var directions = new[]
            {
                0.5f*Scale.x*new Vector3(1,1),
                0.5f*Scale.x*new Vector3(1, -1),
                0.5f*Scale.x*new Vector3(-1,-1),
                0.5f*Scale.x*new Vector3(-1, 1)
            };
            return
                directions.Select(
                    d =>
                        center +
                        Reference.TransformDirection(includeSpacing
                            ? (1 + Spacing.x) * d
                            : d));
        }

        public PlanarSquareMapper(Transform reference) : base(reference)
        {
        }
    }

    public class CubicMapper : GridGeometryMapper
    {
        public CubicMapper(Transform reference) : base(reference)
        {
        }

        public static readonly IntVector3[] OrthogonalArray = 
        {
            IntVector3.right,
            IntVector3.left,
            IntVector3.up,
            IntVector3.down,
            IntVector3.forward,
            IntVector3.back
        };

        protected override IntVector3[] OrthogonalsFor(IntVector3 index)
        {
            return OrthogonalArray;
        }

        public static readonly IntVector3[] DiagonalArray = 
        {
            new IntVector3(1, 1, 1),
            new IntVector3(-1, 1, 1),
            new IntVector3(1, -1, 1),
            new IntVector3(1, 1, -1),
            new IntVector3(-1, -1, 1),
            new IntVector3(1, -1, -1),
            new IntVector3(-1, 1, -1),
            new IntVector3(-1, -1, -1),
            new IntVector3(0, 1, 1),
            new IntVector3(0, -1, 1),
            new IntVector3(0, 1, -1),
            new IntVector3(0, -1, -1),
            new IntVector3(1, 0, 1),
            new IntVector3(-1, 0, 1),
            new IntVector3(1, 0, -1),
            new IntVector3(-1, 0, -1),
            new IntVector3(1, 1, 0),
            new IntVector3(-1, 1, 0),
            new IntVector3(1, -1, 0),
            new IntVector3(-1, -1, 0)
        };

        protected override IntVector3[] DiagonalsFor(IntVector3 index)
        {
            return DiagonalArray;
        }

        public override Vector3 Spacing { get; set; }
        public override Vector3 Scale { get; set; }

        public override Vector3 ToWorld(IntVector3 v)
        {
            return Reference.TransformPoint(v.x*(1 + Spacing.x)*Scale.x, v.y*(1 + Spacing.x)*Scale.x,
                v.z*(1 + Spacing.x)*Scale.x);
        }

        public override IntVector3 ToGrid(Vector3 v)
        {
            v = Reference.InverseTransformPoint(v);
            return new IntVector3(Mathf.RoundToInt(v.x/((1 + Spacing.x)*Scale.x)),
                Mathf.RoundToInt(v.y/((1 + Spacing.x)*Scale.x)), Mathf.RoundToInt(v.z/((1 + Spacing.x)*Scale.x)));
        }

        public override IEnumerable<Vector3> Corners(IntVector3 v, bool includeSpacing = true)
        {
            var center = ToWorld(v);
            var directions = new[]
            {
                0.5f*Scale.x*new Vector3(1,1,1),
                0.5f*Scale.x*new Vector3(1, -1, 1),
                0.5f*Scale.x*new Vector3(-1,-1, 1),
                0.5f*Scale.x*new Vector3(-1, 1, 1),
                0.5f*Scale.x*new Vector3(1,1,-1),
                0.5f*Scale.x*new Vector3(1, -1, -1),
                0.5f*Scale.x*new Vector3(-1,-1, -1),
                0.5f*Scale.x*new Vector3(-1, 1, -1)
            };
            return directions.Select(d => center + Reference.TransformDirection(includeSpacing ? (1+Spacing.x)*d : d));
        }
    }

    public class PlanarTriangleMapper : GridGeometryMapper
    {
        public static readonly IntVector3[] DiagonalArray =
        {
            new IntVector3(-1,1,0),
            new IntVector3(-1,1,1), 
            new IntVector3(0,1,0),
            new IntVector3(1,0,0),
            new IntVector3(1,-1,1),
            new IntVector3(1,-1, 0),
            new IntVector3(0,-1, 0),
            new IntVector3(-1,-1,1),
            new IntVector3(-1,0,0),
        };

        public static readonly IntVector3[] OrthogonalArray =
        {
            new IntVector3(0,0,1), new IntVector3(-1, 0, 1), new IntVector3(0, -1, 1)   
        };

        protected override IntVector3[] DiagonalsFor(IntVector3 index)
        {
            return index.z == 0 ? DiagonalArray : DiagonalArray.Select(v => -v).ToArray();
        }

        protected override IntVector3[] OrthogonalsFor(IntVector3 index)
        {
            return index.z == 0 ? OrthogonalArray : OrthogonalArray.Select(v => -v).ToArray();
        }

        private Vector3 _scale;

        public override Vector3 Scale
        {
            get { return _scale; }
            set
            {
                _dirty = true;
                _scale = value;
            }
        }

        private Vector3 _spacing;

        public override Vector3 Spacing
        {
            get { return _spacing; }
            set
            {
                _dirty = true;
                _spacing = value;
            }
        }

        private bool _dirty = true;

        private static readonly float Height = Mathf.Sqrt(3) / 2;
        private float _a;
        private float A
        {
            get
            {
                if (_dirty)
                {
                    RecalculateBaseMetrics();
                }
                return _a;
            }
        }
        private Vector3 _x;
        private Vector3 X
        {
            get
            {
                if (_dirty)
                {
                    RecalculateBaseMetrics();
                }
                return _x;
            }
        }

        private Vector3 _y;
        private Vector3 Y
        {
            get
            {
                if (_dirty)
                {
                    RecalculateBaseMetrics();
                }
                return _y;
            }
        }

        private Vector3 _z;

        private Vector3 Z
        {
            get
            {
                if (_dirty)
                {
                    RecalculateBaseMetrics();
                }
                return _z;
            }
        }

        public PlanarTriangleMapper(Transform reference) : base(reference)
        {
        }

        public override Vector3 ToWorld(IntVector3 v)
        {
            var offsetPos = v.x * X + v.y * Y + (v.z % 2) * Z;
            //-- Debug.DrawLine(_reference.TransformPoint(offsetPos), _reference.TransformPoint(offsetPos)-_reference.forward, Color.yellow, 2);
            return Reference.TransformPoint(offsetPos);
        }

        public override IntVector3 ToGrid(Vector3 worldPos)
        {
            var centerOffset = (1f / 3) * (X + Y);
            //-- Debug.DrawLine(worldPos, worldPos - _reference.forward, Color.cyan, 1);
            var localPos = Reference.InverseTransformPoint(worldPos) + centerOffset;
            //-- Debug.DrawLine(worldPos, _reference.TransformPoint(offsetPos), Color.blue, 1);
            var normalizedY = localPos.y / (Height * A);
            var normalizedX = localPos.x / A - 0.5f * normalizedY;
            //-- var gridVector = new Vector3(normalizedX, normalizedY, 0);
            //-- Debug.DrawLine(_reference.TransformPoint(offsetPos), _reference.TransformPoint(gridVector), Color.red, 1);
            var u = Mathf.FloorToInt(normalizedX);
            var v = Mathf.FloorToInt(normalizedY);
            //-- var flooredGridVector = new Vector3(u, v, 0);
            //-- Debug.DrawLine(_reference.TransformPoint(gridVector), _reference.TransformPoint(flooredGridVector), Color.green, 1);
            var fracX = normalizedX - u;
            var fracY = normalizedY - v;
            //-- Debug.Log(new IntVector3(u, v, (fracX + fracY) >= 0.5f ? 1 : 0));
            //-- Debug.Log((fracX + fracY));
            return new IntVector3(u, v, (fracX + fracY) >= 1 ? 1 : 0);
        }

        private void RecalculateBaseMetrics()
        {
            _a = (1 + _spacing.x)*_scale.x;
            _x = _a * Vector3.right;
            _y = _a * new Vector3(0.5f, Height);
            _z = _a * new Vector3(0.5f, Height / 3);
            _dirty = false;
        }

        public override IEnumerable<Vector3> Corners(IntVector3 v, bool includeSpacing = true)
        {
            var center = ToWorld(v);
            var a = includeSpacing ? A : Scale.x;
            var h = a * Height;
            var directions = new[]
                    {
                        new Vector3(0, h*2/3),
                        new Vector3(0.5f*a, -h/3),
                        new Vector3(-0.5f*a, -h/3),
                    };
            return directions.Select(d => center + Reference.TransformDirection(v.z == 0 ? d : -d));
        }
    }

    public class PlanarHexMapper : GridGeometryMapper
    {
        public enum Orientation
        {
            FlatTop,
            PointyTop
        }

        public Orientation Mode { get; set; }
        private Vector3 _scale;

        public override Vector3 Scale
        {
            get { return _scale; }
            set
            {
                _dirty = true;
                _scale = value;
            }
        }

        private Vector3 _spacing;

        public override Vector3 Spacing
        {
            get { return _spacing; }
            set
            {
                _dirty = true;
                _spacing = value;
            }
        }
        private static readonly float Sqrt3 = Mathf.Sqrt(3);

        public PlanarHexMapper(Transform reference, Orientation mode) : base(reference)
        {
            Mode = mode;
        }

        public static readonly IntVector3[] DiagonalArray =
        {
            new IntVector3(2,-1, 0), 
            new IntVector3(-2, 1,0),
            new IntVector3(-1,-1,0),
            new IntVector3(1,1,0),
            new IntVector3(-1,2,0),
            new IntVector3(1,-2,0),
        };

        public static readonly IntVector3[] OrthogonalArray =
        {
            new IntVector3(1,0,0), 
            new IntVector3(1,-1,0),
            new IntVector3(-1,0,0),
            new IntVector3(0,-1,0),
            new IntVector3(-1,1,0),
            new IntVector3(0,1,0),
        };

        protected override IntVector3[] OrthogonalsFor(IntVector3 index)
        {
            return OrthogonalArray;
        }

        protected override IntVector3[] DiagonalsFor(IntVector3 index)
        {
            return DiagonalArray;
        }

        private IntVector3 RoundToHex(Vector3 v)
        {
            var rx = Mathf.RoundToInt(v.x);
            var ry = Mathf.RoundToInt(v.y);
            var rz = Mathf.RoundToInt(v.z);

            if (rx + ry + rz != 0)
            {
                var dx = Mathf.Abs(rx - v.x);
                var dy = Mathf.Abs(ry - v.y);
                var dz = Mathf.Abs(rz - v.z);

                if (dx > dy && dx > dz)
                {
                    rx = -ry - rz;
                }
                else if (dy <= dz)
                {
                    rz = -rx - ry;
                }
            }
            return new IntVector3(rx, rz, 0);
        }

        private bool _dirty = true;
        private float _a;
        private float A
        {
            get
            {
                if (_dirty)
                {
                    RecalculateBaseMetrics();
                }
                return _a;
            }
        }

        private void RecalculateBaseMetrics()
        {
            _a = 0.5f * (1 + _spacing.x) * _scale.x;
            _dirty = false;
        }

        public override Vector3 ToWorld(IntVector3 v)
        {
            float x;
            float y;
            switch (Mode)
            {
                case Orientation.FlatTop:
                    x = A*3f/2*v.x;
                    y = A*Sqrt3*(v.y + v.x/2f);
                    break;
                case Orientation.PointyTop:
                    x = A*Sqrt3*(v.x + v.y/2f);
                    y = A*3f/2*v.y;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return Reference.TransformPoint(new Vector3(x, y, 0));
        }

        public override IntVector3 ToGrid(Vector3 v)
        {
            var localPos = Reference.InverseTransformPoint(v);

            float q;
            float r;
            switch (Mode)
            {
                case Orientation.FlatTop:
                    q = 2f/3*localPos.x/A;
                    r = (-1f/3*localPos.x + 1f/3*Sqrt3*localPos.y)/A;
                    break;
                case Orientation.PointyTop:
                    q = (1f/3*Sqrt3*localPos.x - 1f/3*localPos.y)/A;
                    r = 2f/3*localPos.y/A;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return RoundToHex(new Vector3(q, -q-r, r));
        }

        public override IEnumerable<Vector3> Corners(IntVector3 v, bool includeSpacing = true)
        {
            var center = ToWorld(v);
            Vector3[] directions;

            var a = includeSpacing ? A : 0.5f*Scale.x;
            var h = Sqrt3*a/2;

            switch (Mode)
            {
                case Orientation.FlatTop:
                    directions =  new[]
                    {
                        new Vector3(-0.5f*a, h),
                        new Vector3(0.5f*a, h),
                        new Vector3(a, 0),
                        new Vector3(0.5f*a, -h),
                        new Vector3(-0.5f*a, -h),
                        new Vector3(-a, 0),
                    };
                    break;
                case Orientation.PointyTop:
                    directions = new[]
                    {
                        new Vector3(0, a),
                        new Vector3(h, 0.5f*a),
                        new Vector3(h, -0.5f*a),
                        new Vector3(0, -a),
                        new Vector3(-h, -0.5f*a),
                        new Vector3(-h, 0.5f*a),
                    };
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return directions.Select(d => center + Reference.TransformDirection(d));
        }
    }
}    


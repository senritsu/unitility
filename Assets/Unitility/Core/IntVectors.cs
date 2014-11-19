/***************************************************************************\
The MIT License (MIT)

Copyright (c) 2014 Jonas Schiegl (https://github.com/senritsu)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
\***************************************************************************/

using System;
using UnityEngine;

namespace Assets.Unitility.Core
{
    public struct IntVector2 : IEquatable<IntVector2>
    {
        public static IntVector2 zero = new IntVector2(0, 0);
        public static IntVector2 one = new IntVector2(1, 1);
        public static IntVector2 right = new IntVector2(1, 0);
        public static IntVector2 left = new IntVector2(-1, 0);
        public static IntVector2 up = new IntVector2(0, 1);
        public static IntVector2 down = new IntVector2(0, -1);
        public int x, y;

        public IntVector2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int sqrMagnitude
        {
            get { return x * x + y * y; }
        }

        public float magnitude
        {
            get { return Mathf.Sqrt(sqrMagnitude); }
        }

        public int this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return x;
                    case 1:
                        return y;
                    default:
                        throw new ArgumentOutOfRangeException("index", "index has to be 0 or 1");
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        x = value;
                        break;
                    case 1:
                        y = value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("index", "index has to be 0 or 1");
                }
            }
        }

        public bool Equals(IntVector2 other)
        {
            return x == other.x && y == other.y;
        }

        public void Set(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return string.Format("({0}, {1})", x, y);
        }

        public static IntVector2 Scale(IntVector2 v1, IntVector2 v2)
        {
            return new IntVector2(v1.x * v2.x, v1.y * v2.y);
        }

        public static float Distance(IntVector2 v1, IntVector2 v2)
        {
            return (v1 - v2).magnitude;
        }

        public static int Dot(IntVector2 v1, IntVector2 v2)
        {
            return v1.x * v2.x + v1.y * v2.y;
        }

        public static float Angle(IntVector2 v1, IntVector2 v2)
        {
            return Vector2.Angle(v1, v2);
        }

        public static IntVector2 Min(IntVector2 v1, IntVector2 v2)
        {
            return new IntVector2(Mathf.Min(v1.x, v2.x), Mathf.Min(v1.y, v2.y));
        }

        public static IntVector2 Max(IntVector2 v1, IntVector2 v2)
        {
            return new IntVector2(Mathf.Max(v1.x, v2.x), Mathf.Max(v1.y, v2.y));
        }

        public static IntVector2 operator +(IntVector2 v1, IntVector2 v2)
        {
            return new IntVector2(v1.x + v2.x, v1.y + v2.y);
        }

        public static IntVector2 operator -(IntVector2 v1, IntVector2 v2)
        {
            return new IntVector2(v1.x - v2.x, v1.y - v2.y);
        }

        public static IntVector2 operator -(IntVector2 v)
        {
            return new IntVector2(-v.x, -v.y);
        }

        public static IntVector2 operator +(IntVector2 v)
        {
            return new IntVector2(v.x, v.y);
        }

        public static IntVector2 operator *(int i, IntVector2 v)
        {
            return v * i;
        }

        public static IntVector2 operator *(IntVector2 v, int i)
        {
            return new IntVector2(v.x * i, v.y * i);
        }

        public static IntVector2 operator /(int i, IntVector2 v)
        {
            return v / i;
        }

        public static IntVector2 operator /(IntVector2 v, int i)
        {
            return new IntVector2(v.x / i, v.y / i);
        }

        public static implicit operator Vector2(IntVector2 v)
        {
            return new Vector2(v.x, v.y);
        }

        public static implicit operator IntVector2(Vector2 v)
        {
            return new IntVector2(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));
        }

        public static implicit operator Vector3(IntVector2 v)
        {
            return new Vector3(v.x, v.y);
        }

        public static implicit operator IntVector2(Vector3 v)
        {
            return new IntVector2(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));
        }

        public static implicit operator IntVector3(IntVector2 v)
        {
            return new IntVector3(v.x, v.y, 0);
        }

        public static implicit operator IntVector2(IntVector3 v)
        {
            return new IntVector2(v.x, v.y);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is IntVector2 && Equals((IntVector2)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (x * 397) ^ y;
            }
        }

        public static bool operator ==(IntVector2 left, IntVector2 right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(IntVector2 left, IntVector2 right)
        {
            return !left.Equals(right);
        }
    }

    public struct IntVector3 : IEquatable<IntVector3>
    {
        public static IntVector3 zero = new IntVector3(0, 0, 0);
        public static IntVector3 one = new IntVector3(1, 1, 1);
        public static IntVector3 right = new IntVector3(1, 0, 0);
        public static IntVector3 up = new IntVector3(0, 1, 0);
        public static IntVector3 forward = new IntVector3(0, 0, 1);
        public static IntVector3 left = new IntVector3(-1, 0, 0);
        public static IntVector3 down = new IntVector3(0, -1, 0);
        public static IntVector3 back = new IntVector3(0, 0, -1);
        public int x, y, z;

        public IntVector3(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public int sqrMagnitude
        {
            get { return x * x + y * y + z * z; }
        }

        public float magnitude
        {
            get { return Mathf.Sqrt(sqrMagnitude); }
        }

        public int this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return x;
                    case 1:
                        return y;
                    case 2:
                        return z;
                    default:
                        throw new ArgumentOutOfRangeException("index", "index has to be 0 or 1 or 2");
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        x = value;
                        break;
                    case 1:
                        y = value;
                        break;
                    case 2:
                        z = value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("index", "index has to be 0 or 1 or 2");
                }
            }
        }

        public bool Equals(IntVector3 other)
        {
            return x == other.x && y == other.y && z == other.z;
        }

        public void Set(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override string ToString()
        {
            return string.Format("({0}, {1}, {2})", x, y, z);
        }

        public static IntVector3 Scale(IntVector3 v1, IntVector3 v2)
        {
            return new IntVector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
        }

        public static float Distance(IntVector3 v1, IntVector3 v2)
        {
            return (v1 - v2).magnitude;
        }

        public static int Dot(IntVector3 v1, IntVector3 v2)
        {
            return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
        }

        public static IntVector3 Cross(IntVector3 v1, IntVector3 v2)
        {
            return new IntVector3(v1.y * v2.z - v1.z * v2.y, v1.z * v2.x - v1.x * v2.z, v1.x * v2.y - v1.y * v2.x);
        }

        public static float Angle(IntVector3 v1, IntVector3 v2)
        {
            return Vector3.Angle(v1, v2);
        }

        public static IntVector3 Min(IntVector3 v1, IntVector3 v2)
        {
            return new IntVector3(Mathf.Min(v1.x, v2.x), Mathf.Min(v1.y, v2.y), Mathf.Min(v1.z, v2.z));
        }

        public static IntVector3 Max(IntVector3 v1, IntVector3 v2)
        {
            return new IntVector3(Mathf.Max(v1.x, v2.x), Mathf.Max(v1.y, v2.y), Mathf.Max(v1.z, v2.z));
        }

        public static IntVector3 operator +(IntVector3 v1, IntVector3 v2)
        {
            return new IntVector3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }

        public static IntVector3 operator -(IntVector3 v1, IntVector3 v2)
        {
            return new IntVector3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }

        public static IntVector3 operator -(IntVector3 v)
        {
            return new IntVector3(-v.x, -v.y, -v.z);
        }

        public static IntVector3 operator +(IntVector3 v)
        {
            return new IntVector3(v.x, v.y, v.z);
        }

        public static IntVector3 operator *(int i, IntVector3 v)
        {
            return v * i;
        }

        public static IntVector3 operator *(IntVector3 v, int i)
        {
            return new IntVector3(v.x * i, v.y * i, v.z * i);
        }

        public static IntVector3 operator /(int i, IntVector3 v)
        {
            return v / i;
        }

        public static IntVector3 operator /(IntVector3 v, int i)
        {
            return new IntVector3(v.x / i, v.y / i, v.z / i);
        }

        public static implicit operator Vector3(IntVector3 v)
        {
            return new Vector3(v.x, v.y, v.z);
        }

        public static implicit operator IntVector3(Vector3 v)
        {
            return new IntVector3(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y), Mathf.RoundToInt(v.z));
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is IntVector3 && Equals((IntVector3)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = x;
                hashCode = (hashCode * 397) ^ y;
                hashCode = (hashCode * 397) ^ z;
                return hashCode;
            }
        }

        public static bool operator ==(IntVector3 left, IntVector3 right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(IntVector3 left, IntVector3 right)
        {
            return !left.Equals(right);
        }
    }
}
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

public struct IVector2 : IEquatable<IVector2>
{
    public static IVector2 zero = new IVector2(0, 0);
    public static IVector2 one = new IVector2(1, 1);
    public static IVector2 right = new IVector2(1, 0);
    public static IVector2 up = new IVector2(0, 1);
    public int x, y;

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

    public IVector2(int x, int y)
    {
        this.x = x;
        this.y = y;
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

    public static IVector2 Scale(IVector2 v1, IVector2 v2)
    {
        return new IVector2(v1.x * v2.x, v1.y * v2.y);
    }

    public static float Distance(IVector2 v1, IVector2 v2)
    {
        return (v1 - v2).magnitude;
    }

    public static int Dot(IVector2 v1, IVector2 v2)
    {
        return v1.x * v2.x + v1.y * v2.y;
    }

    public static float Angle(IVector2 v1, IVector2 v2)
    {
        return Vector2.Angle(v1, v2);
    }

    public static IVector2 Min(IVector2 v1, IVector2 v2)
    {
        return new IVector2(Mathf.Min(v1.x, v2.x), Mathf.Min(v1.y, v2.y));
    }

    public static IVector2 Max(IVector2 v1, IVector2 v2)
    {
        return new IVector2(Mathf.Max(v1.x, v2.x), Mathf.Max(v1.y, v2.y));
    }

    public IVector2 RotateLeft()
    {
        return new IVector2(-y, x);
    }

    public IVector2 RotateRight()
    {
        return new IVector2(y, -x);
    }

    public static IVector2 operator +(IVector2 v1, IVector2 v2)
    {
        return new IVector2(v1.x + v2.x, v1.y + v2.y);
    }

    public static IVector2 operator -(IVector2 v1, IVector2 v2)
    {
        return new IVector2(v1.x - v2.x, v1.y - v2.y);
    }

    public static IVector2 operator -(IVector2 v)
    {
        return new IVector2(-v.x, -v.y);
    }

    public static IVector2 operator +(IVector2 v)
    {
        return new IVector2(v.x, v.y);
    }

    public static IVector2 operator *(int i, IVector2 v)
    {
        return v * i;
    }

    public static IVector2 operator *(IVector2 v, int i)
    {
        return new IVector2(v.x * i, v.y * i);
    }

    public static IVector2 operator /(int i, IVector2 v)
    {
        return v / i;
    }

    public static IVector2 operator /(IVector2 v, int i)
    {
        return new IVector2(v.x / i, v.y / i);
    }

    public static implicit operator Vector2(IVector2 v)
    {
        return new Vector2(v.x, v.y);
    }

    public static implicit operator IVector2(Vector2 v)
    {
        return new IVector2(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));
    }

    public static implicit operator Vector3(IVector2 v)
    {
        return new Vector3(v.x, v.y);
    }

    public static implicit operator IVector2(Vector3 v)
    {
        return new IVector2(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));
    }

    public static implicit operator IVector3(IVector2 v)
    {
        return new IVector3(v.x, v.y, 0);
    }

    public static implicit operator IVector2(IVector3 v)
    {
        return new IVector2(v.x, v.y);
    }

    public bool Equals(IVector2 other)
    {
        return x == other.x && y == other.y;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        return obj is IVector2 && Equals((IVector2)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (x * 397) ^ y;
        }
    }

    public static bool operator ==(IVector2 left, IVector2 right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(IVector2 left, IVector2 right)
    {
        return !left.Equals(right);
    }
}

public struct IVector3 : IEquatable<IVector3>
{
    public static IVector3 zero = new IVector3(0, 0, 0);
    public static IVector3 one = new IVector3(1, 1, 1);
    public static IVector3 right = new IVector3(1, 0, 0);
    public static IVector3 up = new IVector3(0, 1, 0);
    public static IVector3 forward = new IVector3(0, 0, 1);
    public static IVector3 left = new IVector3(-1, 0, 0);
    public static IVector3 down = new IVector3(0, -1, 0);
    public static IVector3 back = new IVector3(0, 0, -1);
    public int x, y, z;

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

    public IVector3(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
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

    public static IVector3 Scale(IVector3 v1, IVector3 v2)
    {
        return new IVector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
    }

    public static float Distance(IVector3 v1, IVector3 v2)
    {
        return (v1 - v2).magnitude;
    }

    public static int Dot(IVector3 v1, IVector3 v2)
    {
        return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
    }

    public static IVector3 Cross(IVector3 v1, IVector3 v2)
    {
        return new IVector3(v1.y * v2.z - v1.z * v2.y, v1.z * v2.x - v1.x * v2.z, v1.x * v2.y - v1.y * v2.x);
    }

    public static float Angle(IVector3 v1, IVector3 v2)
    {
        return Vector3.Angle(v1, v2);
    }

    public static IVector3 Min(IVector3 v1, IVector3 v2)
    {
        return new IVector3(Mathf.Min(v1.x, v2.x), Mathf.Min(v1.y, v2.y), Mathf.Min(v1.z, v2.z));
    }

    public static IVector3 Max(IVector3 v1, IVector3 v2)
    {
        return new IVector3(Mathf.Max(v1.x, v2.x), Mathf.Max(v1.y, v2.y), Mathf.Max(v1.z, v2.z));
    }

    public static IVector3 operator +(IVector3 v1, IVector3 v2)
    {
        return new IVector3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
    }

    public static IVector3 operator -(IVector3 v1, IVector3 v2)
    {
        return new IVector3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
    }

    public static IVector3 operator -(IVector3 v)
    {
        return new IVector3(-v.x, -v.y, -v.z);
    }

    public static IVector3 operator +(IVector3 v)
    {
        return new IVector3(v.x, v.y, v.z);
    }

    public static IVector3 operator *(int i, IVector3 v)
    {
        return v * i;
    }

    public static IVector3 operator *(IVector3 v, int i)
    {
        return new IVector3(v.x * i, v.y * i, v.z * i);
    }

    public static IVector3 operator /(int i, IVector3 v)
    {
        return v / i;
    }

    public static IVector3 operator /(IVector3 v, int i)
    {
        return new IVector3(v.x / i, v.y / i, v.z / i);
    }

    public static implicit operator Vector3(IVector3 v)
    {
        return new Vector3(v.x, v.y, v.z);
    }

    public static implicit operator IVector3(Vector3 v)
    {
        return new IVector3(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y), Mathf.RoundToInt(v.z));
    }

    public bool Equals(IVector3 other)
    {
        return x == other.x && y == other.y && z == other.z;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        return obj is IVector3 && Equals((IVector3)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hashCode = x;
            hashCode = (hashCode * 397) ^ y;
            hashCode = (hashCode * 397) ^ z;
            return hashCode;
        }
    }

    public static bool operator ==(IVector3 left, IVector3 right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(IVector3 left, IVector3 right)
    {
        return !left.Equals(right);
    }
}
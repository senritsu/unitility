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
using Assets.Unitility.Core;

namespace Assets.Unitility.Grid
{
    public static class IndexTransformUtilities
    {
        private static int PositiveMod(int a, int b)
        {
            return a < 0
                ? a%b + b
                : a%b;
        }

        public static Func<IntVector3, IntVector3> WrapAllAxes(IntVector3 min, IntVector3 max)
        {
            var size = max - min;
            return i =>
            {
                i = new IntVector3(
                    PositiveMod(i.x - min.x, size.x + 1) + min.x,
                    PositiveMod(i.y - min.y, size.y + 1) + min.y,
                    PositiveMod(i.z - min.z, size.z + 1) + min.z);
                return i;
            };
        }

        public static Func<IntVector3, IntVector3> WrapX(int min, int max)
        {
            var size = max - min;
            return i =>
            {
                i = new IntVector3(
                    PositiveMod(i.x - min, size + 1) + min,
                    i.y,
                    i.z);
                return i;
            };
        }

        public static Func<IntVector3, IntVector3> WrapY(int min, int max)
        {
            var size = max - min;
            return i =>
            {
                i = new IntVector3(
                    i.x,
                    PositiveMod(i.y - min, size + 1) + min,
                    i.z);
                return i;
            };
        }

        public static Func<IntVector3, IntVector3> WrapZ(int min, int max)
        {
            var size = max - min;
            return i =>
            {
                i = new IntVector3(
                    i.x,
                    i.y,
                    PositiveMod(i.z - min, size + 1) + min);
                return i;
            };
        }

        public static Func<IntVector3, IntVector3> HexWrap(IntVector3 min, IntVector3 max)
        {
            var size = max - min;
            return i =>
            {
                i = new IntVector3(
                    PositiveMod(i.x - min.x, size.x + 1) + min.x,
                    PositiveMod(i.y - min.y, size.y + 1) + min.y,
                    PositiveMod(i.z - min.z, size.z + 1) + min.z);
                return i;
            };
        }
    }
}
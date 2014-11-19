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

using Assets.Unitility.Core;
using NUnit.Framework;

namespace Assets.Unitility.Tests
{
    [TestFixture]
    public class IntVectorTests {

        [Test]
        public void IntVector2RotateTests()
        {
            var v = IntVector2.up;

            Assert.That(v.RotateLeft(), Is.EqualTo(-IntVector2.right));
            Assert.That(v.RotateRight(), Is.EqualTo(IntVector2.right));

            v = IntVector2.right;

            Assert.That(v.RotateLeft(), Is.EqualTo(IntVector2.up));
            Assert.That(v.RotateRight(), Is.EqualTo(-IntVector2.up));

            v = IntVector2.one;

            Assert.That(v.RotateLeft(), Is.EqualTo(new IntVector2(-1, 1)));
            Assert.That(v.RotateRight(), Is.EqualTo(new IntVector2(1, -1)));
        }

        [Test]
        public void IntVector3RotateXYTests()
        {
            var v = IntVector3.up;

            Assert.That(v.RotateXYLeft(), Is.EqualTo(IntVector3.left));
            Assert.That(v.RotateXYRight(), Is.EqualTo(IntVector3.right));

            v = IntVector3.right;

            Assert.That(v.RotateXYLeft(), Is.EqualTo(IntVector3.up));
            Assert.That(v.RotateXYRight(), Is.EqualTo(IntVector3.down));

            v = IntVector3.forward;

            Assert.That(v.RotateXYLeft(), Is.EqualTo(IntVector3.forward));
            Assert.That(v.RotateXYRight(), Is.EqualTo(IntVector3.forward));

            v = IntVector3.one;

            Assert.That(v.RotateXYLeft(), Is.EqualTo(new IntVector3(-1, 1, 1)));
            Assert.That(v.RotateXYRight(), Is.EqualTo(new IntVector3(1, -1, 1)));
        }

        [Test]
        public void IntVector3RotateXZTests()
        {
            var v = IntVector3.up;

            Assert.That(v.RotateXZLeft(), Is.EqualTo(IntVector3.up));
            Assert.That(v.RotateXZRight(), Is.EqualTo(IntVector3.up));

            
            v = IntVector3.right;

            Assert.That(v.RotateXZLeft(), Is.EqualTo(IntVector3.forward));
            Assert.That(v.RotateXZRight(), Is.EqualTo(IntVector3.back));

            v = IntVector3.forward;

            Assert.That(v.RotateXZLeft(), Is.EqualTo(IntVector3.left));
            Assert.That(v.RotateXZRight(), Is.EqualTo(IntVector3.right));
            
            v = IntVector3.one;

            Assert.That(v.RotateXZLeft(), Is.EqualTo(new IntVector3(-1, 1, 1)));
            Assert.That(v.RotateXZRight(), Is.EqualTo(new IntVector3(1, 1, -1)));
        }

        [Test]
        public void IntVector3RotateZYTests()
        {
            var v = IntVector3.up;

            Assert.That(v.RotateZYLeft(), Is.EqualTo(IntVector3.back));
            Assert.That(v.RotateZYRight(), Is.EqualTo(IntVector3.forward));
            
            v = IntVector3.right;

            Assert.That(v.RotateZYLeft(), Is.EqualTo(IntVector3.right));
            Assert.That(v.RotateZYRight(), Is.EqualTo(IntVector3.right));

            v = IntVector3.forward;

            Assert.That(v.RotateZYLeft(), Is.EqualTo(IntVector3.up));
            Assert.That(v.RotateZYRight(), Is.EqualTo(IntVector3.down));

            v = IntVector3.one;

            Assert.That(v.RotateZYLeft(), Is.EqualTo(new IntVector3(1, 1, -1)));
            Assert.That(v.RotateZYRight(), Is.EqualTo(new IntVector3(1, -1, 1)));
        }
    }
}
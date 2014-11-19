using System;
using System.Linq;
using Assets.Unitility.Core;
using Assets.Unitility.Grid;
using NUnit.Framework;

namespace Assets.Unitility.Tests
{
    [TestFixture]
    public class SparseGridTests2
    {
        [Test]
        public void CanAssignAndRetrieveValues()
        {
            var grid = new SparseGrid2<String>();

            var foo = "Foo";
            var bar = "Foo";

            grid[0, 2] = foo;
            grid[IntVector2.left] = bar;

            Assert.That(grid.HasIndex(new IntVector2(0, 2)), Is.True);
            Assert.That(grid[new IntVector2(0, 2)], Is.EqualTo(foo));
            Assert.That(grid[-1, 0], Is.EqualTo(bar));
            Assert.That(grid.HasIndex(new IntVector2(2, 3)), Is.False);
            Assert.That(grid[2, 3], Is.Null);
        }

        [Test]
        public void CanRemoveItems()
        {
            var grid = new SparseGrid2<String>();

            grid[IntVector2.zero] = "foo";
            grid[IntVector2.one] = "bar";
            grid[2*IntVector2.one] = "baz";

            grid.Remove(new IntVector2(0, 0));
            grid[2, 2] = null;

            Assert.That(grid.Keys.Count(), Is.EqualTo(1));
            Assert.That(grid.Keys.First(), Is.EqualTo(new IntVector2(1, 1)));
            Assert.That(grid.Values.First(), Is.EqualTo("bar"));
        }

        [Test]
        public void UpdatesBoundsCorrectly()
        {
            var grid = new SparseGrid2<String>();

            grid[0, -3] = "foo";
            grid[1, 8] = "bar";

            Assert.That(grid.Bounds.min, Is.EqualTo((IntVector3) new IntVector2(0, -3)));
            Assert.That(grid.Bounds.max, Is.EqualTo((IntVector3) new IntVector2(1, 8)));

            grid[2, 3] = "bar";
            grid[-2, -5] = "baz";

            Assert.That(grid.Bounds.min, Is.EqualTo((IntVector3) new IntVector2(-2, -5)));
            Assert.That(grid.Bounds.max, Is.EqualTo((IntVector3) new IntVector2(2, 8)));

            grid[1, 8] = null;
            grid[-2, -5] = null;

            Assert.That(grid.Bounds.min, Is.EqualTo((IntVector3) new IntVector2(0, -3)));
            Assert.That(grid.Bounds.max, Is.EqualTo((IntVector3) new IntVector2(2, 3)));
        }

        [Test]
        public void GetsNeighborsCorrectly()
        {
            var grid = new SparseGrid3<String>();

            grid[0, 0, 0] = "a";
            // orthogonal
            grid[0, 1, 0] = "o";
            grid[-1, 0, 0] = "o";
            // double diagonal 
            grid[1, 1, 1] = "dd";
            // planar diagonal
            grid[1, -1, 0] = "d";
            // not adjacent
            grid[2, 2, 2] = "n";

            Assert.That(grid.Neighbors(IntVector3.zero), Is.EquivalentTo(new[] {"o", "o"}));
            Assert.That(grid.Neighbors(IntVector3.zero, true), Is.EquivalentTo(new[] {"o", "o", "d", "dd"}));
        }
    }
}
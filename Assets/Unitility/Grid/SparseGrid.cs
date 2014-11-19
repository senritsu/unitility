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

using System.Collections.Generic;
using System.Linq;
using Assets.Unitility.Core;
using UnityEngine;

namespace Assets.Unitility.Grid
{
    public class IntBounds
    {
        public IntVector3 min { get; set; }
        public IntVector3 max { get; set; }

        public static implicit operator Bounds(IntBounds bounds)
        {
            return new Bounds(0.5f*(Vector3) (bounds.max + bounds.min), bounds.max - bounds.min);
        }
    }

    public abstract class BaseSparseGrid<TKey, TValue> where TValue : class
    {
        protected IntBounds _bounds;
        protected bool _boundsDirty;
        protected Dictionary<TKey, TValue> _grid;

        protected BaseSparseGrid()
        {
            _grid = new Dictionary<TKey, TValue>();
            _bounds = new IntBounds();
        }

        protected abstract TKey[] Orthogonals { get; }
        protected abstract TKey[] Diagonals { get; }

        public IEnumerable<TKey> Keys
        {
            get { return _grid.Keys; }
        }

        public IEnumerable<TValue> Values
        {
            get { return _grid.Values; }
        }

        public bool HasIndex(TKey index)
        {
            return _grid.ContainsKey(index);
        }

        public IntBounds Bounds
        {
            get { return _boundsDirty ? RecalculateBounds() : _bounds; }
        }

        public TValue this[TKey index]
        {
            get
            {
                TValue value;
                return _grid.TryGetValue(index, out value) ? value : null;
            }
            set { Add(index, value); }
        }

        public void Add(TKey index, TValue value)
        {
            if (value == null)
            {
                Remove(index);
            }
            else
            {
                if (!_grid.ContainsKey(index))
                {
                    ExtendBounds(index);
                }
                _grid[index] = value;
            }
        }

        public void Remove(TKey index)
        {
            if (!_grid.ContainsKey(index)) return;
            _grid.Remove(index);
            _boundsDirty = true;
        }

        public IEnumerable<TValue> Neighbors(TKey index, bool includeDiagonals = false)
        {
            var result = new List<TValue>();
            result.AddRange(Orthogonals.Select(v => this[v]).Where(x => x != null));
            if (includeDiagonals)
            {
                result.AddRange(Diagonals.Select(v => this[v]).Where(x => x != null));
            }
            return result;
        }

        protected IntBounds RecalculateBounds()
        {
            _bounds = new IntBounds();
            foreach (var index in _grid.Keys)
            {
                ExtendBounds(index);
            }
            _boundsDirty = false;
            return _bounds;
        }

        protected abstract void ExtendBounds(TKey index);
    }

    public class SparseGrid2<T> : BaseSparseGrid<IntVector2, T> where T : class
    {
        private readonly IntVector2[] _diagonalArray =
        {
            new IntVector2(1, 1),
            new IntVector2(-1, 1),
            new IntVector2(1, -1),
            new IntVector2(-1, -1)
        };

        private readonly IntVector2[] _orthogonalArray =
        {
            IntVector2.up, IntVector2.right, IntVector2.down, IntVector2.left
        };

        protected override IntVector2[] Diagonals
        {
            get { return _diagonalArray; }
        }

        protected override IntVector2[] Orthogonals
        {
            get { return _orthogonalArray; }
        }

        public T this[int x, int y]
        {
            get
            {
                T value;
                return _grid.TryGetValue(new IntVector2(x, y), out value) ? value : null;
            }
            set { Add(new IntVector2(x, y), value); }
        }

        protected override void ExtendBounds(IntVector2 index)
        {
            _bounds.min = IntVector2.Min(_bounds.min, index);
            _bounds.max = IntVector2.Max(_bounds.max, index);
        }
    }

    public class SparseGrid3<T> : BaseSparseGrid<IntVector3, T> where T : class
    {
        private readonly IntVector3[] _diagonalArray =
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

        private readonly IntVector3[] _orthogonalArray =
        {
            IntVector3.right,
            IntVector3.left,
            IntVector3.up,
            IntVector3.down,
            IntVector3.forward,
            IntVector3.back
        };

        protected override IntVector3[] Diagonals
        {
            get { return _diagonalArray; }
        }

        protected override IntVector3[] Orthogonals
        {
            get { return _orthogonalArray; }
        }

        public T this[int x, int y, int z]
        {
            get
            {
                T value;
                return _grid.TryGetValue(new IntVector3(x, y, z), out value) ? value : null;
            }
            set { Add(new IntVector3(x, y, z), value); }
        }

        protected override void ExtendBounds(IntVector3 index)
        {
            _bounds.min = IntVector3.Min(_bounds.min, index);
            _bounds.max = IntVector3.Max(_bounds.max, index);
        }
    }
}
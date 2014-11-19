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
    public class SparseGrid<T> where T : class
    {
        private Bounds _bounds;
        private bool _boundsDirty;

        private readonly IntVector2[] _diagonals =
        {
            IntVector2.one, IntVector2.one - IntVector2.up, -IntVector2.one,
            IntVector2.one - IntVector2.right
        };

        private readonly Dictionary<IntVector2, T> _grid;

        private readonly IntVector2[] _orthogonals =
        {
            IntVector2.up, IntVector2.right, -IntVector2.up, -IntVector2.right
        };

        public SparseGrid()
        {
            _grid = new Dictionary<IntVector2, T>();
        }

        public Bounds Bounds
        {
            get { return _boundsDirty ? RecalculateBounds() : _bounds; }
        }

        public IEnumerable<IntVector2> Keys
        {
            get { return _grid.Keys; }
        }

        public IEnumerable<T> Values
        {
            get { return _grid.Values; }
        }

        public T this[IntVector2 index]
        {
            get
            {
                T value;
                return _grid.TryGetValue(index, out value) ? value : null;
            }
            set { Add(index, value); }
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

        public void Add(IntVector2 index, T value)
        {
            if (!_grid.ContainsKey(index))
            {
                _bounds.min = Vector3.Min(_bounds.min, index);
                _bounds.max = Vector3.Max(_bounds.max, index);
            }
            _grid[index] = value;
        }

        public void Remove(IntVector2 index)
        {
            if (!_grid.ContainsKey(index)) return;
            _grid.Remove(index);
            _boundsDirty = true;
        }

        public IEnumerable<T> Neighbors(IntVector2 index, bool includeDiagonals = false)
        {
            var result = new List<T>();
            result.AddRange(_orthogonals.Select(v => this[v]).Where(x => x != null));
            if (includeDiagonals)
            {
                result.AddRange(_diagonals.Select(v => this[v]).Where(x => x != null));
            }
            return result;
        }

        private Bounds RecalculateBounds()
        {
            var min = IntVector2.zero;
            var max = IntVector2.zero;
            foreach (var index in _grid.Keys)
            {
                min = IntVector2.Min(min, index);
                max = IntVector2.Max(max, index);
            }
            var center = new Vector3();
            var size = new Vector3();
            _bounds = new Bounds(center, size);
            _boundsDirty = false;
            return _bounds;
        }
    }
}
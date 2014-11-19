using Assets.Unitility.Grid;
using UnityEngine;

namespace Assets.Unitility.MonoBehaviours
{
    public class GridRenderer<T> : MonoBehaviour where T : class
    {
        private Transform _contentContainer;
        private SparseGrid<T> _grid;
        public float FieldSize;
        public float Spacing;
        // Use this for initialization
        private void Start()
        {
            _contentContainer = transform.Find("Content");
            _grid = new SparseGrid<T>();
        }

        public void Recenter()
        {
            var offset = -_grid.Bounds.center;
            _contentContainer.localPosition = offset;
        }

        public T GetField(Vector3 position)
        {
            position = transform.InverseTransformPoint(position) + _grid.Bounds.center;

            var x = Mathf.RoundToInt(position.x/Spacing);
            var y = Mathf.RoundToInt(position.z/Spacing);
            return _grid[x, y];
        }

        public Vector3 GetWorldPos(int x, int y)
        {
            return
                transform.TransformPoint(new Vector3(x*(FieldSize + Spacing), 0, y*(FieldSize + Spacing)) -
                                         _grid.Bounds.center);
        }

        public

            // Update is called once per frame
            void Update()
        {
        }
    }
}
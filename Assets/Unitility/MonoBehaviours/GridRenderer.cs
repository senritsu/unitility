using System;
using System.Collections.Generic;
using Assets.Unitility.Core;
using Assets.Unitility.Grid;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Unitility.MonoBehaviours
{
    public enum GridType
    {
        Square,
        Rectangular,
        Triangular,
        HexFlat,
        HexPointy
    }
    public class GridRenderer : MonoBehaviour
    {
        private Transform _contentContainer;
        private SparseGrid3<SpriteRenderer> _grid;

        private GridType _previousType;
        public GridType Type;
        public Sprite[] Sprites;

        private GridGeometryMapper _mapper;

        private Vector2 _previousFieldSize;
        public Vector2 Scale;
        private Vector2 _previousSpacing;
        public Vector2 Spacing;
        public GameObject Prefab;
        // Use this for initialization

        private void Start()
        {
            _contentContainer = transform.Find("Content");
            _grid = new SparseGrid3<SpriteRenderer>();
            _mapper = new PlanarSquareMapper(_contentContainer)
            {
                Scale = Scale, 
                Spacing = Spacing
            };
        }

        public void ApplyGridTypeChange()
        {
            _previousType = Type;
            _previousFieldSize = Scale;
            GameObject.Find("ScaleX").GetComponent<Text>().text = Scale.x.ToString("F1");
            GameObject.Find("ScaleY").GetComponent<Text>().text = Scale.y.ToString("F1");
            _previousSpacing = Spacing;
            GameObject.Find("SpacingX").GetComponent<Text>().text = Spacing.x.ToString("F1");
            GameObject.Find("SpacingY").GetComponent<Text>().text = Spacing.y.ToString("F1");

            switch (Type)
            {
                case GridType.Square:
                    _mapper = new PlanarSquareMapper(_contentContainer)
                    {
                        Scale = Scale, 
                        Spacing = Spacing
                    };
                    break;
                case GridType.Rectangular:
                    _mapper = new PlanarRectangularMapper(_contentContainer)
                    {
                        Scale = Scale,
                        Spacing = Spacing
                    };
                    break;
                case GridType.Triangular:
                    _mapper = new PlanarTriangleMapper(_contentContainer)
                    {
                        Scale = Scale,
                        Spacing = Spacing
                    };
                    break;
                case GridType.HexFlat:
                    _mapper = new PlanarHexMapper(_contentContainer, PlanarHexMapper.Orientation.FlatTop)
                    {
                        Scale = Scale,
                        Spacing = Spacing
                    };
                    break;
                case GridType.HexPointy:
                    _mapper = new PlanarHexMapper(_contentContainer, PlanarHexMapper.Orientation.PointyTop)
                    {
                        Scale = Scale,
                        Spacing = Spacing
                    };
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            GameObject.Find("GridType").GetComponent<Text>().text = Type.ToString();

            foreach (var index in _grid.Keys)
            {
                UpdateSprite(index);
            }
        }

        public void UpdateSprite(IntVector3 index)
        {
            var sr = _grid[index];
            
            sr.transform.localScale = Type == GridType.Rectangular 
                ? (Vector3) Scale 
                : Scale.x*Vector3.one;
            sr.sprite = Sprites[(int) Type];

            sr.transform.position = _mapper.ToWorld(index);
            sr.transform.rotation = _contentContainer.rotation;
            if (Type == GridType.Triangular)
            {
                sr.transform.localRotation = index.z == 0
                    ? Quaternion.identity
                    : Quaternion.AngleAxis(180, Vector3.forward);
            }
            if (Type == GridType.HexPointy)
            {
                sr.transform.localRotation = Quaternion.AngleAxis(30, Vector3.forward);
            }
        }

        public void Recenter()
        {
            var offset = -((Bounds) _grid.Bounds).center;
            _contentContainer.localPosition = offset;
        }

        public void Switch(IntVector3 index)
        {
            var item = _grid[index];
            if (item == null)
            {
                var go = Instantiate(Prefab) as GameObject;
                go.transform.SetParent(_contentContainer);
                _grid[index] = go.GetComponentInChildren<SpriteRenderer>();
                foreach (var corner in _mapper.Corners(index, false))
                {
                    Debug.DrawLine(corner, corner - _contentContainer.forward, Color.red, 2);
                }
                UpdateSprite(index);
            }
            else
            {
                _grid[index] = null;
                Destroy(item.gameObject);
            }
        }

        public void Update()
        {
            var click = -1;
            for (int i = 0; i < 3; i++)
            {
                if (Input.GetMouseButtonDown(i))
                {
                    click = i;
                }
            }
            if (click >= 0)
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                var plane = new Plane(_contentContainer.forward, _contentContainer.position);
                float distance;
                if (plane.Raycast(ray, out distance))
                {
                    var point = ray.GetPoint(distance);

                    var gridPos = _mapper.ToGrid(point);
                    var gridPositions = click == 0 
                        ? new List<IntVector3> {gridPos} 
                        : _mapper.Neighbors(gridPos, click == 2);
                    foreach (var index in gridPositions)
                    {
                        Switch(index);
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Type = (GridType) (((int) Type + 1)%5);
            }
            if (_previousType != Type || _previousFieldSize != Scale || _previousSpacing != Spacing)
            {
                ApplyGridTypeChange();   
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                Scale += 0.1f*Vector2.one;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                Scale -= 0.1f * Vector2.one;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                Spacing -= 0.1f * Vector2.one;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                Spacing += 0.1f * Vector2.one;
            }
        }
    }
}
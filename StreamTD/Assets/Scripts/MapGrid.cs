using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Controllers;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Assets.Scripts
{
    public class MapGrid
    {
        private readonly (int x, int y) _mapSize = (18, 18);
        public readonly MapField[][] Map;

        private SpriteRenderer[][] _fieldsSpriteRenderers;
        private GameObject[][] _fieldsObjects;
        private float _gridDistance = 1f;

        //map[x][y]
        //map:
        //x0:y0y1y2y3
        //x1:y0y1y2y3
        //x2:y0y1y2y3

        public MapField[] Path;

        public MapGrid(GameObject fieldPrefab, Transform parent, GameController gc)
        {
            GenerateEmptyMap(ref Map);
            RandomizeTerrainBlocks(ref Map, 30);
            RandomizePath(ref Map);
            Instantiate(fieldPrefab, parent, gc);
        }

        private void GenerateEmptyMap(ref MapField[][] map)
        {
            map = new MapField[_mapSize.x][];
            for (var i = 0; i < _mapSize.x; i++)
            {
                map[i] = new MapField[_mapSize.y];
                for (var j = 0; j < Map[i].Length; j++) map[i][j] = new MapField();
            }
        }

        private void RandomizeTerrainBlocks(ref MapField[][] map, int count)
        {
            var result = new Vector2Int[count];
            var emptyFields = new List<Vector2Int>();

            for (var i = 0; i < map.Length; i++)
            for (var j = 0; j < map[i].Length; j++)
                if (map[i][j].Type == MapFieldType.Empty)
                    emptyFields.Add(new Vector2Int(i, j));

            if (emptyFields.Count < count)
                throw new ArgumentException(
                    "You are trying to randomize more terrain fields than there are free fields on the map. Try again with proper parameters this time");

            for (var i = 0; i < count; i++)
            {
                var randIndex = GameController.RandomGenerator.Next(0, emptyFields.Count - 1);
                result[i] = emptyFields[randIndex];
                map[result[i].x][result[i].y].Type = MapFieldType.Terrain;
                emptyFields.RemoveAt(randIndex);
            }
        }

        private void RandomizePath(ref MapField[][] map)
        {
            var path = new List<MapField>();
            var pathX = GameController.RandomGenerator.Next(0, _mapSize.x);
            var pathY = 0;

            var wasUpwardsMove = false;
            var wasDownwardsMove = false;

            do
            {
                path.Add(map[pathX][pathY]);
                path.Last().Type = MapFieldType.Path;

                if (GameController.RandomGenerator.Next(0, 101) < 66)
                {
                    var moveUp = GameController.RandomGenerator.Next(0, 101) < 50;
                    if (moveUp && !wasDownwardsMove && pathX > 0)
                    {
                        pathX--;
                        wasUpwardsMove = true;
                    }
                    else if (!wasUpwardsMove && pathX < _mapSize.x - 1)
                    {
                        pathX++;
                        wasDownwardsMove = true;
                    }
                    else
                    {
                        pathY++;
                        wasUpwardsMove = false;
                        wasDownwardsMove = false;
                    }
                }
                else
                {
                    pathY++;
                    wasUpwardsMove = false;
                    wasDownwardsMove = false;
                }
            } while (pathY < _mapSize.y);

            Path = path.ToArray();
            //Setting last path tile to be a base
            Path[Path.Length - 1].Type = MapFieldType.Base;
        }

        private void Instantiate(GameObject fieldPrefab, Transform parent, GameController gc)
        {
            _fieldsSpriteRenderers = new SpriteRenderer[_mapSize.x][];
            _fieldsObjects = new GameObject[_mapSize.x][];

            var offsetValue = -Map.Length * 0.4725f;
            var offset = new Vector3(offsetValue, offsetValue, 0);
            for (var i = 0; i < Map.Length; i++)
            {
                _fieldsSpriteRenderers[i] = new SpriteRenderer[_mapSize.x];
                _fieldsObjects[i] = new GameObject[_mapSize.x];

                for (var j = 0; j < Map[i].Length; j++)
                {
                    GenerateGameObjectsForMapFields(i, j, fieldPrefab, parent, offset);
                    AddListeners(gc, i, j);
                }
            }

            Render();
        }

        private void GenerateGameObjectsForMapFields(int i, int j, GameObject fieldPrefab, Transform parent, Vector3 offset)
        {
            _fieldsObjects[i][j] = Object.Instantiate(fieldPrefab, parent);
            _fieldsSpriteRenderers[i][j] = _fieldsObjects[i][j].GetComponent<SpriteRenderer>();

            var position = new Vector3(i * _gridDistance, j * _gridDistance) + offset;
            position.z = 0;

            Map[i][j].Position = position;
            _fieldsObjects[i][j].transform.localPosition = position;
        }

        private void AddListeners(GameController gc, int i, int j)
        {
            Map[i][j].OnTypeChanged += () => RenderTile(i, j);
            var btn = _fieldsObjects[i][j].AddComponent<Button>();
            btn.onClick.AddListener(() =>
            {
                Map[i][j].OnClick(gc.Hqm);
                Render();
            });
            Map[i][j].Button = btn;
        }

        private void RenderTile(int x, int y)
        {
            _fieldsSpriteRenderers[x][y].color = MapField.FieldColors[Map[x][y].Type];
        }

        public void Render()
        {
            for (var i = 0; i < Map.Length; i++)
            for (var j = 0; j < Map[i].Length; j++)
                RenderTile(i, j);
        }
    }
}
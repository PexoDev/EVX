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
        private float _gridDistance = 0.6f;
        private readonly (int x, int y) _mapSize = (17, 17);
        //map[x][y]
        //map:
        //x0:y0y1y2y3
        //x1:y0y1y2y3
        //x2:y0y1y2y3

        public MapField[] Path;
        public readonly MapField[][] Map;

        private Image[][] _fieldsImages;
        private GameObject[][] _fieldsObjects;

        public MapGrid(GameObject fieldPrefab, Canvas canvas, GameController gc)
        {
            GenerateEmptyMap(ref Map);
            RandomizeTerrainBlocks(ref Map, 30);
            RandomizePath(ref Map);
            Instantiate(fieldPrefab, canvas, gc);
        }

        private void GenerateEmptyMap(ref MapField[][] map)
        {
            map = new MapField[_mapSize.x][];
            for (int i = 0; i < _mapSize.x; i++)
            {
                map[i] = new MapField[_mapSize.y];
                for (var j = 0; j < Map[i].Length; j++)
                {
                    map[i][j] = new MapField();
                }
            }
        }

        private void RandomizeTerrainBlocks(ref MapField[][] map, int count)
        {
            var result = new Vector2Int[count];
            List<Vector2Int> emptyFields = new List<Vector2Int>();

            for (int i = 0; i < map.Length; i++)
            for (int j = 0; j < map[i].Length; j++)
                if (map[i][j].Type == MapFieldType.Empty)
                    emptyFields.Add(new Vector2Int(i,j));

            if (emptyFields.Count < count)
                throw new ArgumentException("You are trying to randomize more terrain fields than there are free fields on the map. Try again with proper parameters this time");

            for (int i = 0; i < count; i++)
            {
                int randIndex = GameController.RandomGenerator.Next(0, emptyFields.Count - 1);
                result[i] = emptyFields[randIndex];
                map[result[i].x][result[i].y].Type = MapFieldType.Terrain;
                emptyFields.RemoveAt(randIndex);
            }
        }

        private void RandomizePath(ref MapField[][] map)
        {
            var path = new List<MapField>();
            int pathX = GameController.RandomGenerator.Next(0, _mapSize.x);
            int pathY = 0;

            bool wasUpwardsMove = false;
            bool wasDownwardsMove = false;

            do
            {
                path.Add(map[pathX][pathY]);
                path.Last().Type = MapFieldType.Path;

                if (GameController.RandomGenerator.Next(0, 101) < 66)
                {
                    bool moveUp = (GameController.RandomGenerator.Next(0, 101) < 50);
                    if (moveUp && !wasDownwardsMove && pathX>0)
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

            //for (int i = 0; i < _mapSize.x; i++)
            //{
            //    map[i][pathIndex].Type = MapFieldType.Path;
            //    path.Add(map[i][pathIndex]);
            //}

            Path = path.ToArray();
            //Setting last path tile to be a base
            Path[Path.Length - 1].Type = MapFieldType.Base;
        }

        private void Instantiate(GameObject fieldPrefab, Canvas canvas, GameController gc)
        {
            _fieldsImages = new Image[_mapSize.x][];
            _fieldsObjects = new GameObject[_mapSize.x][];

            var camera = Camera.main;
            var size = canvas.pixelRect.width * 0.5f / _mapSize.y;
            _gridDistance = size;

            Vector3 offset = new Vector3(canvas.pixelRect.width*0.1f, canvas.pixelRect.height* 0.08f);

            for (int i = 0; i < Map.Length; i++)
            {
                _fieldsImages[i] = new Image[_mapSize.x];
                _fieldsObjects[i] = new GameObject[_mapSize.x];

                for (int j = 0; j < Map[i].Length; j++)
                {
                    GenerateGameObjectsForMapFields(i, j, fieldPrefab, canvas, camera, size, offset);
                    AddListeners(gc, i, j);
                }
            }

            Render();
        }

        private void GenerateGameObjectsForMapFields(int i, int j, GameObject fieldPrefab, Canvas canvas, Camera camera, float size, Vector3 offset)
        {
            _fieldsObjects[i][j] = Object.Instantiate(fieldPrefab, canvas.transform);
            _fieldsImages[i][j] = _fieldsObjects[i][j].GetComponent<Image>();

            _fieldsImages[i][j].rectTransform.sizeDelta = Vector2.one * size;
            _fieldsObjects[i][j].transform.localScale = Vector3.one;

            var position = camera.ScreenToWorldPoint(new Vector3(i * _gridDistance, j * _gridDistance) + offset);
            position.z = 0;

            Map[i][j].Position = position;
            _fieldsObjects[i][j].transform.position = position;
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
                _fieldsImages[x][y].color = MapField.FieldColors[Map[x][y].Type];
        }

        public void Render()
        {
            for (int i = 0; i < Map.Length; i++)
            for (int j = 0; j < Map[i].Length; j++)
                RenderTile(i,j);
        }
    }
}

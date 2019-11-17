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
        //map[x][y]
        //map:
        //x0:y0y1y2y3
        //x1:y0y1y2y3
        //x2:y0y1y2y3
        public readonly InteractiveMapField[][] Map;
        public MapField[] Path;

        private readonly (int x, int y) _mapSize = (18, 18);
        private float _gridDistance = 1f;

        public MapGrid(GameObject fieldPrefab, Transform parent, GameController gc, Sprite mapFieldSprite)
        {
            GenerateEmptyMap(ref Map);
            RandomizeTerrainBlocks(ref Map, 30);
            RandomizePath(ref Map);
            Instantiate(fieldPrefab, parent, gc, mapFieldSprite);
        }

        private void GenerateEmptyMap(ref InteractiveMapField[][] map)
        {
            map = new InteractiveMapField[_mapSize.x][];
            for (var i = 0; i < _mapSize.x; i++)
            {
                map[i] = new InteractiveMapField[_mapSize.y];
                for (var j = 0; j < Map[i].Length; j++) map[i][j] = new InteractiveMapField();
            }
        }

        private void RandomizeTerrainBlocks(ref InteractiveMapField[][] map, int count)
        {
            var result = new Vector2Int[count];
            var emptyFields = new List<Vector2Int>();

            for (var i = 0; i < map.Length; i++)
            for (var j = 0; j < map[i].Length; j++)
                if (map[i][j].Field.Type == MapFieldType.Empty)
                    emptyFields.Add(new Vector2Int(i, j));

            if (emptyFields.Count < count)
                throw new ArgumentException(
                    "You are trying to randomize more terrain fields than there are free fields on the map. Try again with proper parameters this time");

            for (var i = 0; i < count; i++)
            {
                var randIndex = GameController.RandomGenerator.Next(0, emptyFields.Count - 1);
                result[i] = emptyFields[randIndex];
                map[result[i].x][result[i].y].Field.Type = MapFieldType.Terrain;
                emptyFields.RemoveAt(randIndex);
            }
        }

        private void RandomizePath(ref InteractiveMapField[][] map)
        {
            var path = new List<InteractiveMapField>();
            var pathX = GameController.RandomGenerator.Next(0, _mapSize.x);
            var pathY = 0;

            var wasUpwardsMove = false;
            var wasDownwardsMove = false;

            do
            {
                path.Add(map[pathX][pathY]);
                path.Last().Field.Type = MapFieldType.Path;

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

            Path = path.Select(field => field.Field).ToArray();
            //Setting last path tile to be a base
            Path[Path.Length - 1].Type = MapFieldType.Base;
        }

        private void Instantiate(GameObject fieldPrefab, Transform parent, GameController gc, Sprite mapFieldSprite)
        {
            var offsetValue = -Map.Length * 0.4725f;
            var offset = new Vector3(offsetValue, offsetValue, 0);
            for (var i = 0; i < Map.Length; i++)
            for (var j = 0; j < Map[i].Length; j++)
            {
                GenerateGameObjectsForMapFields(i, j, fieldPrefab, parent, offset, mapFieldSprite);
                AddListeners(gc, i, j);
            }

            Render();
        }

        private void GenerateGameObjectsForMapFields(int i, int j, GameObject fieldPrefab, Transform parent, Vector3 offset, Sprite mapFieldSprite)
        {
            Map[i][j].GameObject = Object.Instantiate(fieldPrefab, parent);
            Map[i][j].SpriteRenderer = Map[i][j].GameObject.AddComponent<SpriteRenderer>();
            Map[i][j].ClickableObject = Map[i][j].GameObject.AddComponent<ClickableObject>();
            Map[i][j].GameObject.AddComponent<BoxCollider2D>().size = Vector2.one * 0.11f;

            var position = new Vector3(i * _gridDistance, j * _gridDistance) + offset;
            position.z = 0;

            Map[i][j].Field.Position = position;
            Map[i][j].GameObject.transform.localPosition = position;
            Map[i][j].SpriteRenderer.sprite = mapFieldSprite;
        }

        private void AddListeners(GameController gc, int i, int j)
        {
            Map[i][j].Field.OnTypeChanged += () => RenderTile(i, j);
            Map[i][j].ClickableObject.OnClickActions.Push(() =>
            {
                Map[i][j].Field.PlaceSoldier(gc.Hqm, Map[i][j]);
                Render();
            });
        }

        private void RenderTile(int x, int y)
        {
            Map[x][y].SpriteRenderer.color = MapField.FieldColors[Map[x][y].Field.Type];
        }

        public void Render()
        {
            for (var i = 0; i < Map.Length; i++)
            for (var j = 0; j < Map[i].Length; j++)
                RenderTile(i, j);
        }
    }
}
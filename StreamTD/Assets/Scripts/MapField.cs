using System;
using System.Collections.Generic;
using Assets.Scripts.Controllers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class MapField
    {
        public event Action OnTypeChanged = () => { };

        public static Dictionary<MapFieldType, Color> FieldColors = new Dictionary<MapFieldType, Color>()
        {
            {MapFieldType.Empty, Color.white},
            {MapFieldType.Soldier, Color.red},
            {MapFieldType.Path, Color.blue},
            {MapFieldType.Terrain, Color.green},
            {MapFieldType.Base, Color.magenta},
            {MapFieldType.Blocked, Color.cyan}
        };

        private MapFieldType _type = MapFieldType.Empty;
        public MapFieldType Type
        {
            get => _type;

            set
            {
                _type = value;
                OnTypeChanged();
            }
        }

        public Vector2 Position { get; set; }

        public void PlaceSoldier(HQManager hqm, InteractiveMapField field)
        {
            if (GameController.Mode != GameMode.Building) return;
            if (Type != MapFieldType.Empty) return;

            if(hqm.PlaceSoldier(field))
                Type = MapFieldType.Soldier;
        }
    }

    public enum MapFieldType
    {
        Empty = 0,
        Terrain = 1,
        Soldier = 2,
        Path = 3,
        Base = 4,
        Blocked = 5
    }
}
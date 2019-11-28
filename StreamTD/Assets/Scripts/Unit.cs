using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Assets.Scripts.Attacks;
using Assets.Scripts.Controllers;
using Assets.Scripts.Units;
using Assets.Scripts.Units.Enemy;
using Assets.Scripts.Units.Soldier;
using UnityEngine;

namespace Assets.Scripts
{
    public class Unit : Entity
    {
        public const int UnitSize = 4;

        public readonly Soldier[] Soldiers = new Soldier[UnitSize];

        private InteractiveMapField _tile;

        private Vector2 _unitPosition;
        public UnitParameters BaseUP { get; private set; }
        private UnitParameters _UPWithItems;
        public UnitParameters UP
        {
            get => _UPWithItems;
            set
            {
                BaseUP = value; 
                RefreshUPWithItems();
                foreach (Soldier s in Soldiers)
                    s._up = _UPWithItems;
            }
        }

        public List<Action<Soldier,Enemy>> OnHitActions = new List<Action<Soldier, Enemy>>();
        public List<Item> _equipedItems = new List<Item>();

        private const float _unitSplitDistance = 0.2f;
        private static Vector2[] _positionOffsets =
        {
            new Vector2(_unitSplitDistance ,_unitSplitDistance ), 
            new Vector2(_unitSplitDistance ,-_unitSplitDistance ),
            new Vector2(-_unitSplitDistance ,_unitSplitDistance ),
            new Vector2(-_unitSplitDistance ,-_unitSplitDistance ),
            new Vector2(0,0),
        };

        private GameController _gc;

        public Unit(GameController gc, EnemiesController ec, SoldiersController sc, InteractiveMapField field,
            UnitParameters up, DamageType dt, HealthType ht, Sprite soldiersSprite)
        {
            _gc = gc;
            Tile = field;
            _unitPosition = Tile.Field.Position;
            BaseUP = UnitParameters.GetCopy(up);
            _UPWithItems = UnitParameters.GetCopy(up);

            ParseTypes(dt, ht);

            for (var i = 0; i < Soldiers.Length; i++)
            {
                Soldiers[i] = new Soldier(RandomNamesGenerator.GetRandomHumanName(), ec, sc, UP)
                {
                    Position = _unitPosition + _positionOffsets[i]
                };
                var placeholder = new GameObject(Soldiers[i].Name).AddComponent<SpriteRenderer>();
                placeholder.sprite = soldiersSprite;
                placeholder.transform.position = Soldiers[i].Position;
                placeholder.transform.localScale = Vector3.one * 0.4f;
                Soldiers[i].Body = placeholder.gameObject;
            }
        }

        private void ParseTypes(DamageType dt, HealthType ht)
        {
            switch (ht)
            {
                case HealthType.Armor:
                    UP.Armor = UP.Health;
                    break;
                case HealthType.EnergyShields:
                    UP.EnergyShields = UP.Health;
                    break;
                case HealthType.EMF:
                    UP.EMF = UP.Health;
                    break;
            }

            switch (dt)
            {
                case DamageType.Plasma:
                    UP.PlasmaDamage = Soldier.DefaultDamage * 2;
                    UP.AttacksPerSecond /= 2;
                    break;
                case DamageType.Ballistic:
                    UP.BallisticDamage = Soldier.DefaultDamage / 2;
                    UP.AttacksPerSecond *= 2;
                    break;
                case DamageType.Laser:
                    UP.LaserDamage = Soldier.DefaultDamage;
                    break;
            }
        }

        public void AddItem(Item item)
        {
            _equipedItems.Add(item);
            RefreshUPWithItems();
        }

        public void RemoveItem(Item item)
        {
            _equipedItems.Remove(item);
            RefreshUPWithItems();
        }

        private void RefreshUPWithItems()
        {
            _UPWithItems = UnitParameters.GetCopy(BaseUP);
            foreach (var equiped in _equipedItems)
            {
                _UPWithItems = ((EquipmentItem)equiped).Apply(_UPWithItems);
            }
        }

        public InteractiveMapField Tile
        {
            get => _tile;
            set
            {
                _tile = value;
                if (_tile == null) return;
                Move(_tile.Field.Position);
            }
        }

        public void RemoveUnit()
        {
            foreach (Soldier soldier in Soldiers)
            {
                soldier.Die();
            }

            foreach (Item item in _equipedItems)
            {
                _gc.UIController.EQCanvasController.AddNewItem(item, _gc);
            }

            _tile.Field.Type = MapFieldType.Empty;
            _tile.ClickableObject.OnClickActions.Pop();

            _gc.EconomyController.Quants += HQUIManager.UnitPrice / 2;
        }
    }
}
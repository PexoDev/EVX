using System;
using Assets.Scripts.Attacks;
using Assets.Scripts.Controllers;
using Assets.Scripts.Units;
using Assets.Scripts.Units.Enemy;
using Assets.Scripts.Units.Soldier;
using Boo.Lang;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts
{
    public class Unit : LivingEntity
    {
        public const int UnitSize = 4;

        public readonly Soldier[] Soldiers = new Soldier[UnitSize];

        private InteractiveMapField _tile;

        private Vector2 _unitPosition;
        public UnitParameters UP { get; private set; }

        public List<Action<Soldier,Enemy>> OnHitActions = new List<Action<Soldier, Enemy>>();

        public Action<UnitParameters> ChangeParameters;

        private const float _unitSplitDistance = 0.2f;
        private static Vector2[] _positionOffsets =
        {
            new Vector2(_unitSplitDistance ,_unitSplitDistance ), 
            new Vector2(_unitSplitDistance ,-_unitSplitDistance ),
            new Vector2(-_unitSplitDistance ,_unitSplitDistance ),
            new Vector2(-_unitSplitDistance ,-_unitSplitDistance ),
            new Vector2(0,0),
        };

        public Unit(EnemiesController ec, SoldiersController sc, InteractiveMapField field,
            UnitParameters up, DamageType dt, HealthType ht, Sprite soldiersSprite) : base(up)
        {
            Tile = field;
            _unitPosition = Tile.Field.Position;
            UP = UnitParameters.GetCopy(up);

            ChangeParameters = parameters =>
            {
                UP = parameters;
                UP.ValueChanged();
                foreach (Soldier soldier in Soldiers)
                {
                    soldier._up = UP;
                }
            };

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

        public override void AnimateHurt()
        {
        }
    }
}
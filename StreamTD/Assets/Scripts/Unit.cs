using System;
using Assets.Scripts.Attacks;
using Assets.Scripts.Controllers;
using Assets.Scripts.Units;
using Assets.Scripts.Units.Enemy;
using Assets.Scripts.Units.Soldier;
using Boo.Lang;
using UnityEngine;

namespace Assets.Scripts
{
    public class Unit : LivingEntity
    {
        public const int UnitSize = 5;

        public readonly Soldier[] Soldiers = new Soldier[UnitSize];

        private InteractiveMapField _tile;

        private Vector2 _unitPosition;
        public UnitParameters UP { get; private set; }

        public List<Action<Soldier,Enemy>> OnHitActions = new List<Action<Soldier, Enemy>>();

        public Action<UnitParameters> ChangeParameters;

        private static Vector2[] _positionOffsets =
        {
            new Vector2(0,0),
            new Vector2(0.33f,.33f), 
            new Vector2(.33f,-.33f),
            new Vector2(-.33f,.33f),
            new Vector2(-.33f,-.33f),
        };

        public Unit(EnemiesController ec, SoldiersController sc, InteractiveMapField field,
            UnitParameters up, DamageType dt, HealthType ht) : base(up)
        {
            Tile = field;
            _unitPosition = Tile.Field.Position;
            UP = up;

            ChangeParameters = parameters =>
            {
                UP = parameters;
                UP.ValueChanged();
                foreach (Soldier soldier in Soldiers)
                {
                    soldier._up = UP;
                }
            };

            for (var i = 0; i < Soldiers.Length; i++)
            {
                Soldiers[i] = new Soldier(RandomNamesGenerator.GetRandomHumanName(), ec, sc, dt, ht, UP);
                Soldiers[i].Position = _unitPosition + _positionOffsets[i];
                var placeholder = GameObject.CreatePrimitive(PrimitiveType.Cube);
                placeholder.transform.position = Soldiers[i].Position;
                placeholder.transform.localScale = Vector3.one * 0.25f;
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
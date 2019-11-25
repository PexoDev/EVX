using Assets.Scripts.Attacks;
using Assets.Scripts.Controllers;
using Assets.Scripts.Units.Soldier;
using UnityEngine;

namespace Assets.Scripts.Units.Enemy
{
    public abstract class Enemy: AttackingEntity<Enemy, Soldier.Soldier>
    {
        public EnemyType Type;
        public int ScoreValue { get; set; } = 1;
        protected bool BaseInRange;
        public MapField[] PathToTraverse { get; set; }
        private PlayerBase _targetBase;

        public bool Frozen { get; set; }
        private UnitParameters _up;

        protected Enemy(MapField[] path, SoldiersController sc, EnemiesController ec, PlayerBase targetBase, DamageType dt, HealthType ht, UnitParameters up, int defaultDamage) : base(sc, ec, up)
        {
            PathToTraverse = path;
            _targetBase = targetBase;
            _up = up;

            switch (ht)
            {
                case HealthType.Armor:
                    _up.Armor = up.Health;
                    break;
                case HealthType.EnergyShields:
                    _up.EnergyShields = up.Health;
                    break;
                case HealthType.EMF:
                    _up.EMF = up.Health;
                    break;
            }

            switch (dt)
            {
                case DamageType.Plasma:
                    _up.PlasmaDamage = defaultDamage * 2;
                    break;
                case DamageType.Ballistic:
                    _up.BallisticDamage = Mathf.CeilToInt(defaultDamage / 2);
                    break;
                case DamageType.Laser:
                    _up.LaserDamage = defaultDamage;
                    break;
            }

            CurrentMovementTarget = PathToTraverse[0].Position;
        }

        public int CurrentFieldIndex { get; set; }
        public Vector2 CurrentMovementTarget;
        public void DoStep()
        {
            if (Frozen ) return;
            if (BaseInRange) return;

            var jitterVector = new Vector2(GameController.RandomGenerator.Next(-33, 33), GameController.RandomGenerator.Next(-33, 33)) * 0.01f;

            if (IsInRange(CurrentMovementTarget, 0.33f))
            {
                CurrentFieldIndex++;
                CurrentMovementTarget = PathToTraverse[CurrentFieldIndex].Position + jitterVector;
            }

            if (CurrentFieldIndex == PathToTraverse.Length - 1)
            {
                BaseInRange = true;
                return;
            }

            if(PathToTraverse[CurrentFieldIndex].Type != MapFieldType.Path) return;
            var newPosition = Vector2.MoveTowards(Position, CurrentMovementTarget, _up.MovementSpeed);
            Move(newPosition);
        }

        public override void AutoAttack()
        {
            if(Frozen) return;

            if (BaseInRange)
            {
                AttackBase();
            }
            else
                base.AutoAttack();
        }

        private void AttackBase()
        {
            Attack(_targetBase);
        }

        public override void Die()
        {
            base.Die();
            Controller.Gc.EconomyController.Quants += ScoreValue;
            Controller.RemoveInstance(this);
        }
    }
}

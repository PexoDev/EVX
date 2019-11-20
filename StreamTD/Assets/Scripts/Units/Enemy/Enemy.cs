using Assets.Scripts.Attacks;
using Assets.Scripts.Units.Soldier;
using UnityEngine;

namespace Assets.Scripts.Units.Enemy
{
    public abstract class Enemy: AttackingEntity<Enemy, Soldier.Soldier>
    {
        public int ScoreValue { get;} = 150;
        protected bool BaseInRange;
        public MapField[] PathToTraverse { get; set; }
        private PlayerBase _targetBase;

        public bool Frozen { get; set; }
        private UnitParameters _up;

        protected Enemy(MapField[] path, SoldiersController sc, EnemiesController ec, PlayerBase targetBase, DamageType dt, HealthType ht, UnitParameters up, Sprite sprite) : base(sc, ec, up)
        {
            PathToTraverse = path;
            _targetBase = targetBase;
            _up = up;
        }

        public int CurrentFieldIndex { get; set; }

        public void DoStep()
        {
            if (Frozen ) return;
            if (BaseInRange) return;

            if(IsInRange(PathToTraverse[CurrentFieldIndex].Position,0.1f))
                CurrentFieldIndex++;

            if (CurrentFieldIndex == PathToTraverse.Length - 1)
            {
                BaseInRange = true;
                return;
            }

            if(PathToTraverse[CurrentFieldIndex].Type != MapFieldType.Path) return;
            var targetPosition = PathToTraverse[CurrentFieldIndex].Position;
            var newPosition = Vector2.MoveTowards(Position, targetPosition, _up.MovementSpeed);
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
            Controller.RemoveInstance(this);
        }
    }
}

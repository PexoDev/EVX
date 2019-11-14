using Assets.Scripts.Attacks;
using Assets.Scripts.Traits;
using Assets.Scripts.Units.Soldier;
using UnityEngine;

namespace Assets.Scripts.Units.Enemy
{
    public abstract class Enemy: AttackingEntity<Enemy, Soldier.Soldier>
    {
        private GameObject _body;
        public int ScoreValue { get;} = 150;
        protected bool BaseInRange;
        public MapField[] PathToTraverse { get; set; }
        private PlayerBase _targetBase;

        public bool Frozen { get; set; }
        public override void GetHit(IAttack attack)
        {
            base.GetHit(attack);
            if (_body == null || MaxHp <= 0 || HP <= 0) return;
                _body.transform.localScale = Vector3.one * HP / MaxHp;
        }

        protected Enemy(MapField[] path, SoldiersController sc, EnemiesController ec, PlayerBase targetBase, DamageType dt, HealthType ht, UnitParameters up) : base(sc, ec, dt, ht, up)
        {
            _body = Object.Instantiate(EnemiesController.EnemyPrefab, Vector3.one * 1000, Quaternion.identity, EnemiesController.ParentCanvas.transform);
            _body.transform.localScale = Vector3.one;
            PathToTraverse = path;
            _targetBase = targetBase;
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
            var newPosition = Vector2.MoveTowards(Position, PathToTraverse[CurrentFieldIndex].Position, UnitParams.MovementSpeed ?? 0);
            Move(newPosition);
            _body.transform.position = Position;
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
            Object.Destroy(_body);
        }
    }
}

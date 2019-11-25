using System;
using Assets.Scripts.Attacks;
using Assets.Scripts.Units.Enemy;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Units.Soldier
{
    public class Soldier: AttackingEntity<Soldier, Enemy.Enemy>
    {
        public GameObject Body;
        public static int DefaultDamage = 5;
        public static UnitParameters DefaultParams = new UnitParameters
        {
            AttacksPerSecond = 2f,
            Health = 200,
            AttackRange = 4,
            ClipSize = 20,
            AttackAccuracy = 0.9f,
            CriticalChance = 0.05f,
            ReloadTime = 1
        };

        public string Name { get; }

        public int KillCount { get; set; }

        public Soldier(string name, EnemiesController ec, SoldiersController sc, UnitParameters up) : base(ec, sc, up)
        {
            Name = name;

            _up.ValueChanged();
        }

        public override void AutoAttack()
        {
            base.AutoAttack();
            if(CurrentTarget!= null && Body != null)
                Body.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(CurrentTarget.Position.y - Position.y, CurrentTarget.Position.x - Position.x) * Mathf.Rad2Deg - 90f);
        }

        public override string ToString()
        {
            return Name;
        }

        public override void Die()
        {
            base.Die();
            Controller.RemoveInstance(this);
            Object.Destroy(Body);
        }

        public override void AnimateHurt()
        {

        }
    }
}
using Assets.Scripts.Attacks;
using Assets.Scripts.Units.Soldier;
using UnityEngine;

namespace Assets.Scripts.Units.Enemy
{
    public class DummyEnemy: Enemy
    {
        public DummyEnemy(MapField[] path, SoldiersController sc, EnemiesController ec, PlayerBase pb, DamageType dt,
            HealthType ht, UnitParameters up, int defaultDamage, Sprite sprite) : base(path, sc, ec, pb, dt, ht, up, defaultDamage, sprite)
        {

        }

        public override void AutoAttack()
        {
            base.AutoAttack();
            //if (BaseInRange)
            //    base.AutoAttack();
        }

        public override void AnimateHurt()
        {
        }
    }
}
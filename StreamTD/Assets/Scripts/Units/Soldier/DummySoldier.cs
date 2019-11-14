using Assets.Scripts.Attacks;
using Assets.Scripts.Units.Enemy;

namespace Assets.Scripts.Units.Soldier
{
    public class DummySoldier: Soldier
    {
        public DummySoldier(string name, MapField field, EnemiesController ec, SoldiersController sc, DamageType dt, HealthType ht, UnitParameters up) : base(name, field, ec, sc, dt,ht, up)
        {
        }

        public void AttackRoutine()
        {
            AutoAttack();
        }

    }
}
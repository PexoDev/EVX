using Assets.Scripts.Attacks;
using Assets.Scripts.Units.Enemy;

namespace Assets.Scripts.Units.Soldier
{
    public class Soldier: AttackingEntity<Soldier, Enemy.Enemy>
    {
        public static int DefaultDamage = 0;
        public static UnitParameters DefaultParams = new UnitParameters
        {
            AttacksPerSecond = 4f,
            Health = 100,
            AttackRange = 3,
            ClipSize = 20,
            AttackAccuracy = 0.9f,
            CriticalChance = 0.05f
        };

        public string Name { get; }

        public int KillCount { get; set; }

        public Soldier(string name, EnemiesController ec, SoldiersController sc, DamageType dt, HealthType ht, UnitParameters up) : base(ec, sc, up)
        {
            Name = name;

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
                    _up.PlasmaDamage = DefaultDamage * 2;
                    _up.AttacksPerSecond /= 2;
                    break;
                case DamageType.Ballistic:
                    _up.BallisticDamage = DefaultDamage / 2;
                    _up.AttacksPerSecond *= 2;
                    break;
                case DamageType.Laser:
                    _up.LaserDamage = DefaultDamage;
                    break;
            }

            _up.ValueChanged();
        }

        public override string ToString()
        {
            return Name;
        }

        public override void Die()
        {
            base.Die();
            Controller.RemoveInstance(this);
        }

        public override void AnimateHurt()
        {

        }
    }
}
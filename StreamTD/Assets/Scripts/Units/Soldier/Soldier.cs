using System;
using System.Collections.Generic;
using Assets.Scripts.Attacks;
using Assets.Scripts.Units.Enemy;

namespace Assets.Scripts.Units.Soldier
{
    public class Soldier: AttackingEntity<Soldier, Enemy.Enemy>
    {
        public static UnitParameters DefaultParams = new UnitParameters { AttacksPerSecond = 3f, Health = 250, AttackRange = 3};
        public static int DefaultDamage = 0;

        public string Name { get; }

        public int KillCount { get; set; }

        public Soldier(string name, EnemiesController ec, SoldiersController sc, DamageType dt, HealthType ht, UnitParameters up) : base(ec, sc,up)
        {
            Name = name;

            switch (ht)
            {
                case HealthType.Armor:
                    up.Armor = up.Health / 2;
                    break;
                case HealthType.EnergyShields:
                    up.EnergyShields = up.Health / 2;
                    break;
                case HealthType.EMF:
                    up.EMF = up.Health / 2;
                    break;
            }

            switch (dt)
            {
                case DamageType.Plasma:
                    up.PlasmaDamage = DefaultDamage;
                    break;
                case DamageType.Ballistic:
                    up.BallisticDamage = DefaultDamage;
                    break;
                case DamageType.Laser:
                    up.LaserDamage = DefaultDamage;
                    break;
            }
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
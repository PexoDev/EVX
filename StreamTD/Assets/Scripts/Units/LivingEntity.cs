using System;
using Assets.Scripts.Attacks;
using Assets.Scripts.Controllers;

namespace Assets.Scripts.Units
{
    public abstract class LivingEntity : Entity
    {
        private readonly string _cooldownKey = Guid.NewGuid().ToString();

        private readonly UnitParameters _up;

        protected int Hp;

        protected LivingEntity(UnitParameters up)
        {
            _up = up;
            Hp = up.Health ?? 1;
            _up.OnValueChanged += newUp => UpdateParams();
        }

        public bool Targetable { get; set; } = true;
        public bool Alive { get; private set; } = true;

        public int MaxHp => _up.Health ?? 1;

        public Health Armor { get; set; } = new Health {Type = HealthType.Armor};
        public Health EMF { get; set; } = new Health {Type = HealthType.EMF};
        public Health EnergyShields { get; set; } = new Health {Type = HealthType.EnergyShields};

        protected int HealingPerSecond => _up.RegenerationPerSecond ?? 0;
        protected float DeflectionChance => _up.DeflectionChance ?? 0;
        protected float EvasionChance => _up.EvasionChance ?? 0;

        public virtual int HP
        {
            get => Hp;
            set
            {
                if (!Alive) return;

                Hp = value;
                if (value > MaxHp) HP = MaxHp;
                if (Hp <= 0) Die();
            }
        }

        private void UpdateParams()
        {
            bool fill = EMF.MaxValue == 0;
            EMF.MaxValue = _up.EMF ?? 0;
            if (fill) EMF.Value = EMF.MaxValue;
            EMF.Threshold = _up.BallisticThreshold ?? 0;
            EMF.Resistance = _up.BallisticResistance ?? 0;

            fill = Armor.MaxValue == 0;
            Armor.MaxValue = _up.Armor ?? 0;
            if (fill) Armor.Value = Armor.MaxValue;
            Armor.Threshold = _up.PlasmaThreshold ?? 0;
            Armor.Resistance = _up.PlasmaResistance ?? 0;

            fill = EnergyShields.MaxValue == 0;
            EnergyShields.MaxValue = _up.EnergyShields ?? 0;
            if (fill) EnergyShields.Value = EnergyShields.MaxValue;
            EnergyShields.Threshold = _up.LaserThreshold ?? 0;
            EnergyShields.Resistance = _up.LaserResistance ?? 0;
        }

        public virtual void GetHit(IAttack attack, LivingEntity attacker)
        {
            if (GameController.RandomGenerator.Next(0, 101) * 0.01f < DeflectionChance)
                //DeflectAttack(attack);
                return;

            if (GameController.RandomGenerator.Next(0, 101) * 0.01f < EvasionChance)
            {
                EvadeAttack();
                return;
            }

            //Damages in order EMF -> ES -> Armor -> HP
            var afterEMF = DamageModule(EMF, attack);
            var afterES = DamageModule(EnergyShields, afterEMF);
            var afterArm = DamageModule(Armor, afterES);

            if(afterArm.OverallDamage <= 0) return;

            HP -= afterArm.OverallDamage;
            AnimateHurt();
        }

        private Attack DamageModule(Health hp, IAttack attack)
        {
            //Damages in order: Plasma -> Laser -> Ballistic
            var plasma = hp.Hit(attack.PlasmaDamage, DamageType.Plasma);
            var laser = hp.Hit(attack.LaserDamage, DamageType.Laser);
            var ballistic = hp.Hit(attack.BallisticDamage, DamageType.Ballistic);
            var @default = hp.Hit(attack.DefaultDamage, DamageType.Default);

            return new Attack
            {
                BallisticDamage = ballistic,
                DefaultDamage = @default,
                PlasmaDamage = plasma,
                LaserDamage = laser
            };
        }

        public virtual void Regenerate()
        {
            if (CooldownController.GetCooldown(_cooldownKey, 1f))
                //regenerates in order HP -> Armor -> ES -> EMF
            {
                var healValue = HealingPerSecond;
                healValue -= MaxHp - Hp;
                Hp += HealingPerSecond;
                if (healValue <= 0) return;
                
                healValue = Armor.Regenerate(healValue);
                healValue = EnergyShields.Regenerate(healValue);
                EMF.Regenerate(healValue);
            }
        }

        private void EvadeAttack()
        {

        }

        private void DeflectAttack(Projectile attack)
        {
            //ProjectilesController.Instance.InitializeProjectile(new Projectile(attack.Damage, attack.DamageType, Position, this, attack.Attacker,
            //    () => attack.Attacker.GetHit(new Projectile(attack.Damage, attack.DamageType) { Attacker = this })));
        }

        public virtual void Die()
        {
            Alive = false;
        }

        public abstract void AnimateHurt();
    }

    public class Health
    {
        public HealthType Type { get; set; }
        private int _value;
        public int Value
        {
            get => _value;
            set
            {
                _value = value;
                if (_value > MaxValue) _value = MaxValue;
            }
        }
        public int MaxValue { get; set; }
        public float Resistance { get; set; }
        public int Threshold { get; set; }

        public int Hit(int damage, DamageType dt)
        {
            var calcuatedDmg = Damage.CalculateDamage(damage, dt, Type);
            damage = (int)(damage * (1f - Resistance));
            damage -= Threshold;
            if (damage <= 0) return 0;
            var returnDmg = damage - Value;
            Value -= damage;
            return returnDmg;
        }

        public int Regenerate(int value)
        {
            int returnValue = value - (MaxValue - Value);
            Value += value;
            return returnValue > 0 ? returnValue : 0;
        }
    }
}
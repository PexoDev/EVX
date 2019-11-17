using System;
using Assets.Scripts.Attacks;
using Assets.Scripts.Controllers;
using UnityEngine;

namespace Assets.Scripts.Units
{
    public abstract class LivingEntity: Entity
    {
        public bool Targetable { get; set; } = true;
        public bool Alive { get; private set; } = true;

        public HealthType HealthType { get; protected set; }

        public int MaxHp { get; protected set; } = 1;
        protected int Hp = 1;
        public virtual int HP
        {
            get => Hp;
            set
            {
                Hp = value;
                if (value > MaxHp) HP = MaxHp;
                if (Hp <= 0) Die();
            }
        }

        protected int HealingPerSecond;
        protected int DamageThreshold;
        protected float DamageResistance;
        protected float DeflectionChance;
        protected float EvasionChance = 0.05f;

        private readonly string _cooldownKey = Guid.NewGuid().ToString();

        public virtual void GetHit(IAttack attack)
        {
            int damage = attack.Damage;

            if (attack.DamageType != DamageType.Default)
            {
                if (GameController.RandomGenerator.Next(0, 101) * 0.01f < DeflectionChance)
                {
                    DeflectAttack();
                    return;
                }

                if (GameController.RandomGenerator.Next(0, 101) * 0.01f < EvasionChance)
                {
                    EvadeAttack();
                    return;
                }

                damage = Damage.CalculateDamage(attack, HealthType);
                damage = (int) (damage * (1f - DamageResistance));
                damage -= DamageThreshold;
                if (damage <= 0) return;
            }

            HP -= damage;
            AnimateHurt();
        }

        public virtual void Regenerate()
        {
            if (CooldownController.GetCooldown(_cooldownKey, 1f))
                HP += HealingPerSecond;
        }

        private void EvadeAttack()
        {
        }

        private void DeflectAttack()
        {
            Debug.Log("Deflected attack");
        }

        public virtual void Die()
        {
            Alive = false;
        }

        public abstract void AnimateHurt();
    }
}

using System;
using Assets.Scripts.Units;
using UnityEngine;

namespace Assets.Scripts.Attacks
{
    public class Projectile : Entity, IAttack
    {
        public Action HitMethod { get; set; }
        public DamageType DamageType { get; }
        public int Damage { get; }
        public LivingEntity Target;

        public Projectile(int damageValue, DamageType type)
        {
            Damage = damageValue;
            DamageType = type;
            Speed = 5;
        }

        public Projectile(int damageValue, DamageType type, Vector2 position, LivingEntity target, Action hitMethod) :
            this(damageValue, type)
        {
            Move(position);
            Target = target;
            HitMethod = hitMethod;
        }


        public void Move()
        {
            Move(Vector2.MoveTowards(Position, Target.Position, Speed * Time.fixedDeltaTime));
            Rotation = new Vector3(0, 0, Mathf.Atan2(Target.Position.y - Position.y, Target.Position.x - Position.x) * Mathf.Rad2Deg);
            if (IsInRange(Target.Position, 0.01f))
                DisposeProjectile();
        }

        private void DisposeProjectile()
        {
            HitMethod();
        }
    }
}
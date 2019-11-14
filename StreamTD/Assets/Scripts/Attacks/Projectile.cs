using System;
using Assets.Scripts.Units;
using UnityEngine;

namespace Assets.Scripts.Attacks
{
    public class Projectile : Entity, IAttack
    {
        public readonly Action HitMethod;

        private GameObject _body;
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
            Render();
        }

        public GameObject Body
        {
            get => _body;
            set
            {
                _body = value;
                _body.transform.position = Position;
                _body.SetActive(true);
            }
        }

        public DamageType DamageType { get; }
        public int Damage { get; }


        public void Act()
        {
            Move();
            Render();
        }

        private void Render()
        {
            if (Body == null) return;
            Body.transform.position = Position;
        }

        private void Move()
        {
            Move(Vector2.MoveTowards(Position, Target.Position, Speed * Time.fixedDeltaTime));
            if (IsInRange(Target.Position, 0.01f))
                DisposeProjectile();
        }

        private void DisposeProjectile()
        {
            HitMethod();
        }
    }
}
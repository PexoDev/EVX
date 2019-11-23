using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Attacks;
using Assets.Scripts.Controllers;
using UnityEngine;

namespace Assets.Scripts.Units
{
    public abstract class AttackingEntity<TEntityType, TTargetType> : LivingEntity
        where TTargetType : LivingEntity
        where TEntityType : LivingEntity
    {
        public List<Action<AttackingEntity<TEntityType, TTargetType>, TTargetType>> OnHitEffects = new List<Action<AttackingEntity<TEntityType, TTargetType>, TTargetType>>();

        public EntitiesController<TEntityType> Controller { get; set; }
        public EntitiesController<TTargetType> TargetsController { get; set; }

        protected float CriticalChance => _up.CriticalChance;
        protected float CriticalMultiplier => _up.CriticalMultiplier;
        protected int AttackRange => _up.AttackRange;
        private int _bulletsCount;

        protected Attack DefaultAttack => new Attack
        {
            BallisticDamage = _up.BallisticDamage,
            LaserDamage = _up.LaserDamage,
            PlasmaDamage = _up.PlasmaDamage
        };

        private readonly string _cooldownKey = Guid.NewGuid().ToString();

        public TTargetType CurrentTarget { get; protected set; }

        protected AttackingEntity(EntitiesController<TTargetType> targetsController,
            EntitiesController<TEntityType> entitiesController, UnitParameters up) : base(up)
        {
            TargetsController = targetsController;
            Controller = entitiesController;

            _bulletsCount = _up.ClipSize;
        }

        public virtual void AutoAttack()
        {
            Attack(LookForTarget());
        }

        private bool _isReloading;
        private float _reloadingTime = 0;
        protected void Attack(TTargetType target)
        {
            if (_reloadingTime > 0)
            {
                _reloadingTime -= Time.deltaTime;
                if (_reloadingTime <= 0)
                {
                    _bulletsCount = _up.ClipSize;
                }
                return;
            }

            if (_up.AttacksPerSecond <= 0.01f) return;
            if (target == null) return;
            if (!CooldownController.GetCooldown(_cooldownKey, 1 / _up.AttacksPerSecond)) return;

            Attack attack = DefaultAttack;
            if (GameController.RandomGenerator.Next(0, 101) * 0.01f < CriticalChance)
                attack *= CriticalMultiplier;

            void FinalHitAction()
            {
                if (!(GameController.RandomGenerator.Next(0, 101) * 0.01f < _up.AttackAccuracy))
                {
                    return;
                };
                target.GetHit(attack, this);
                foreach (var action in OnHitEffects.ToArray())
                    action?.Invoke(this, target);
            }

            ProjectilesController.Instance.InitializeProjectile(attack, this, target, FinalHitAction);
            _bulletsCount--;
            if (_bulletsCount <= 0)
            {
                _reloadingTime = _up.ReloadTime;
            }
        }


        public virtual TTargetType LookForTarget()
        {
            if (CurrentTarget != null)
                if (CurrentTarget.Alive && CurrentTarget.IsInRange(Position, AttackRange))
                    return CurrentTarget;
                else
                    CurrentTarget = null;

            var enemiesInRange = TargetsController.Entities.Where(enemy => enemy.Targetable && enemy.IsInRange(Position, AttackRange)).ToArray();
            if(enemiesInRange.Length>0) CurrentTarget = enemiesInRange[GameController.RandomGenerator.Next(0, enemiesInRange.Length)];

            return CurrentTarget;
        }
    }
}
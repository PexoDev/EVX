using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Attacks;
using Assets.Scripts.Controllers;

namespace Assets.Scripts.Units
{
    public abstract class AttackingEntity<TEntityType, TTargetType> : LivingEntity
        where TTargetType : LivingEntity
        where TEntityType : LivingEntity
    {
        public List<Action<TTargetType>> OnHitEffects = new List<Action<TTargetType>>();

        public EntitiesController<TEntityType> Controller { get; set; }
        public EntitiesController<TTargetType> TargetsController { get; set; }

        protected float CriticalChance => _up.CriticalChance ?? 0;
        protected float CriticalMultiplier => _up.CriticalMultiplier ?? 0;
        protected int AttackRange => _up.AttackRange ?? 0;

        protected Attack DefaultAttack => new Attack
        {
            BallisticDamage = _up.BallisticDamage ?? 0, LaserDamage = _up.LaserDamage ?? 0,
            PlasmaDamage = _up.PlasmaDamage ?? 0
        };

        private readonly string _cooldownKey = Guid.NewGuid().ToString();

        public TTargetType CurrentTarget { get; protected set; }

        protected AttackingEntity(EntitiesController<TTargetType> targetsController,
            EntitiesController<TEntityType> entitiesController, UnitParameters up) : base(up)
        {
            TargetsController = targetsController;
            Controller = entitiesController;

            _up = up;
        }

        private readonly UnitParameters _up;

        public virtual void AutoAttack()
        {
            Attack(LookForTarget());
        }

        protected void Attack(TTargetType target)
        {
            if ((_up.AttacksPerSecond ?? -1) <= 0) return;
            if (target == null) return;
            if (!CooldownController.GetCooldown(_cooldownKey, 1 / (float)_up.AttacksPerSecond)) return;

            Attack attack = DefaultAttack;
            if (GameController.RandomGenerator.Next(0, 101) * 0.01f < CriticalChance)
                attack *= CriticalMultiplier;

            void FinalHitAction()
            {
                target.GetHit(attack, this);
                foreach (Action<TTargetType> action in OnHitEffects.ToArray())
                    action?.Invoke(target);
            }

            ProjectilesController.Instance.InitializeProjectile(attack, this, target, FinalHitAction);
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
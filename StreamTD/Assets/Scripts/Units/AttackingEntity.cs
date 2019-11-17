using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Attacks;
using Assets.Scripts.Controllers;
using Assets.Scripts.Traits;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Units
{
    public abstract class AttackingEntity<TEntityType, TTargetType> : LivingEntity
        where TTargetType : LivingEntity
        where TEntityType : LivingEntity
    {
        public EntitiesController<TEntityType> Controller { get; set; }
        public EntitiesController<TTargetType> TargetsController { get; set; }
        public DamageType DamageType { get; protected set; }
        protected float CriticalChance => UnitParams.CriticalMultiplier ?? 0;
        protected float CriticalMultiplier => UnitParams.CriticalMultiplier ?? 0;
        protected int AttackRange => UnitParams.AttackRange ?? 0;
        protected int Damage => UnitParams.Damage ?? 0;

        public SpecialAttacksModule SpecialAttacksModule { get; set; }
        public Debuff CCDebuff { get; set; }
        public Debuff DOTDebuff { get; set; }

        private readonly string _cooldownKey = Guid.NewGuid().ToString();

        public TTargetType CurrentTarget { get; protected set; }

        protected AttackingEntity(EntitiesController<TTargetType> targetsController,
            EntitiesController<TEntityType> entitiesController, DamageType damage, HealthType health, UnitParameters up)
        {
            ApplyNewParams(up);
            DamageType = damage;
            HealthType = health;

            TargetsController = targetsController;
            Controller = entitiesController;

            var unitParameters = UnitParams;
            SpecialAttacksModule = new SpecialAttacksModule(unitParameters);
        }

        private UnitParameters _unitParams = new UnitParameters();

        protected AttackingEntity()
        { }

        public UnitParameters UnitParams
        {
            get
            {
                if (CCDebuff == null) return _unitParams;
                return UnitParameters.CopyOrFillWithDefault(CCDebuff.GetNegativeEffects(_unitParams));
            }
            set => ApplyNewParams(value);
        }

        protected void ApplyNewParams(UnitParameters newParams)
        {
            _unitParams = UnitParameters.CopyOrFillWithDefault(newParams);
            _unitParams.Health = _unitParams.MaxHealth;
            MaxHp = _unitParams.MaxHealth ?? 0;
            HP = _unitParams.Health ?? 0;
            if(SpecialAttacksModule != null) SpecialAttacksModule.Parameters = _unitParams;
        }

        public virtual void AutoAttack()
        {
            Attack(LookForTarget());
        }

        protected void Attack(TTargetType target)
        {
            if (UnitParams.AttacksPerSecond == null || UnitParams.AttacksPerSecond <= 0) return;
            if (target == null) return;
            if (!CooldownController.GetCooldown(_cooldownKey, 1 / (float)UnitParams.AttacksPerSecond)) return;

            if(target is AttackingEntity<TTargetType, TEntityType> attackingTarget) SpecialAttacksModule.ApplyEffects(attackingTarget);

            int attackDamage = UnitParams.Damage ?? 0;
            if (GameController.RandomGenerator.Next(0, 100) * 0.01f < CriticalChance)
            {
                attackDamage = (int) (UnitParams.Damage ?? 0 * CriticalMultiplier);
                Debug.Log("Critted!");
            }

            ProjectilesController.Instance.InitializeProjectile(new Projectile(attackDamage, DamageType, Position, target,
                () => target.GetHit(new Projectile(Damage, DamageType))), null);
        }

        public virtual TTargetType LookForTarget()
        {
            if (CurrentTarget != null)
                if (CurrentTarget.Alive && CurrentTarget.IsInRange(Position, AttackRange))
                    return CurrentTarget;
                else
                    CurrentTarget = null;

            CurrentTarget = TargetsController.Entities.FirstOrDefault(enemy => enemy.Targetable && enemy.IsInRange(Position, AttackRange));

            return CurrentTarget;
        }
    }
}
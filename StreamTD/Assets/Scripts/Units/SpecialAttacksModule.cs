using System;
using System.Reflection;
using Assets.Scripts.Attacks;
using Assets.Scripts.Controllers;
using Assets.Scripts.Traits;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Units
{
    public class SpecialAttacksModule
    {
        public UnitParameters Parameters { get; set; }
        public Action SpecialSkill { get; set; }

        public int MaxEnergy => Parameters.MaxEnergy ?? 0;
        private int _energy;

        public int Energy
        {
            get => _energy;
            set
            {
                _energy = value;
                if (_energy > MaxEnergy)
                    _energy = MaxEnergy;
                if (_energy < 0)
                    _energy = 0;
            }
        }

        public int EnergyGainPerSecond => Parameters.EnergyGainPerSecond ?? 0;
        public float SpecialEffectChance => Parameters.SpecialEffectChance ?? 0;
        public int SpecialEffectDamagePerTick => Parameters.SpecialEffectDamagePerTick ?? 0;
        public float SpecialEffectSlow => Parameters.SpecialEffectSlow ?? 0;
        public float SpecialEffectDuration => Parameters.SpecialEffectDuration ?? 0;
        public float SpecialEffectAttackSpeedReduction => Parameters.SpecialEffectAttackSpeedReduction ?? 0;
        public float SpecialEffectDamageIntakeIncreased => Parameters.SpecialEffectDamageIntakeIncreased ?? 0;

        public bool Active { get; set; }
        public Debuff Debuff => Active ? new Debuff(this) : null;

        public SpecialAttacksModule(UnitParameters parameters)
        {
            Parameters = parameters;
        }

        public void ApplyEffects<TEntityType, TTargetType>(AttackingEntity<TEntityType, TTargetType> target)
        where TEntityType : LivingEntity
        where TTargetType : LivingEntity
        {
            if (Energy >= 100)
            {
                SpecialSkill?.Invoke();
                Debug.Log("Using skill!");
                Energy = 0;
            }

            if (!(GameController.RandomGenerator.Next(0, 101) * 0.01f < SpecialEffectChance)) return;

            if (Debuff != null)
                target.CCDebuff = Debuff;
        }
    }

    public class Debuff
    {
        public static Debuff GetNewCopy(Debuff original)
        {
            Debuff copy = new Debuff(original._module);
            foreach (PropertyInfo property in original.GetType().GetProperties())
                property.SetValue(copy, property.GetValue(original));
            copy.Active = true;
            return copy;
        }

        public bool Active { get; set; } = true;

        private string _cooldownKey = Guid.NewGuid().ToString();
        private string _cooldownToSelfDestruct = Guid.NewGuid().ToString();
        public float Cooldown { get; set; } = 1;
        public int DamagePerTick => _module.SpecialEffectDamagePerTick;

        private SpecialAttacksModule _module;

        public Debuff(SpecialAttacksModule module)
        {
            _module = module;
            CooldownController.GetCooldown(_cooldownToSelfDestruct, _module.SpecialEffectDuration);
        }

        public void ProcessEffect<TEntityType, TTargetType>(AttackingEntity<TEntityType, TTargetType> entity)
        where TEntityType:LivingEntity
        where TTargetType: LivingEntity
        {
            if (!CooldownController.GetCooldown(_cooldownKey, Cooldown)) return;
            entity.GetHit(new Attack(DamagePerTick, DamageType.Default), null);
        }

        public UnitParameters GetNegativeEffects(UnitParameters baseParameters)
        {
            return new UnitParameters()
            {
                MovementSpeed = baseParameters.MovementSpeed * (1-_module.SpecialEffectSlow),
                AttacksPerSecond = baseParameters.MovementSpeed * (1-_module.SpecialEffectAttackSpeedReduction),
                DamageResistance =  baseParameters.DamageResistance * (1-_module.SpecialEffectDamageIntakeIncreased),
            };
        }

        public void UpdateDuration()
        {
            if (CooldownController.GetCooldown(_cooldownToSelfDestruct, _module.SpecialEffectDuration))
                Active = false;
        }
    }
}
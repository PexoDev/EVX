using System;
using System.Collections.Generic;
using Assets.Scripts.Controllers;
using Assets.Scripts.Units;
using Assets.Scripts.Units.Enemy;
using Assets.Scripts.Units.Soldier;

namespace Assets.Scripts.Traits
{
    public static class TraitsList
    {
        public static readonly Action<Soldier> Mech = soldier =>
        {
            var mechTrait = new Trait(new UnitParameters
            {
                DamageResistance = 0.1f,
                DamageThreshold = 10,
                HealingPerSecond = 0,
                EvasionChance = 0,
                MaxHealth = soldier.UnitParams.MaxHealth * 2,
                DeflectionChance = 0.05f
            });
            mechTrait.ApplyParameters(soldier);
            AddToSoldierList(soldier, new MechTraitsList());
            soldier.SpecialSkills.Add(() =>
            {
                SpecialAttacks.DeflectionShield(soldier);
            });
        };

        public static readonly Action<Soldier> Bio = soldier =>
        {
            var mechTrait = new Trait(new UnitParameters
            {
                DamageResistance = 0f,
                DamageThreshold = 0,
                HealingPerSecond = (int?)(soldier.UnitParams.MaxHealth * 0.1f),
                EvasionChance = 0.15f,
                DeflectionChance = 0f
            });
            mechTrait.ApplyParameters(soldier);
            AddToSoldierList(soldier, new BioTraitsList());
            soldier.SpecialSkills.Add(() =>
            {
                SpecialAttacks.BioHeal(soldier, (SoldiersController)soldier.Controller);
            });
        };

        public static readonly Action<Soldier> FocusFire = soldier =>
        {
            var ffTrait = new Trait(new UnitParameters
            {
                AttacksPerSecond = soldier.UnitParams.AttacksPerSecond * 0.5f,
                Damage = soldier.UnitParams.Damage * 2,
                CriticalChance = 0.2f,
                CriticalMultiplier = 2f,
                AttackRange = soldier.UnitParams.AttackRange + 1
            });
            ffTrait.ApplyParameters(soldier);
            AddToSoldierList(soldier, new FocusFireTraitsList());
            soldier.SpecialSkills.Add(() =>
            {
                SpecialAttacks.SniperStrike(soldier, soldier.LookForTarget());
            });
        };

        public static readonly Action<Soldier> CoverFire = soldier =>
        {
            var cfTrait = new Trait(new UnitParameters
            {
                AttacksPerSecond = soldier.UnitParams.AttacksPerSecond * 2f,
                Damage = soldier.UnitParams.Damage / 2,
                CriticalChance = 0.1f
            });
            cfTrait.ApplyParameters(soldier);
            AddToSoldierList(soldier, new CoverFireTraitsList());
            soldier.SpecialSkills.Add(() =>
            {
                SpecialAttacks.RocketAttack(soldier, (EnemiesController)soldier.TargetsController, soldier.LookForTarget());
            });
        };

        public static readonly Action<Soldier> Peacemonger = soldier =>
        {
            var ffTrait = new Trait(new UnitParameters
            {
                MaxEnergy = 100,
                Energy = 100,
                EnergyGainPerSecond = 10,
                SpecialEffectChance = .5f,
                SpecialEffectDuration = 2f,
                SpecialEffectDamagePerTick = 0,
                SpecialEffectSlow = .33f,
                SpecialEffectDamageIntakeIncreased = .25f,
                SpecialEffectAttackSpeedReduction = 0.25f
            });
            ffTrait.ApplyParameters(soldier);
            soldier.SpecialAttacksModule.Active = true;
            AddToSoldierList(soldier, new PeacemongerTraitsList());
            soldier.SpecialSkills.Add(() =>
            {
                SpecialAttacks.Freeze(soldier.LookForTarget());
            });
        };

        public static readonly Action<Soldier> Warmonger = soldier =>
        {
            var cfTrait = new Trait(new UnitParameters
            {
                MaxEnergy = 100,
                Energy = 100,
                EnergyGainPerSecond = 10,
                SpecialEffectChance = .5f,
                SpecialEffectDuration = 4f,
                SpecialEffectDamagePerTick = 50,
                SpecialEffectSlow = 0f,
                SpecialEffectDamageIntakeIncreased = 0f,
                SpecialEffectAttackSpeedReduction = 0f
            });
            cfTrait.ApplyParameters(soldier);
            soldier.SpecialAttacksModule.Active = true;
            AddToSoldierList(soldier,new WarmongerTraitsList());
            soldier.SpecialSkills.Add(() =>
            {
                SpecialAttacks.AuraOfFire(soldier, (EnemiesController) soldier.TargetsController);
            });
        };

        private static void AddToSoldierList(Soldier soldier, SelectableTraitsList list)
        {
            soldier.SelectableTraits.AddRange(list.Traits);
            soldier.SelectableUltimateTraits.Add(list.UltimateTrait);
        }
        
        public static Func<Soldier, List<SelectableTrait>> RandomTraits = soldier =>
        {
            var traitsCopy = new List<SelectableTrait>(soldier.SelectableTraits);
            List<SelectableTrait> traitsToSelect = new List<SelectableTrait>();
            for (int i = 0; i < 3; i++)
            {
                var rand = GameController.RandomGenerator.Next(0, traitsCopy.Count);
                traitsToSelect.Add(traitsCopy[rand]);
                traitsCopy.RemoveAt(rand);
            }
            return traitsToSelect;
        };
    }
}
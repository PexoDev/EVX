using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.Scripts.Units
{
    //Fighting Entities Statistics:
    //Damage[p]
    //Attack Frequency[hz]
    //Health[p]
    //Health Regeneration Per Second [p/s]
    //Attack AttackRange[distance units]
    //Damage Resistance[p]
    //Critical Strike Chance[%]
    //Critical Strike Multiplier[%]
    //Evasion Chance[%]
    //Deflection Chance[%]
    //“Energy” [100 points]
    //“Energy” Gain[p/s]
    //Special Effect Chance[%]
    //Special Effect Duration[s]
    //Special Effect Damage per tick[p]
    //Special Effect Slow[%]
    //Special Effect Attack Speed Reduction[%]
    //Special Effect Increased Damage Intake[%]

    public class UnitParameters
    {

        public static UnitParameters GetCopy(UnitParameters up)
        {
            UnitParameters copy = new UnitParameters();
            foreach (PropertyInfo property in up.GetType().GetProperties())
                property.SetValue(copy,property.GetValue(up));
            return copy;
        }

        public static UnitParameters CopyOrFillWithDefault(UnitParameters up)
        {
            UnitParameters copy = new UnitParameters();
            foreach (PropertyInfo property in up.GetType().GetProperties())
            {
                var currentValue = property.GetValue(up) ?? Activator.CreateInstance(Nullable.GetUnderlyingType(property.PropertyType));
                property.SetValue(copy, currentValue);
            }
            return copy;
        }

        public static UnitParameters SetNewestOrDefault(UnitParameters baseUp, UnitParameters newUp)
        {
            UnitParameters copy = new UnitParameters();
            foreach (PropertyInfo property in newUp.GetType().GetProperties())
            {
                var currentValue = property.GetValue(newUp) ?? property.GetValue(baseUp) ?? Activator.CreateInstance(Nullable.GetUnderlyingType(property.PropertyType));
                property.SetValue(copy, currentValue);
            }
            return copy;
        }

        public UnitParameters IncreaseAllByFactor(float factor)
        {
            UnitParameters copy = new UnitParameters();
            foreach (PropertyInfo property in GetType().GetProperties())
            {
                var truePropertyType = Nullable.GetUnderlyingType(property.PropertyType);
                var value = property.GetValue(this);
                if(value == null) continue;

                var castValue = Convert.ChangeType(value, truePropertyType);
                object increasedValue = null;
                if (truePropertyType == typeof(int)) increasedValue = (int) castValue * (1 + factor);
                if (truePropertyType == typeof(float)) increasedValue = (float) castValue * (1 + factor);
                property.SetValue(copy, Convert.ChangeType(increasedValue, truePropertyType));
            }
            return copy;
        }

        public int? MaxHealth { get; set; }
        public int? Health { get; set; }
        public int? HealingPerSecond { get; set; }
        public int? DamageThreshold { get; set; }
        public float? DamageResistance { get; set; }
        public float? EvasionChance { get; set; }
        public float? DeflectionChance { get; set; }
        public float? MovementSpeed { get; set; }

        public int? AttackRange { get; set; }
        public float? AttacksPerSecond { get; set; }
        public int? Damage { get; set; }
        public float? CriticalChance { get; set; }
        public float? CriticalMultiplier { get; set; }

        public int? MaxEnergy { get; set; }
        public int? Energy { get; set; }
        public int? EnergyGainPerSecond { get; set; }
        public float? SpecialEffectChance { get; set; }
        public float? SpecialEffectDuration { get; set; }
        public int? SpecialEffectDamagePerTick { get; set; }
        public float? SpecialEffectSlow { get; set; }
        public float? SpecialEffectAttackSpeedReduction { get; set; }
        public float? SpecialEffectDamageIntakeIncreased { get; set; }
    }
}
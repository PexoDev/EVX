using System;
using System.Reflection;

namespace Assets.Scripts.Units
{
    public class UnitParameters
    {
        public event Action<UnitParameters> OnValueChanged = up => { };

        public void ValueChanged()
        {
            OnValueChanged(this);
        }

        public int? Health { get; set; }
        public int? RegenerationPerSecond { get; set; }

        public int? Armor { get; set; }
        public int? EMF { get; set; }
        public int? EnergyShields { get; set; }

        public int? PlasmaThreshold { get; set; }
        public int? LaserThreshold { get; set; }
        public int? BallisticThreshold { get; set; }

        public float? PlasmaResistance { get; set; }
        public float? LaserResistance { get; set; }
        public float? BallisticResistance { get; set; }

        public float? EvasionChance { get; set; }
        public float? DeflectionChance { get; set; }
        public float? MovementSpeed { get; set; }
        public float? Luck { get; set; }

        public int? AttackRange { get; set; }
        public float? AttacksPerSecond { get; set; }

        public int? PlasmaDamage { get; set; }
        public int? LaserDamage { get; set; }
        public int? BallisticDamage { get; set; }

        public float? CriticalChance { get; set; }
        public float? CriticalMultiplier { get; set; }
        public int? ClipSize { get; set; }
        public float? ReloadTime { get; set; }

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

        public static UnitParameters operator * (UnitParameters up, float factor)
        {
            return up.IncreaseAllByFactor(factor);
        }

        public static UnitParameters operator + (UnitParameters a, UnitParameters b)
        {
            foreach (PropertyInfo property in typeof(UnitParameters).GetProperties())
            {
                var truePropertyType = Nullable.GetUnderlyingType(property.PropertyType);
                var valueA = property.GetValue(a);
                var valueB = property.GetValue(b);
                if (valueB == null) continue;

                var castValueA = Convert.ChangeType(valueA, truePropertyType);
                var castValueB = Convert.ChangeType(valueA, truePropertyType);

                object sum = null;
                if (truePropertyType == typeof(int)) sum = ((int?)valueA ?? 0) + (int?) valueB;
                if (truePropertyType == typeof(float)) sum = ((float?)valueA ?? 0) + (float?) valueB;

                property.SetValue(a, Convert.ChangeType(sum, truePropertyType));
            }

            return a;
        }

        public UnitParameters IncreaseAllByFactor(float factor)
        {
            foreach (PropertyInfo property in GetType().GetProperties())
            {
                var truePropertyType = Nullable.GetUnderlyingType(property.PropertyType);
                var value = property.GetValue(this);
                if(value == null) continue;

                var castValue = Convert.ChangeType(value, truePropertyType);
                object increasedValue = null;
                if (truePropertyType == typeof(int)) increasedValue = (int) castValue * (1 + factor);
                if (truePropertyType == typeof(float)) increasedValue = (float) castValue * (1 + factor);
                property.SetValue(this, Convert.ChangeType(increasedValue, truePropertyType));
            }

            return this;
        }
    }
}
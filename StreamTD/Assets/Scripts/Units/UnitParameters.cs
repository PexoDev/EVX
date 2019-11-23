using System;
using System.Reflection;

namespace Assets.Scripts.Units
{
    [Serializable]
    public class UnitParameters
    {
        public event Action<UnitParameters> OnValueChanged = up => { };

        public void ValueChanged()
        {
            OnValueChanged(this);
        }

        public int Health { get; set; }
        public int RegenerationPerSecond { get; set; }

        public int Armor { get; set; }
        public int EMF { get; set; }
        public int EnergyShields { get; set; }

        public int PlasmaThreshold { get; set; }
        public int LaserThreshold { get; set; }
        public int BallisticThreshold { get; set; }

        public float PlasmaResistance { get; set; }
        public float LaserResistance { get; set; }
        public float BallisticResistance { get; set; }

        private float _evasionChance;
        public float EvasionChance
        {
            get => _evasionChance + Luck;
            set => _evasionChance = value;
        }

        private float _deflectionChance;
        public float DeflectionChance
        {
            get => _deflectionChance + Luck;
            set => _deflectionChance = value;
        }

        public float MovementSpeed { get; set; }
        public float Luck { get; set; }

        public int AttackRange { get; set; }
        public float AttacksPerSecond { get; set; }
        public float AttackAccuracy { get; set; }

        public int DefaultDamage { get; set; }
        public int PlasmaDamage { get; set; }
        public int LaserDamage { get; set; }
        public int BallisticDamage { get; set; }

        private float _criticalChance;
        public float CriticalChance
        {
            get => _criticalChance + Luck;
            set => _criticalChance = value;
        }
        public float CriticalMultiplier { get; set; }
        public int ClipSize { get; set; }
        public float ReloadTime { get; set; }

        public static UnitParameters GetCopy(UnitParameters up)
        {
            UnitParameters copy = new UnitParameters();
            foreach (PropertyInfo property in up.GetType().GetProperties())
                property.SetValue(copy,property.GetValue(up));
            return copy;
        }
       
        public static UnitParameters operator * (UnitParameters up, float factor)
        {
            foreach (PropertyInfo property in typeof(UnitParameters).GetProperties())
            {
                if (property.PropertyType == typeof(int))
                {
                    var value = (int)property.GetValue(up);
                    property.SetValue(up, (int)(value *factor));
                }

                if (property.PropertyType == typeof(float))
                {
                    var value = (float)property.GetValue(up);
                    property.SetValue(up, value * factor);
                }
            }

            return up;
        }

        //!###! WARNING !###!
        //Floats are multiplied by their values
        //Ints are turned into percents and then multiplied
        //This means that if you have a multiplication by 2, all floats are doubled but ints are increased by 2% only.
        //I'm already sorry for all future generations trying to figure this part out.
        public static UnitParameters operator *(UnitParameters a, UnitParameters b)
        {
            foreach (PropertyInfo property in typeof(UnitParameters).GetProperties())
            {
                if (property.PropertyType == typeof(int))
                {
                    var valueA = (int)property.GetValue(a);
                    var valueB = (int)property.GetValue(b);
                    if(valueB == 0) continue;

                    //Here's the hack...
                    property.SetValue(a, (int)(valueA * (1 + valueB/100f)));
                }

                if (property.PropertyType == typeof(float))
                {
                    var valueA = (float)property.GetValue(a);
                    var valueB = (float)property.GetValue(b);
                    if (valueB > -0.01f && valueB < 0.01f) continue;

                    property.SetValue(a, valueA * valueB);
                }
            }

            return a;
        }

        public static UnitParameters operator + (UnitParameters a, UnitParameters b)
        {
            foreach (PropertyInfo property in typeof(UnitParameters).GetProperties())
            {
                if (property.PropertyType == typeof(int))
                {
                    var valueA = (int)property.GetValue(a);
                    var valueB = (int)property.GetValue(b);
                    property.SetValue(a, valueA + valueB);
                }

                if (property.PropertyType == typeof(float))
                {
                    var valueA = (float)property.GetValue(a);
                    var valueB = (float)property.GetValue(b);
                    property.SetValue(a, valueA + valueB);
                }
            }

            if(a.LaserDamage > 0) a.LaserDamage += a.DefaultDamage;
            if(a.PlasmaDamage> 0) a.PlasmaDamage += a.DefaultDamage;
            if(a.BallisticDamage > 0) a.BallisticDamage += a.DefaultDamage;
            a.DefaultDamage = 0;

            return a;
        }

        public static UnitParameters operator - (UnitParameters a, UnitParameters b)
        {
            return a + (b * -1);
        }
    }
}
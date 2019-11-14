using System.Collections.Generic;
using Assets.Scripts.Units;

namespace Assets.Scripts.Traits
{
    public class MechTraitsList : SelectableTraitsList
    {
        public override List<SelectableTrait> Traits { get; set; } = new List<SelectableTrait>
        {
            new SelectableTrait(soldier =>
            {
                new Trait(new UnitParameters
                {
                    MaxHealth = soldier.UnitParams.MaxHealth * 4
                }).ApplyParameters(soldier);
            }){Name = "Vitality Core Improvement"},

            new SelectableTrait(soldier =>
            {
                new Trait(new UnitParameters
                {
                    DeflectionChance = soldier.UnitParams.DeflectionChance + 0.1f
                }).ApplyParameters(soldier);
            }){Name = "Improved Deflection"},

            new SelectableTrait(soldier =>
            {
                new Trait(new UnitParameters
                {
                    DamageThreshold = soldier.UnitParams.DamageThreshold + 5
                }).ApplyParameters(soldier);
            }){Name = "Hardened Armor"},

            new SelectableTrait(soldier =>
            {
                new Trait(new UnitParameters
                {
                    DamageResistance = soldier.UnitParams.DamageResistance + 0.1f
                }).ApplyParameters(soldier);
            }){Name = "New Metal Plating"},

            new SelectableTrait(soldier =>
            {
                new Trait(new UnitParameters
                {
                }).ApplyParameters(soldier);
            }){Name = "Placeholder"}
        };
        public override SelectableTrait UltimateTrait { get; set; } = new SelectableTrait(soldier =>
            {
                new Trait(new UnitParameters
                {

                }).ApplyParameters(soldier);
            })
            { Name = "Placeholder" };
    }
}
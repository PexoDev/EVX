using System.Collections.Generic;
using Assets.Scripts.Units;

namespace Assets.Scripts.Traits
{
    public class BioTraitsList : SelectableTraitsList
    {
        public override List<SelectableTrait> Traits { get; set; } = new List<SelectableTrait>
        {
            new SelectableTrait(soldier =>
            {
                new Trait(new UnitParameters
                {
                    HealingPerSecond = soldier.UnitParams.HealingPerSecond * 2,
                }).ApplyParameters(soldier);
            }){Name = "Increased Healing"},

            new SelectableTrait(soldier =>
            {
                new Trait(new UnitParameters
                {
                    EvasionChance = soldier.UnitParams.EvasionChance + 0.1f
                }).ApplyParameters(soldier);
            }){Name = "Improved Evasion"},

            new SelectableTrait(soldier =>
            {
                new Trait(new UnitParameters
                {
                    MaxHealth = (int?)(soldier.UnitParams.MaxHealth * 1.2f)
                }).ApplyParameters(soldier);
            }){Name = "Enhanced Endurance"},

            new SelectableTrait(soldier =>
            {
                new Trait(new UnitParameters
                {
                }).ApplyParameters(soldier);
            }){Name = "Placeholder"},

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
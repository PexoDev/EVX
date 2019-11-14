using System.Collections.Generic;
using Assets.Scripts.Units;

namespace Assets.Scripts.Traits
{
    public class WarmongerTraitsList:SelectableTraitsList
    {
        public override List<SelectableTrait> Traits { get; set; } = new List<SelectableTrait>
        {
            new SelectableTrait(soldier =>
            {
                new Trait(new UnitParameters
                {
                    SpecialEffectDamagePerTick = soldier.UnitParams.SpecialEffectDamagePerTick * 2
                }).ApplyParameters(soldier);
            }){Name = "Fiery fire"},

            new SelectableTrait(soldier =>
            {
                new Trait(new UnitParameters
                {
                    SpecialEffectDuration = soldier.UnitParams.SpecialEffectDuration * 2
                }).ApplyParameters(soldier);
            }){Name = "Extended Oxidation"},

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
        }){Name = "Placeholder"};
    }
}
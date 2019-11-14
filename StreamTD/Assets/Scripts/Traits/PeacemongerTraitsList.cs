using System.Collections.Generic;
using Assets.Scripts.Units;

namespace Assets.Scripts.Traits
{
    public class PeacemongerTraitsList:SelectableTraitsList
    {
        public override List<SelectableTrait> Traits { get; set; } = new List<SelectableTrait>
        {
            new SelectableTrait(soldier =>
            {
                new Trait(new UnitParameters
                {
                    SpecialEffectSlow = 0.5f
                }).ApplyParameters(soldier);
            }){Name = "Slooooooower sloooooow"},

            new SelectableTrait(soldier =>
            {
                new Trait(new UnitParameters
                {
                    SpecialEffectDamageIntakeIncreased = .33f
                }).ApplyParameters(soldier);
            }){Name = "Crio hydrogen"},

            new SelectableTrait(soldier =>
            {
                new Trait(new UnitParameters
                {
                    SpecialEffectDuration = soldier.UnitParams.SpecialEffectDuration * 2
                }).ApplyParameters(soldier);
            }){Name = "Looooooonger slooooow"},

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
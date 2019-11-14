using System.Collections.Generic;
using Assets.Scripts.Units;

namespace Assets.Scripts.Traits
{
    public class CoverFireTraitsList:SelectableTraitsList
    {
        public override List<SelectableTrait> Traits { get; set; } = new List<SelectableTrait>
        {
            new SelectableTrait(soldier =>
            {
                new Trait(new UnitParameters
                {
                    AttacksPerSecond = soldier.UnitParams.AttacksPerSecond * 2,
                    Damage = soldier.UnitParams.Damage / 2
                }).ApplyParameters(soldier);
            }){Name = "Rubber bullets"},

            new SelectableTrait(soldier =>
            {
                new Trait(new UnitParameters
                {
                    AttackRange = 2
                }).ApplyParameters(soldier);
            }){Name = "Improved rifle grip"},

            new SelectableTrait(soldier =>
            {
                new Trait(new UnitParameters
                {
                    AttacksPerSecond = soldier.UnitParams.AttacksPerSecond + 0.5f
                }).ApplyParameters(soldier);
            }){Name = "Larger magazines"},

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
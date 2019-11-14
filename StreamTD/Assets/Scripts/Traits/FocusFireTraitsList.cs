using System.Collections.Generic;
using Assets.Scripts.Units;

namespace Assets.Scripts.Traits
{
    public class FocusFireTraitsList:SelectableTraitsList
    {
        public override List<SelectableTrait> Traits { get; set; } = new List<SelectableTrait>
        {
            new SelectableTrait(soldier =>
            {
                new Trait(new UnitParameters
                {
                    AttacksPerSecond = soldier.UnitParams.AttacksPerSecond / 2,
                    Damage = soldier.UnitParams.Damage * 2
                }).ApplyParameters(soldier);
            }){Name = "EVEN heavier rounds"},

            new SelectableTrait(soldier =>
            {
                new Trait(new UnitParameters
                {
                    AttackRange = 3
                }).ApplyParameters(soldier);
            }){Name = "Sniper's Scope"},

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
            })
            { Name = "Placeholder" };
    }
}
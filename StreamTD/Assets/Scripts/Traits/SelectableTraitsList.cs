using System;
using System.Collections.Generic;
using Assets.Scripts.Units.Soldier;

namespace Assets.Scripts.Traits
{
    public abstract class SelectableTraitsList
    {
        public abstract List<SelectableTrait> Traits { get; set; }
        public abstract SelectableTrait UltimateTrait { get; set; }
    }

    public class SelectableTrait
    {
        public string Name { get; set; }
        public Action<Soldier> TraitAction;

        public SelectableTrait(Action<Soldier> traitAction)
        {
            TraitAction = soldier =>
            {
                traitAction(soldier);
                if(soldier.SelectableTraits.Contains(this)) soldier.SelectableTraits.Remove(this);
                else soldier.SelectableUltimateTraits.Remove(this);
            };
        }
    }
}
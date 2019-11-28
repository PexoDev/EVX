using System;
using System.Collections.Generic;
using Assets.Scripts.Units;
using Assets.Scripts.Units.Enemy;
using Assets.Scripts.Units.Soldier;

namespace Assets.Scripts
{
    public abstract class Item
    {
        public string Name { get; set; }
        public UnitParameters StatsChange { get; set; }

        public List<Action<Soldier, Enemy>> OnHitActions = new List<Action<Soldier, Enemy>>();
        public Action<Unit> SpecialEffectAction;

        protected readonly bool _multiplyStats;

        public Item(UnitParameters statsChange, bool multiplyStats = false)
        {
            StatsChange = statsChange;
            _multiplyStats = multiplyStats;
        }

        public virtual void Apply(Unit unit)
        {
            unit.UP = _multiplyStats ? unit.UP * StatsChange : unit.UP + StatsChange;
            SpecialEffectAction?.Invoke(unit);
        }
    }

    public class EquipmentItem : Item
    {
        public EquipmentItem(UnitParameters statsChange, bool multiplyStats = false) : base(statsChange, multiplyStats)
        {
        }

        public override void Apply(Unit unit)
        {
            unit.AddItem(this);
        }

        public UnitParameters Apply(UnitParameters up)
        {
            return _multiplyStats ? up * StatsChange : up + StatsChange;
        }
    }

    public class Mutagen : Item
    {
        public Mutagen(UnitParameters statsChange, bool multiplyStats = false) : base(statsChange, multiplyStats)
        {

        }
    }

    public class ConsumableItem : Item
    {
        public ConsumableItem(UnitParameters statsChange, bool multiplyStats = false) : base(statsChange, multiplyStats)
        {

        }
    }

}
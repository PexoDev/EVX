using System;
using System.Collections.Generic;
using Assets.Scripts.Units;
using Assets.Scripts.Units.Enemy;
using Assets.Scripts.Units.Soldier;

namespace Assets.Scripts
{
    public class ConsumableItem
    {
        public string Name { get; set; }
        public UnitParameters StatsChange { get; set; }

        public List<Action<Soldier, Enemy>> OnHitActions = new List<Action<Soldier, Enemy>>();

        private readonly bool _multiplyStats;

        public ConsumableItem(UnitParameters statsChange, bool multiplyStats = false)
        {
            StatsChange = statsChange;
            _multiplyStats = multiplyStats;
        }

        public UnitParameters Apply(Unit unit)
        {
            var up = unit.UP;
            if (_multiplyStats)
                up *= StatsChange;
            else
                up += StatsChange;

            return up;
        }
    }
}
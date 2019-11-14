using System;
using Assets.Scripts.Units.Soldier;

namespace Assets.Scripts
{
    public class LevelingUpPreset
    {
        public string Title { get; set; }
        public string LeftButton { get; set; }
        public string MidButton { get; set; }
        public string RightButton { get; set; }
        public Action<Soldier> LeftButtonAct { get; set; }
        public Action<Soldier> MidButtonAct { get; set; }
        public Action<Soldier> RightButtonAct { get; set; }
    }
}
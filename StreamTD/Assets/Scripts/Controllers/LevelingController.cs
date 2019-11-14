using Assets.Scripts.Traits;
using Assets.Scripts.Units.Soldier;

namespace Assets.Scripts.Controllers
{
    public class LevelingController
    {
        public static int MaxLevel = 10;

        public static int[] ExperiencePerLevel { get; } = {100, 300, 60, 1100, 1500, 2100, 2700, 3500, 4300, 5300};

        public static LevelingUpPreset GetLevelingUpPreset(Soldier soldier, int level)

        {
            if (level < 3) return _levelingUpPresets[level];
            if (level < 9)
            {
                var traits = TraitsList.RandomTraits(soldier);
                return new LevelingUpPreset
                {
                    Title = $"Level {level + 1}!", LeftButton = traits[0].Name, MidButton = traits[1].Name,
                    RightButton = traits[2].Name,
                    LeftButtonAct = traits[0].TraitAction,
                    MidButtonAct = traits[1].TraitAction,
                    RightButtonAct = traits[2].TraitAction,
                };
            }

            var ultimateTraits = soldier.SelectableUltimateTraits;
            return new LevelingUpPreset
            {
                Title = $"Level {level + 1}!",
                LeftButton = ultimateTraits[0].Name,
                MidButton = ultimateTraits[1].Name,
                RightButton = ultimateTraits[2].Name,
                LeftButtonAct = ultimateTraits[0].TraitAction,
                MidButtonAct = ultimateTraits[1].TraitAction,
                RightButtonAct = ultimateTraits[2].TraitAction,
            };
        }

        private static LevelingUpPreset[] _levelingUpPresets { get; } =
        {
            new LevelingUpPreset
            {
                Title = "Level 1!", LeftButton = "Bio", MidButton = "", RightButton = "Mech",
                LeftButtonAct = TraitsList.Bio,
                RightButtonAct = TraitsList.Mech
            },
            new LevelingUpPreset
            {
                Title = "Level 2!", LeftButton = "Focus Fire", MidButton = "", RightButton = "Cover Fire",
                LeftButtonAct = TraitsList.FocusFire,
                RightButtonAct = TraitsList.CoverFire
            },
            new LevelingUpPreset
            {
                Title = "Level 3!", LeftButton = "Peacemonger", MidButton = "", RightButton = "Warmonger",
                LeftButtonAct = TraitsList.Peacemonger,
                RightButtonAct = TraitsList.Warmonger
            }
        };
    }
}
using System;
using System.Threading.Tasks;
using Assets.Scripts.Attacks;
using Assets.Scripts.Units.Enemy;
using Assets.Scripts.Units.Soldier;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Controllers
{
    public class HQUIManager
    {
        private GameObject _backgroundPrefab;
        private readonly Button _hqNewSoldierTilesButton;

        public static DamageType SelectedDamageType;
        public static HealthType SelectedHealthType;

        public HQUIManager(Button hqNewSoldierTilesButton)
        {
            _hqNewSoldierTilesButton = hqNewSoldierTilesButton;
        }

        public void Instantiate(Func<string, string, string, string, Action,Action,Action, Task> setButtonsBehaviour, EnemiesController ec, SoldiersController sc)
        { 
            _hqNewSoldierTilesButton.onClick.AddListener(async () =>
            {
                GameController.Mode = GameMode.Building;

               await setButtonsBehaviour("Choose a weapon type for this soldier.","Laser Weapon", "Plasma Weapon", "Ballistic Weapon",
                    () => { SelectedDamageType = DamageType.Laser; },
                    () => { SelectedDamageType = DamageType.Plasma; },
                    () => { SelectedDamageType = DamageType.Ballistic; }
                    );

               await setButtonsBehaviour("Choose a type of protection for this soldier.","EMF Defense", "Armor", "Energy Shields",
                    () => { SelectedHealthType = HealthType.EMF; },
                    () => { SelectedHealthType = HealthType.Armor; },
                    () => { SelectedHealthType = HealthType.EnergyShields; }
                );
            });
        }
    }
}

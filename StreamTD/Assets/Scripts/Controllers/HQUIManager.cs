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
        public const int UnitPrice = 300;
        private GameObject _backgroundPrefab;
        private EconomyController _ecoController;
        private readonly Button _recruitNewUnitButton;
        private readonly Button _sellUnitButton;

        public static DamageType SelectedDamageType;
        public static HealthType SelectedHealthType;
        private UpgradeUIManager _upgradeManager;

        public HQUIManager(Button recruitNewUnitButton, Button sellUnitButton, EconomyController ecoContr, UpgradeUIManager upgradeManager)
        {
            _upgradeManager = upgradeManager;
            _ecoController = ecoContr;
            _recruitNewUnitButton = recruitNewUnitButton;
            _sellUnitButton = sellUnitButton;
        }

        public void Instantiate(Func<string, string, string, string, Action,Action,Action, Task> setButtonsBehaviour, EnemiesController ec, SoldiersController sc)
        { 
            _recruitNewUnitButton.onClick.AddListener(async () =>
            {
                if (!_ecoController.TryBuy(UnitPrice))
                {
                    Debug.Log("Too little quants! Try earning more.");
                    return;
                }
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

            _sellUnitButton.onClick.AddListener(() =>
            {
                if(_upgradeManager.CurrentUnit == null) return;
                _upgradeManager.CurrentUnit.RemoveUnit();
                _upgradeManager.CurrentUnit = null;
            });
        }
    }
}

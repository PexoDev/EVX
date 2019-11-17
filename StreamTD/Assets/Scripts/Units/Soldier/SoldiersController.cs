using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Controllers;
using UnityEngine;

namespace Assets.Scripts.Units.Soldier
{
    public class SoldiersController: EntitiesController<Soldier>
    {
        private string _energyRegenCooldownKey = Guid.NewGuid().ToString();

        public SoldiersController(GameController gc) : base(gc)
        {
        }

        public bool SpawnSoldier(Soldier soldier, InteractiveMapField field)
        {
            if (Entities.Count >= 10)
            {
                Debug.Log("You can't have more than 10 soldiers at once!");
                return false;
            }

            soldier.Tile = field.Field;

            Entities.Add(soldier);
            field.ClickableObject.OnClickActions.Push(async ()=>
            {
                await soldier.LevelUp();
                Gc.UIController.UpgradeManager.CurrentSoldier = soldier;
            });

            return true;
        }

        public void ProcessAttacks()
        {
            foreach (Soldier soldier in Entities)
            {
                if(soldier.Alive)
                    soldier.AutoAttack();
            }
        }

        public override void ProcessActions()
        {
            ApplyDebuffs();
            ProcessAttacks();
            ProcessLivingEntities();
            RegenEnergy();
        }

        private void RegenEnergy()
        {
            if (!CooldownController.GetCooldown(_energyRegenCooldownKey, 1)) return;

            foreach (var soldier in Entities)
            {
                if (soldier.Alive)
                    soldier.SpecialAttacksModule.Energy += soldier.SpecialAttacksModule.EnergyGainPerSecond;
            }
        }

        private void ApplyDebuffs()
        {
            foreach (Soldier soldier in Entities)
            {
                if (!soldier.Alive) continue;

                soldier.DOTDebuff?.UpdateDuration();
                soldier.CCDebuff?.UpdateDuration();

                if (soldier.DOTDebuff != null && !soldier.DOTDebuff.Active) soldier.DOTDebuff = null;
                if (soldier.CCDebuff != null && !soldier.CCDebuff.Active) soldier.CCDebuff = null;

                soldier.DOTDebuff?.ProcessEffect(soldier);
            }
        }

        public override void RemoveInstance(Soldier entity)
        {
            base.RemoveInstance(entity);
            Gc.ScoreController.Add(-100);
        }

        public void GrantExperienceToSoldiers(Enemy.Enemy entity)
        {
            var targetingSoldiers = Entities.Where(s => s.CurrentTarget == entity).ToArray();
            foreach (Soldier soldier in targetingSoldiers)
            {
                if (!soldier.Alive) return;
                soldier.Experience += entity.ScoreValue / targetingSoldiers.Length;
                soldier.KillCount++;
            }
        }
    }
}

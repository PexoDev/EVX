using System.Collections.Generic;
using Assets.Scripts.Controllers;
using Assets.Scripts.Units.Enemy;
using UnityEngine;

namespace Assets.Scripts.Units.Soldier
{
    public class SoldiersController: EntitiesController<Soldier>
    {
        private EnemiesController _ec;
        private Sprite[] _soldierSprites;
        public List<Unit> Units { get; set; } = new List<Unit>();

        public SoldiersController(GameController gc, EnemiesController ec, Sprite[] soldierSprites) : base(gc)
        {
            _ec = ec;
            _soldierSprites = soldierSprites;
        }

        public bool SpawnSoldier(InteractiveMapField field)
        {
            var unit = new Unit(_ec, this, field, Soldier.DefaultParams, HQUIManager.SelectedDamageType, HQUIManager.SelectedHealthType, _soldierSprites[GameController.RandomGenerator.Next(0,_soldierSprites.Length)]);
            Units.Add(unit);

            Entities.AddRange(unit.Soldiers);
            field.ClickableObject.OnClickActions.Push(() =>
            {
                Gc.UIController.UpgradeManager.CurrentUnit = unit;
            });

            GameController.Mode = GameMode.Play;
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
            ProcessAttacks();
            ProcessLivingEntities();
        }

        public override void RemoveInstance(Soldier entity)
        {
            base.RemoveInstance(entity);
            Gc.ScoreController.Add(-100);
        }
    }
}

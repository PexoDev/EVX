using System.Linq;
using Assets.Scripts.Controllers;
using UnityEngine;

namespace Assets.Scripts.Units.Enemy
{
    public class EnemiesController : EntitiesController<Enemy>
    {
        public static GameObject EnemyPrefab;
        public static Canvas ParentCanvas;

        private MapField _tile;
        private MapField StartingTile
        {
            get
            {
                if (_tile != null) return _tile;
                _tile = FindStartingTile();
                return _tile;
            }
        }

        private MapField FindStartingTile()
        {
            return Gc.Map.Path.First();
        }

        public void SpawnEnemy(Enemy enemy)
        {
            enemy.Move(StartingTile.Position);
            Entities.Add(enemy);
        }

        public void ProcessMovement()
        {
            var entities = Entities.ToArray();
            foreach (Enemy enemy in entities)
            {
                enemy.DoStep();
            }
        }

        public void ProcessAttacks()
        {
            var entities = Entities.ToArray();
            foreach (Enemy enemy in entities)
            {
                enemy.AutoAttack();
            }
        }

        public override void ProcessActions()
        {
            ApplyDebuffs();
            ProcessMovement();
            ProcessAttacks();
            ProcessLivingEntities();
        }

        private void ApplyDebuffs()
        {
            var entities = Entities.ToArray();
            foreach (Enemy enemy in entities)
            {
                enemy.DOTDebuff?.UpdateDuration();
                enemy.CCDebuff?.UpdateDuration();

                if (enemy.DOTDebuff != null && !enemy.DOTDebuff.Active) enemy.DOTDebuff = null;
                if (enemy.CCDebuff != null && !enemy.CCDebuff.Active) enemy.CCDebuff = null;

                if(enemy.DOTDebuff == null) return;
                enemy.DOTDebuff.ProcessEffect(enemy);
            }
        }

        public EnemiesController(GameController gc) : base(gc)
        {
        }

        public override void RemoveInstance(Enemy entity)
        {
            Gc.SoldiersController.GrantExperienceToSoldiers(entity);

            Gc.ScoreController.Add(entity.ScoreValue);
            base.RemoveInstance(entity);
        }
    }
}

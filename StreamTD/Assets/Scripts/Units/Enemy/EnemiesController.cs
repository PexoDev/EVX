using System.Linq;
using UnityEngine;
using Assets.Scripts.Controllers;
using System.Collections.Generic;

namespace Assets.Scripts.Units.Enemy
{
    public class EnemiesController : EntitiesController<Enemy>
    {
        public static Canvas ParentCanvas;
        private EnemySpawner _spawner;
        private readonly ObjectBodyPool<Enemy, EnemyType> _pool;
        private GameObject _enemyPrefab;
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

        public void SpawnEnemy(float powerLevel)
        {
            if (_spawner == null) _spawner = new EnemySpawner(Gc.Map.Path, Gc.SoldiersController, this, Gc.PlayerBase);

            var enemy = _spawner.GenerateEnemy(powerLevel);
            var newEnemy = _pool.GetObject(enemy, null, enemy.Type);
            newEnemy.Object.Move(StartingTile.Position + Vector2.one * GameController.RandomGenerator.Next(-3,3)*0.1f);
            Entities.Add(newEnemy.Object);
        }

        public void ProcessMovement()
        {
            foreach (Enemy enemy in Entities.ToArray())
                enemy.DoStep();
            _pool.UpdateAllBodies();

            foreach (var bodyPoolItem in _pool.Pool.Where(item => item.InUse))
            {
                var targetLookPosition = bodyPoolItem.Object.PathToTraverse[bodyPoolItem.Object.CurrentFieldIndex].Position;
                var newPosition = bodyPoolItem.Object.Position;
                bodyPoolItem.Body.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(targetLookPosition.y - newPosition.y, targetLookPosition.x - newPosition.x) * Mathf.Rad2Deg - 90f);
                if(bodyPoolItem.Object.MaxHp <= 0) return;
                bodyPoolItem.Body.transform.localScale = Vector3.one * 0.33f * bodyPoolItem.Object.HP / bodyPoolItem.Object.MaxHp;
            }
        }

        public void ProcessAttacks()
        {
            foreach (Enemy enemy in Entities.ToArray())
                enemy.AutoAttack();
        }

        public override void ProcessActions()
        {
            ProcessMovement();
            ProcessAttacks();
            ProcessLivingEntities();
        }
        public EnemiesController(GameController gc, GameObject enemyPrefab, Transform enemiesParent, Sprite spriteLight, Sprite spriteMedium, Sprite spriteHeavy) : base(gc)
        {
            _pool = new ObjectBodyPool<Enemy, EnemyType>(enemyPrefab, enemiesParent, new Dictionary<EnemyType, Sprite> {
                { EnemyType.Light, spriteLight},
                { EnemyType.Medium, spriteMedium},
                { EnemyType.Heavy, spriteHeavy}
            });
        }

        public override void RemoveInstance(Enemy entity)
        {
            Gc.ScoreController.Add(entity.ScoreValue);
            base.RemoveInstance(entity);

            _pool.ReturnToPool(entity);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using Assets.Scripts.Controllers;
using Assets.Scripts.Units.Enemy;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts
{
    public class WaveController
    {
        private bool _spawnTwo;
        private int _enemiesSpawned = 0;
        private float _enemySpawnCooldown = 0.45f;
        private float _currentEnemySpawnCooldown;
        private float _powerLevel = 1;

        private GameController _gc;
        private EnemiesController _ec;

        private readonly Queue<EnemyWave> _allWaves = new Queue<EnemyWave>();
        private EnemyWave _currentWave;
        private Queue<(float, EnemyType)> _currentGroup;

        public WaveController(GameController gc)
        {
            _gc = gc;
            _ec = gc.EnemiesController;
        }

        public void SpawnEnemy()
        {
            if (_currentEnemySpawnCooldown > 0)
            {
                _currentEnemySpawnCooldown -= Time.deltaTime;
                return;
            }

            SpawnSingleEnemy();
            if (_spawnTwo) SpawnSingleEnemy();

            _spawnTwo = !_spawnTwo;

            _currentEnemySpawnCooldown = _enemySpawnCooldown;
        }

        private void SpawnSingleEnemy()
        {
            if (_currentWave == null)
            {
                if(_allWaves.Count < 1) return;
                _currentWave = _allWaves.Dequeue();
            }

            if (_currentGroup == null || _currentGroup.Count < 1)
            {
                if(_currentWave.Enemies.Count<1) return;
                _currentGroup = _currentWave.Enemies.Dequeue();
            }
                
            var toSpawn = _currentGroup.Dequeue();
            Debug.Log($"Spawning: {toSpawn.Item1}/{toSpawn.Item2}");
            _gc.EnemiesController.SpawnEnemy(toSpawn.Item1, toSpawn.Item2);
        }

        private int EnemiesPerGroup = 5;
        private int GroupsPerWave = 5;
        private EnemyWave GenerateWave(EnemyWave.EnemyWaveType type)
        {
            Queue<Queue<(float, EnemyType)>> allGroups = new Queue<Queue<(float, EnemyType)>>();
            for (int j = 0; j < GroupsPerWave; j++)
            {
                List<EnemyType> availableTypes = new List<EnemyType>();
                if ((type & EnemyWave.EnemyWaveType.Fast) == EnemyWave.EnemyWaveType.Fast) availableTypes.Add(EnemyType.Light);
                if ((type & EnemyWave.EnemyWaveType.Medium) == EnemyWave.EnemyWaveType.Medium) availableTypes.Add(EnemyType.Medium);
                if ((type & EnemyWave.EnemyWaveType.Heavy) == EnemyWave.EnemyWaveType.Heavy) availableTypes.Add(EnemyType.Heavy);

                Queue<(float, EnemyType)> group = new Queue<(float, EnemyType)>();
                for (int i = 0; i < EnemiesPerGroup; i++)
                {
                    group.Enqueue((_powerLevel, availableTypes[GameController.RandomGenerator.Next(0, availableTypes.Count)]));
                    Debug.Log($"Generating: {group.Peek().Item1}/{group.Peek().Item2}");
                    _enemiesSpawned++;
                    _powerLevel = 1 + ((float) math.pow(_enemiesSpawned, 1.1) / 300 +
                                       math.cos(_enemiesSpawned * 0.1f) * 0.2f) * 0.25f;
                }
                allGroups.Enqueue(group);
            }

            return new EnemyWave(allGroups, type);
        }

        private int WavesPerLevel = 10;
        public void GenerateAllWaves()
        {
            for (int i = 0; i < WavesPerLevel; i++)
            {
                _allWaves.Enqueue(GenerateWave((EnemyWave.EnemyWaveType)GameController.RandomGenerator.Next(1,7)));
            }
        }
    }

    public class EnemyWave
    {
        public EnemyWaveType WaveType { get; }
        public Queue<Queue<(float, EnemyType)>> Enemies { get; }

        public EnemyWave(Queue<Queue<(float, EnemyType)>> enemies, EnemyWaveType type)
        {
            WaveType = type;
            Enemies = enemies;
        }

        [Flags]
        public enum EnemyWaveType
        {
            None = 0,
            Fast = 1,
            Medium = 2,
            Heavy = 4
        }
    }
}
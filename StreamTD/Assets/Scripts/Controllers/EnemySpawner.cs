
using System;
using System.Collections.Generic;
using Assets.Scripts.Attacks;
using Assets.Scripts.Units;
using Assets.Scripts.Units.Enemy;
using Assets.Scripts.Units.Soldier;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class EnemySpawner
    {
        private static Dictionary<int,List<Func<float,Enemy>>> AllEnemiesPrefabs = new Dictionary<int, List<Func<float,Enemy>>>();
        private EnemiesController _ec;

        public Enemy GenerateEnemy(float powerLevel)
        {
           return AllEnemiesPrefabs[0][GameController.RandomGenerator.Next(0, AllEnemiesPrefabs[0].Count)].Invoke(powerLevel);
        }

        public EnemySpawner(MapField[] path,SoldiersController sc, EnemiesController ec,PlayerBase pb)
        {
            Enemy Light(float powerLevel)
            {
                var light0UP = new UnitParameters {Health = 20, EvasionChance = 0.01f, MovementSpeed = 0.01f,} * powerLevel;
                return new DummyEnemy(path, sc, ec, pb, DamageType.Default, HealthType.Default, light0UP, 0) {Type = EnemyType.Light};
            }

            Enemy Mid(float powerLevel)
            {
                var mid0UP = new UnitParameters
                {
                    Health = 50,
                    EvasionChance = 0.01f,
                    MovementSpeed = 0.005f,
                } * powerLevel;
                return new DummyEnemy(path, sc, ec, pb, DamageType.Default, HealthType.Default, mid0UP, 0)
                    {Type = EnemyType.Medium, ScoreValue = 2};
            }

            Enemy Heavy(float powerLevel)
            {
                var heavy0UP = new UnitParameters
                {
                    Health = 100,
                    EvasionChance = 0.01f,
                    MovementSpeed = 0.01f,
                } * powerLevel;
                return new DummyEnemy(path, sc, ec, pb, DamageType.Default, HealthType.Default, heavy0UP, 0)
                    {Type = EnemyType.Heavy, ScoreValue = 2};
            }

            Enemy Assassin(float powerLevel)
            {
                var assassin0UP = new UnitParameters
                {
                    Health = 40,
                    EvasionChance = 0.01f,
                    MovementSpeed = 0.005f,
                    AttackAccuracy = 0.75f,
                    DefaultDamage = 1,
                    AttackRange = 3,
                    AttacksPerSecond = 1f
                } * powerLevel;
                return new DummyEnemy(path, sc, ec, pb, DamageType.Ballistic, HealthType.Default, assassin0UP, Mathf.FloorToInt(1 * powerLevel)) { Type = EnemyType.Light, ScoreValue = 2};
            }

            AllEnemiesPrefabs.Add(0,new List<Func<float,Enemy>> { Light, Mid,Heavy, Assassin});
        }
    }
}
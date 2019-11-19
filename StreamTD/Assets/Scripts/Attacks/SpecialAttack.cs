using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Assets.Scripts;
using Assets.Scripts.Units;
using Assets.Scripts.Units.Enemy;
using Assets.Scripts.Units.Soldier;
using UnityEngine;

namespace Assets
{
    public static class SpecialAttacks
    {
        public static void SniperStrike(Soldier executor, LivingEntity target)
        {
            //ProjectilesController.Instance.InitializeProjectile(new Projectile(damage, executor.DamageType, executor.Position, executor, target, () => { }));
        }

        private const int BioHealRange = 2;
        public static void BioHeal(Soldier executor, SoldiersController controller)
        {
            foreach (var entity in controller.Entities.Where(entity => entity.IsInRange(executor.Position,BioHealRange)))
            {
            }
        }

        private const int DeflectionShieldDuration = 2000;
        public static void DeflectionShield(Soldier executor)
        {
            Debug.Log("Shields up!");
            Task.Run(() =>
            {
                //todo Bug risk, what if leveled up during being shielded? Improved deflection would be lost 
                Debug.Log("Start"+DateTime.Now.Second);
                Thread.Sleep(DeflectionShieldDuration);
                Debug.Log("end" + DateTime.Now.Second);
            });
        }

        private const int ExplosionRange = 1;
        public static void RocketAttack(Soldier executor, EnemiesController controller, LivingEntity target)
        {
            //ProjectilesController.Instance.InitializeProjectile(new Projectile(dmg, executor.DamageType, executor.Position, executor, target,
            //    () =>
            //    {
            //        var collection =
            //            controller.Entities.Where(entity => entity.IsInRange(target.Position, ExplosionRange)).ToArray();
            //        for (var i = 0; i < collection.Length; i++)
            //        {
            //            collection[i].GetHit(new Projectile(dmg, executor.DamageType));
            //        }
            //    }));
        }

        private const int AuraOfFireRange = 2;
        public static void AuraOfFire(Soldier executor, EnemiesController controller)
        {
            foreach (var entity in controller.Entities.Where(entity => entity.IsInRange(executor.Position, AuraOfFireRange)))
            {
            }
        }

        private const int WallOfIceDuration = 900;
        public static void WallOfIce(Enemy target)
        {
            Task.Run(() =>
            {
                var index = target.CurrentFieldIndex + 1;
                if (index >= target.PathToTraverse.Length) index = target.PathToTraverse.Length - 1;
                target.PathToTraverse[index].Type = MapFieldType.Blocked;
                Thread.Sleep(WallOfIceDuration);
                target.PathToTraverse[index].Type = MapFieldType.Path;
            });
        }

        private const int FreezeDuration = 2000;
        public static void Freeze(Enemy target)
        {
            Task.Run(() =>
            {
                target.Frozen = true;
                Thread.Sleep(FreezeDuration);
                target.Frozen = false;
            });
        }
    }
}
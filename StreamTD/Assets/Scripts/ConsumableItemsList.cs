using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Attacks;
using Assets.Scripts.Units;
using Assets.Scripts.Units.Enemy;
using Assets.Scripts.Units.Soldier;
using UnityEngine;

namespace Assets.Scripts
{
    public static class ConsumableItemsList
    {
        private static ConsumableItem[] _allConsumableItems;
        public static ConsumableItem[] AllConsumableItems
        {
            get
            {
                if (_allConsumableItems == null)
                {
                    _allConsumableItems = typeof(ConsumableItemsList).GetFields()
                        .Select(field => (ConsumableItem) field.GetValue(null)).ToArray();
                }

                return _allConsumableItems;
            }
        }

        public static ConsumableItem Mutagen = new Mutagen(
            new UnitParameters
            {
                Health = 20,
                RegenerationPerSecond = 2
            }, true)
        {
            Name = "Health Elixir"
        };

        public static ConsumableItem RifleUpgrade = new EquipmentItem(
            new UnitParameters
            {
                DefaultDamage = 2,
                AttacksPerSecond = 0.5f,
                AttackAccuracy = 0.05f
            })
            {
                Name = "Rifle Upgrade"
            };

        public static ConsumableItem AmmoUpgrade = new EquipmentItem(
            new UnitParameters
            {
                ClipSize = 10,
                ReloadTime = -0.25f
            })
        {
            Name = "Better Ammo"
        };

        public static ConsumableItem ImprovedArmor = new EquipmentItem(
            new UnitParameters
            {
                 Armor = 100,
                 BallisticThreshold = 1
            })
        {
            Name = "Improved Armor"
        };

        public static ConsumableItem ExplosiveShells = new EquipmentItem(
            new UnitParameters
            {
                AttacksPerSecond = 0.25f
            }, true)
        {
            Name = "Explosive Shells",
            OnHitActions = new List<Action<Soldier, Enemy>>()
            {
                (soldier, enemy) =>
                {
                    foreach (var target in enemy.Controller.Entities.Where(e=> enemy.IsInRange(e.Position,1)).ToArray())
                    {
                        target.GetHit(new Attack()
                        {
                            LaserDamage = soldier._up.LaserDamage,
                            PlasmaDamage = soldier._up.PlasmaDamage,
                            BallisticDamage = soldier._up.BallisticDamage
                        }, null);
                    }
                }
            }
        };
    }
}
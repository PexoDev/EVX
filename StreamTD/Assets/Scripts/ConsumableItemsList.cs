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
        private static Item[] _allConsumableItems;
        public static Item[] AllConsumableItems
        {
            get
            {
                if (_allConsumableItems == null)
                {
                    _allConsumableItems = typeof(ConsumableItemsList).GetFields()
                        .Select(field => (Item) field.GetValue(null)).ToArray();
                }

                return _allConsumableItems;
            }
        }

        public static Item Medkit = new ConsumableItem(new UnitParameters())
        {
            Name = "Medkit",
            SpecialEffectAction = unit =>
            {
                foreach (Soldier soldier in unit.Soldiers)
                {
                    soldier.HP = soldier.MaxHp;
                }
            }
        };

        public static Item Mutagen = new Mutagen(
            new UnitParameters
            {
                Health = 20,
                RegenerationPerSecond = 2
            }, true)
        {
            Name = "Health Elixir"
        };

        public static Item RifleUpgrade = new EquipmentItem(
            new UnitParameters
            {
                DefaultDamage = 2,
                AttacksPerSecond = 0.5f,
                AttackAccuracy = 0.05f
            })
            {
                Name = "Rifle Upgrade"
            };

        public static Item AmmoUpgrade = new EquipmentItem(
            new UnitParameters
            {
                ClipSize = 10,
                ReloadTime = -0.25f
            })
        {
            Name = "Better Ammo"
        };

        public static Item ImprovedArmor = new EquipmentItem(
            new UnitParameters
            {
                 Armor = 100,
                 BallisticThreshold = 1
            })
        {
            Name = "Improved Armor"
        };

        public static Item ExplosiveShells = new EquipmentItem(
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
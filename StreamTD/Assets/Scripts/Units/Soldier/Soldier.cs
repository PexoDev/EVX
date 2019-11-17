using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Scripts.Attacks;
using Assets.Scripts.Controllers;
using Assets.Scripts.Traits;
using Assets.Scripts.Units.Enemy;
using UnityEngine;

namespace Assets.Scripts.Units.Soldier
{
    public abstract class Soldier: AttackingEntity<Soldier, Enemy.Enemy>
    {
        public static UnitParameters DefaultParams = new UnitParameters { Damage = 25, AttacksPerSecond = 1, MaxHealth = 250, Health = 250, AttackRange = 2};
        public List<SelectableTrait> SelectableTraits = new List<SelectableTrait>();
        public List<SelectableTrait> SelectableUltimateTraits = new List<SelectableTrait>();
        public List<Action> SpecialSkills = new List<Action>();

        public string Name { get; private set; }
        public int Experience { get; set; }

        public async Task LevelUp()
        {
            if (Level >= LevelingController.MaxLevel) return;
            if (Experience < LevelingController.ExperiencePerLevel[Level]) return;

            LevelingUpPreset lup = LevelingController.GetLevelingUpPreset(this, Level);
            Level++;

            GameController.Mode = GameMode.Upgrading;
            Action midAction = null;
            if (lup.MidButtonAct != null) midAction = () => { lup.MidButtonAct(this); };
            await Controller.Gc.UIController.SetButtonsBehaviour(lup.Title,lup.LeftButton,lup.MidButton,lup.RightButton,
                ()=>lup.LeftButtonAct(this), midAction, ()=>lup.RightButtonAct(this)
            );
            GameController.Mode = GameMode.Play;
        }

        private InteractiveMapField _tile;
        public InteractiveMapField Tile
        {
            get => _tile;
            set
            {
                _tile = value;
                if (_tile == null) return;
                Move(_tile.Field.Position);
            }
        }

        public int Level { get; set; }
        public int KillCount { get; set; }

        protected Soldier(string name, InteractiveMapField tile, EnemiesController ec, SoldiersController sc, DamageType dt, HealthType ht, UnitParameters up) : base(ec, sc, dt,ht,up)
        {
            Name = name;
            Tile = tile;
            SpecialAttacksModule.SpecialSkill = () => { if(SpecialSkills.Count>0)SpecialSkills[GameController.RandomGenerator.Next(0,SpecialSkills.Count)].Invoke(); };
        }

        protected Soldier(SoldiersController controller)
        {
            Controller = controller;
        }

        public override string ToString()
        {
            return Name;
        }

        public override void Die()
        {
            base.Die();
            Tile.Field.Type = MapFieldType.Empty;
            Tile.ClickableObject.OnClickActions.Pop();
            Controller.RemoveInstance(this);
        }

        public override void AnimateHurt()
        {
        }
    }
}
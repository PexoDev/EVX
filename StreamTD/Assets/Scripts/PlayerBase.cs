using Assets.Scripts.Units;
using Assets.Scripts.Units.Soldier;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerBase : Soldier
    {
        public PlayerBase(SoldiersController controller) : base(controller)
        {
            HP = 1000;
        }

        public override void AnimateHurt()
        {
            Debug.Log("Base attacked");
        }
    }
}
using Assets.Scripts.Attacks;
using Assets.Scripts.Units;
using Assets.Scripts.Units.Soldier;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerBase : Soldier
    {
        public static UnitParameters DefaultBaseParams = new UnitParameters() { Health = 1000 };
        public PlayerBase(SoldiersController controller) : base("Base",null,controller, DefaultBaseParams) { }

        public override void AnimateHurt()
        {
            Debug.Log("Base attacked");
        }
    }
}
using System.Runtime.CompilerServices;

namespace Assets.Scripts.Attacks
{
    public struct Attack : IAttack
    {
        public int LaserDamage { get; set; }
        public int PlasmaDamage { get; set; }
        public int BallisticDamage { get; set; }
        public int DefaultDamage { get; set; }
        public int OverallDamage => LaserDamage + PlasmaDamage + BallisticDamage + DefaultDamage;

        public static Attack operator *(Attack basic, float factor)
            {
                return new Attack
                {
                    BallisticDamage = (int) (basic.BallisticDamage * factor),
                    PlasmaDamage = (int) (basic.PlasmaDamage * factor),
                    LaserDamage = (int) (basic.LaserDamage * factor),
                    DefaultDamage = (int) (basic.DefaultDamage * factor),
                };
            }
    }
}
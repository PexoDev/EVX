namespace Assets.Scripts.Attacks
{
    public class Attack : IAttack
    {
        public int Damage { get; set; }
        public DamageType DamageType { get; set; }

        public Attack(int damage, DamageType damageType)
        {
            Damage = damage;
            DamageType = damageType;
        }
    }
}
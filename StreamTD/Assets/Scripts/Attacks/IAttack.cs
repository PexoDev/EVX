namespace Assets.Scripts.Attacks
{
    public interface IAttack
    {
        int Damage { get; set; }
        DamageType DamageType { get; set; }
    }
}
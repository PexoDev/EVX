namespace Assets.Scripts.Attacks
{
    public interface IAttack
    {
        DamageType DamageType { get; }
        int Damage { get; }
    }
}
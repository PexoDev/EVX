namespace Assets.Scripts.Attacks
{
    public interface IAttack
    {
        int LaserDamage { get; set; }
        int PlasmaDamage{ get; set; }
        int BallisticDamage { get; set; }
        int DefaultDamage { get; set; }

        int OverallDamage { get; }
    }

}
using Assets.Scripts;
using Assets.Scripts.Attacks;
using Assets.Scripts.Units;
using Assets.Scripts.Units.Soldier;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class DamageTypesTests
    {
        [Test]
        public void TestDamageTypes()
        {
            Assert.AreEqual((int)(100*Damage.WeakMultiplier), Damage.CalculateDamage(new Projectile(100,DamageType.Ballistic),HealthType.Armor,false));
            Assert.AreEqual(100, Damage.CalculateDamage(new Projectile(100,DamageType.Ballistic),HealthType.EMF,false));
            Assert.AreEqual((int)(100*Damage.StrongMultiplier), Damage.CalculateDamage(new Projectile(100,DamageType.Ballistic),HealthType.EnergyShields,false));

            Assert.AreEqual((int)(100 * Damage.WeakMultiplier), Damage.CalculateDamage(new Projectile(100, DamageType.Laser), HealthType.EnergyShields, false));
            Assert.AreEqual(100, Damage.CalculateDamage(new Projectile(100, DamageType.Laser), HealthType.Armor, false));
            Assert.AreEqual((int)(100 * Damage.StrongMultiplier), Damage.CalculateDamage(new Projectile(100, DamageType.Laser), HealthType.EMF, false));

            Assert.AreEqual((int)(100 * Damage.WeakMultiplier), Damage.CalculateDamage(new Projectile(100, DamageType.Plasma), HealthType.EMF, false));
            Assert.AreEqual(100, Damage.CalculateDamage(new Projectile(100, DamageType.Plasma), HealthType.EnergyShields, false));
            Assert.AreEqual((int)(100 * Damage.StrongMultiplier), Damage.CalculateDamage(new Projectile(100, DamageType.Plasma), HealthType.Armor, false));
        }
    }
}
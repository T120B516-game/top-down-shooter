namespace Shared
{
    public class Pistol : Weapon
    {
        public Pistol() : base(new SingleShot()) // Default to SingleShot mode
        {
        }
    }
}

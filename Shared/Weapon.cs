namespace Shared
{
    public abstract class Weapon
    {
        protected IShootingMode _shootingMode;

        public Weapon(IShootingMode shootingMode)
        {
            _shootingMode = shootingMode;
        }

        public void Shoot()
        {
            _shootingMode.Shoot();
        }

        public void SetShootingMode(IShootingMode newMode)
        {
            _shootingMode = newMode;
        }
    }
}

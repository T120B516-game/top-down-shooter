using Shared;

namespace Client
{
    public class ShootingHandler
    {
        private Weapon _currentWeapon;

        public void HandleShootingInput(KeyEventArgs e)
        {
            if (_currentWeapon == null) return;

            if (e.KeyCode == Keys.Space) // Single Shot mode
            {
                _currentWeapon.SetShootingMode(new SingleShot());
                _currentWeapon.Shoot();
            }
            else if (e.KeyCode == Keys.ControlKey) // Burst Shot mode
            {
                _currentWeapon.SetShootingMode(new BurstShot());
                _currentWeapon.Shoot();
            }
        }

        public void SetWeapon(Weapon weapon)
        {
            _currentWeapon = weapon;
        }
    }
}

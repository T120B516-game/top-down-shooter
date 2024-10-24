using Backend.CreationalPatterns;
using Shared;

namespace Backend
{
	public class EnemyRepository
	{
		private List<Enemy> Enemies { get; set; } = new List<Enemy>();
		private EnemyAbstractFactory _StationaryEnemyFactory = new StationaryEnemyFactory();
		private EnemyAbstractFactory _MobileEnemyFactory = new MobileEnemyFactory();

		public EnemyRepository()
		{
			var mobileMeeleEnemy = _MobileEnemyFactory.CreateMeeleEnemy(200, 500, 100, 5);
			mobileMeeleEnemy.SetMovementBehaviour();
			var mobileShootingEnemy = _MobileEnemyFactory.CreateShootingEnemy(200, 700, 100, 6);
			mobileShootingEnemy.SetMovementBehaviour();
			var stationaryShootingEnemy = _StationaryEnemyFactory.CreateShootingEnemy(300, 550, 100, 7);
			stationaryShootingEnemy.SetMovementBehaviour();
			Enemies.Add(mobileMeeleEnemy);
            Enemies.Add(mobileShootingEnemy);
            Enemies.Add(stationaryShootingEnemy);
        }

			Enemies.Add(mobileShootingEnemy);
			Enemies.Add(stationaryShootingEnemy);

			var clonedEnemy = mobileMeeleEnemy.ShallowClone();
			clonedEnemy.SetMovementBehaviour();

			Enemies.Add(clonedEnemy);

		}

		public async Task<List<Enemy>> ListAsync()
		{
			return Enemies;
		}
	}
}

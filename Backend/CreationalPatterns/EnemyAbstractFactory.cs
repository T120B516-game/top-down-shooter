using Shared;

namespace Backend.CreationalPatterns
{
	public abstract class EnemyAbstractFactory
	{
		public abstract Enemy CreateExplosiveEnemy(int X, int Y, int health, int id);
		public abstract Enemy CreateShootingEnemy(int X, int Y, int health, int id);
		public abstract Enemy CreateMeeleEnemy(int X, int Y, int health, int id);
	}

	public class MobileEnemyFactory : EnemyAbstractFactory
	{
		public override Enemy CreateExplosiveEnemy(int X, int Y, int health, int id)
		{
			return new MobileExplosiveEnemy(X, Y, health, id);
		}

		public override Enemy CreateMeeleEnemy(int X, int Y, int health, int id)
		{
			return new MobileMeeleEnemy(X, Y, health, id);
		}

		public override Enemy CreateShootingEnemy(int X, int Y, int health, int id)
		{
			return new MobileShootingEnemy(X, Y, health, id);
		}
	}

	public class StationaryEnemyFactory : EnemyAbstractFactory
	{
		public override Enemy CreateExplosiveEnemy(int X, int Y, int health, int id)
		{
			return new StationaryExplosiveEnemy(X, Y, health, id);
		}

		public override Enemy CreateMeeleEnemy(int X, int Y, int health, int id)
		{
			return new StationaryMeeleEnemy(X, Y, health, id);
		}

		public override Enemy CreateShootingEnemy(int X, int Y, int health, int id)
		{
			return new StationaryShootingEnemy(X, Y, health, id);
		}
	}
}

using Backend.CreationalPatterns;
using Shared;

namespace Backend;

public class EnemyRepository
{
	private readonly List<Enemy> _enemies = [];
	private readonly EnemyAbstractFactory _stationaryEnemyFactory = new StationaryEnemyFactory();
	private readonly EnemyAbstractFactory _mobileEnemyFactory = new MobileEnemyFactory();

	public EnemyRepository()
	{
		var mobileMeeleEnemy = _mobileEnemyFactory.CreateMeeleEnemy(200, 500, 100, 5);
		mobileMeeleEnemy.SetMovementBehaviour(new AdvancedMovement());

		var mobileShootingEnemy = _mobileEnemyFactory.CreateShootingEnemy(200, 700, 100, 6);
		mobileShootingEnemy.SetMovementBehaviour(new SimpleMovement());

		var stationaryShootingEnemy = _stationaryEnemyFactory.CreateShootingEnemy(300, 550, 100, 7);
		stationaryShootingEnemy.SetMovementBehaviour(new AdvancedMovement());
		
		_enemies.Add(mobileMeeleEnemy);
		_enemies.Add(mobileShootingEnemy);
		_enemies.Add(stationaryShootingEnemy);
		_enemies.Add(mobileShootingEnemy);
		_enemies.Add(stationaryShootingEnemy);

		var clonedEnemy = mobileMeeleEnemy.DeepClone();

		_enemies.Add(clonedEnemy);
	}

	public async Task<List<Enemy>> ListAsync()
	{
		return _enemies;
	}
}

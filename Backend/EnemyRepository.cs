using Backend.CreationalPatterns;
using Backend.Iterator;
using Shared;

namespace Backend;

public class EnemyRepository
{
	private readonly List<Enemy> _enemies = [];
	private readonly EnemyIteratorRepository _enemyIteratorRepository = new EnemyIteratorRepository("list");
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
		
		_enemyIteratorRepository.GetIterator().Add(mobileMeeleEnemy);
		//_enemyIteratorRepository.GetIterator().Add(stationaryShootingEnemy);
		//_enemyIteratorRepository.GetIterator().Add(mobileShootingEnemy);
		//_enemyIteratorRepository.GetIterator().Add(stationaryShootingEnemy);
        _enemyIteratorRepository.GetIterator().Add(mobileShootingEnemy);

		//var clonedEnemy = mobileMeeleEnemy.DeepClone();

		//_enemyIteratorRepository.GetIterator().Add(clonedEnemy);
	}

    public async Task<List<Enemy>> ListAsync()
    {
        var iterator = _enemyIteratorRepository.GetIterator();
        var enemyList = new List<Enemy>();
		iterator.Start();

        while (iterator.HasNext())
        {
            enemyList.Add(iterator.Next());
        }

        return await Task.FromResult(enemyList);
    }

	public void Remove(Enemy enemy)
	{
		_enemyIteratorRepository.Remove(enemy);
	}
}

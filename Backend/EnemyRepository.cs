using Backend.CreationalPatterns;
using Backend.Iterator;
using Shared;

namespace Backend;

public interface IEnemyRepository
{
	public Task<List<Enemy>> ListAsync();
	public void Remove(Enemy enemy);
	public void Add(int id, int x, int y, string moveType, string attackType);
}

public class EnemyRepositoryProxy : IEnemyRepository
{
	private EnemyRepository _enemyRepository;
	private readonly EnemyAbstractFactory _stationaryEnemyFactory = new StationaryEnemyFactory();
	private readonly EnemyAbstractFactory _mobileEnemyFactory = new MobileEnemyFactory();

	public void Add(int id, int x, int y, string moveType, string attackType)
	{
		if (_enemyRepository is null)
			_enemyRepository = new EnemyRepository();

		EnemyAbstractFactory factory = moveType switch
		{
			"mobile" => _mobileEnemyFactory,
			"stationary" => _stationaryEnemyFactory,
			_ => throw new ArgumentException("Unknown movement type")
		};

		_enemyRepository.SetFactory(factory);
		_enemyRepository.Add(id, x, y, null, attackType);
	}

	public async Task<List<Enemy>> ListAsync()
	{
		if (_enemyRepository is null)
			_enemyRepository = new EnemyRepository();

		return await _enemyRepository.ListAsync();
	}

	public void Remove(Enemy enemy)
	{
		if (_enemyRepository is null)
			_enemyRepository = new EnemyRepository();

		_enemyRepository.Remove(enemy);
	}
}

public class EnemyRepository : IEnemyRepository
{
	private readonly EnemyIteratorRepository _enemyIteratorRepository = new EnemyIteratorRepository("list");
	private EnemyAbstractFactory _enemyFactory;

	public void SetFactory(EnemyAbstractFactory factory)
	{
		_enemyFactory = factory;
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

	public void Add(int id, int x, int y, string moveType, string attackType)
	{
		Enemy enemy = attackType switch
		{
			"meele" => _enemyFactory.CreateMeeleEnemy(x, y, 100, id),
			"explosive" => _enemyFactory.CreateExplosiveEnemy(x, y, 100, id),
			"shooting" => _enemyFactory.CreateShootingEnemy(x, y, 100, id),
			_ => throw new ArgumentException("Unknow attack type")
		};

		if(id % 2 == 0)
			enemy.SetMovementBehaviour(new SimpleMovement());
		else
			enemy.SetMovementBehaviour(new AdvancedMovement());

		_enemyIteratorRepository.GetIterator().Add(enemy);
	}

	public void Remove(Enemy enemy)
	{
		_enemyIteratorRepository.Remove(enemy);
	}
}

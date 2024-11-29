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

        _enemyIteratorRepository.Add(mobileMeeleEnemy);
        _enemyIteratorRepository.Add(stationaryShootingEnemy);
        _enemyIteratorRepository.Add(mobileShootingEnemy);
        _enemyIteratorRepository.Add(stationaryShootingEnemy);
        _enemyIteratorRepository.Add(mobileShootingEnemy);

        var clonedEnemy = mobileMeeleEnemy.DeepClone();

        _enemyIteratorRepository.Add(clonedEnemy);
    }

    public async Task<List<Enemy>> ListAsync()
    {
        var iterator = _enemyIteratorRepository.GetIterator();

        var enemyList = new List<Enemy>();

        while (iterator.HasNext())
        {
            Console.WriteLine(iterator.Next().X);
            Console.WriteLine(iterator.Next().Y);
            Console.WriteLine(iterator.Next().Id);
            enemyList.Add(iterator.Next());
        }

        return await Task.FromResult(enemyList);
    }
}

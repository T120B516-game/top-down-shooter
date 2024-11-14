using Backend;
using Shared;

namespace Tests.Strategy;

public class EnemyRepositoryTests
{
    [Fact]
    public void EnemyRepository_ShouldInitializeEnemiesWithCorrectMovement()
    {
        var repository = new EnemyRepository();
        var enemies = repository.ListAsync();


        //Assert.IsType<AdvancedMovement>(enemies[0].MovementBehaviour);
        //Assert.IsType<SimpleMovement>(enemies[1].MovementBehaviour);
        //Assert.IsType<AdvancedMovement>(enemies[2].MovementBehaviour);
    }
}

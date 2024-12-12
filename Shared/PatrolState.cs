using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class PatrolState : IEnemyState
    {
        public void EnterState(Enemy enemy)
        {
            //Console.WriteLine($"{enemy.Id} entering Patrol state.");
            enemy.SetMovementBehaviour(new SimpleMovement());
        }

        public void UpdateState(Enemy enemy, List<Player> players)
        {
            // Patrol logic (e.g., move randomly)
            enemy.PerformMovement(players);

            // Transition to Chase if a player is nearby
            var closestPlayer = players.OrderBy(p => CalculateDistance(p, enemy)).FirstOrDefault();
            if (closestPlayer != null && CalculateDistance(closestPlayer, enemy) < 10)
            {
                enemy.TransitionToState(new ChaseState());
            }
        }

        public void ExitState(Enemy enemy)
        {
            //Console.WriteLine($"{enemy.Id} exiting Patrol state.");
        }

        private double CalculateDistance(Player player, Enemy enemy)
        {
            return Math.Sqrt(Math.Pow(player.X - enemy.X, 2) + Math.Pow(player.Y - enemy.Y, 2));
        }
    }
}

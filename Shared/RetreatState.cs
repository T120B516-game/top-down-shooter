using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class RetreatState : IEnemyState
    {
        public void EnterState(Enemy enemy)
        {
            //Console.WriteLine($"{enemy.Id} entering Retreat state.");
            enemy.SetMovementBehaviour(new SimpleMovement());
        }

        public void UpdateState(Enemy enemy, List<Player> players)
        {
            // Retreat logic (e.g., move away from players)

            // Transition to Patrol when far from all players
            var closestPlayer = players.OrderBy(p => CalculateDistance(p, enemy)).FirstOrDefault();
            if (closestPlayer == null || CalculateDistance(closestPlayer, enemy) > 15)
            {
                enemy.TransitionToState(new PatrolState());
            }
        }

        public void ExitState(Enemy enemy)
        {
            //Console.WriteLine($"{enemy.Id} exiting Retreat state.");
        }

        private double CalculateDistance(Player player, Enemy enemy)
        {
            return Math.Sqrt(Math.Pow(player.X - enemy.X, 2) + Math.Pow(player.Y - enemy.Y, 2));
        }
    }
}

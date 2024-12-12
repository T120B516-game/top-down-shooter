using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class ChaseState : IEnemyState
    {
        public void EnterState(Enemy enemy)
        {
            //Console.WriteLine($"{enemy.Id} entering Chase state.");
            enemy.SetMovementBehaviour(new AdvancedMovement());
        }

        public void UpdateState(Enemy enemy, List<Player> players)
        {
            // Chase logic
            enemy.PerformMovement(players);

            // Transition to Attack if close to the player
            var closestPlayer = players.OrderBy(p => CalculateDistance(p, enemy)).FirstOrDefault();
            if (closestPlayer != null && CalculateDistance(closestPlayer, enemy) < 2)
            {
                enemy.TransitionToState(new AttackState());
            }
        }

        public void ExitState(Enemy enemy)
        {
           // Console.WriteLine($"{enemy.Id} exiting Chase state.");
        }

        private double CalculateDistance(Player player, Enemy enemy)
        {
            return Math.Sqrt(Math.Pow(player.X - enemy.X, 2) + Math.Pow(player.Y - enemy.Y, 2));
        }
    }

}

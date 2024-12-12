using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Shared
{
    public static class EnemyAI
    {
        public static void HandleStateTransitions(Enemy enemy, List<Player> players)
        {
            var closestPlayer = players
                .OrderBy(p => Math.Sqrt(Math.Pow(p.X - enemy.X, 2) + Math.Pow(p.Y - enemy.Y, 2)))
                .FirstOrDefault();

            if (enemy.Health <= 0)
            {
                // Transition to a "dead" state (not yet implemented)
                enemy.SetState(new PatrolState());
            }
            else if (enemy.Health < 20)
            {
                // Transition to "retreat" or "idle" state if health is low
                enemy.SetState(new RetreatState());
            }
            else if(closestPlayer != null)
            {
                enemy.SetState(new ChaseState());
            }
            else if (closestPlayer != null && IsPlayerInRange(enemy, closestPlayer))
            {
                // Transition to "attacking" state if the player is nearby
                enemy.SetState(new AttackState());
            }
            else
            {
                // Default to "idle" or "patrolling" state
                enemy.SetState(new PatrolState());
            }
        }

        private static bool IsPlayerInRange(Enemy enemy, Player player)
        {
            const int attackRange = 2; // Example attack range
            return Math.Abs(player.X - enemy.X) <= attackRange &&
                   Math.Abs(player.Y - enemy.Y) <= attackRange;
        }
    }

}

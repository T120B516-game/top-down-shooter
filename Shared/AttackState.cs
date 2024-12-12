using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class AttackState : IEnemyState
    {
        public void EnterState(Enemy enemy)
        {
            Console.WriteLine($"{enemy.Id} entering Attack state.");
        }

        public void UpdateState(Enemy enemy, List<Player> players)
        {
            // Attack logic (e.g., decrease player health)

            // Transition to Retreat if health is low
            if (enemy.Health < 10)
            {
                enemy.TransitionToState(new RetreatState());
            }
        }

        public void ExitState(Enemy enemy)
        {
            Console.WriteLine($"{enemy.Id} exiting Attack state.");
        }
    }

}

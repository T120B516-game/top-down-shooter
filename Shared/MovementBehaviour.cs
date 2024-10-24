using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public interface IMovementBehaviour
    {
        void Move(Enemy enemy, List<Player> players);
    }

    public class SimpleMovement : IMovementBehaviour
    {
        private Random _random = new Random();

        public void Move(Enemy enemy, List<Player> players)
        {
            enemy.X += _random.Next(-1, 2);
            enemy.Y += _random.Next(-1, 2);
        }
    }

    public class AdvancedMovement : IMovementBehaviour
    {
        public void Move(Enemy enemy, List<Player> players)
        {
            Player closestPlayer = players.OrderBy(p => Math.Sqrt(Math.Pow(p.X - enemy.X, 2) + Math.Pow(p.Y - enemy.Y, 2)))
                                          .FirstOrDefault();
            if (closestPlayer != null)
            {
                if (enemy.X < closestPlayer.X) enemy.X++;
                if (enemy.X > closestPlayer.X) enemy.X--;
                if (enemy.Y < closestPlayer.Y) enemy.Y++;
                if (enemy.Y > closestPlayer.Y) enemy.Y--;
            
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class InteractionVisitor : IInteractionVisitor
    {
        private readonly Player _player;

        public InteractionVisitor(Player player)
        {
            _player = player;
        }

        public void Visit(Player player)
        {
            // Leave this empty or handle player-to-player interactions if necessary
        }

        public void Visit(Obstacle obstacle)
        {
            if (_player.IsCollidingWith(obstacle))
            {
                if (obstacle is Penetratable)
                {
                    Console.WriteLine($"Player {_player.Id} collided with a penetratable obstacle!");
                }
                else if (obstacle is Unpenetratable)
                {
                    Console.WriteLine($"Player {_player.Id} collided with an unpenetratable obstacle!");
                }
            }
        }
    }

}

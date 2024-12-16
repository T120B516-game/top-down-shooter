using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class LoggingVisitor : IPlayerVisitor
    {
        public void Visit(Player player)
        {
            Console.WriteLine($"Player {player.Id} -> Health: {player.Health}, Position: ({player.X}, {player.Y})");
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class BuffVisitor : IPlayerVisitor
    {
        private readonly int _healthIncrease;
        private readonly int _speedIncrease;

        public BuffVisitor(int healthIncrease, int speedIncrease)
        {
            _healthIncrease = healthIncrease;
            _speedIncrease = speedIncrease;
        }

        public void Visit(Player player)
        {
            player.Health += _healthIncrease;
            player.Speed += _speedIncrease;
            Console.WriteLine($"Buffed player {player.Id}: +{_healthIncrease} Health, +{_speedIncrease} Speed");
        }
    }

}

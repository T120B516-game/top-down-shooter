using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public interface IEnemyState
    {
        void EnterState(Enemy enemy);
        void UpdateState(Enemy enemy, List<Player> players);
        void ExitState(Enemy enemy);
    }

}

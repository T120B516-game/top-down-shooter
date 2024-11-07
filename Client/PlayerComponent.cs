using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Client
{
    public interface PlayerComponent
    {   
        int Id { get; }
        int X { get; }
        int Y { get; }
        void Draw(Graphics g);
    }
}

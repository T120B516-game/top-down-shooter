using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Entities
{
    public class Player
    {
        public int Id;
        public int Horizontal;
        public int Vertical;
        public string Image;
        public int HP;

        public Player(int id, int horizontal, int vertical, string image, int hP)
        {
            Id = id;
            Horizontal = horizontal;
            Vertical = vertical;
            Image = image;
            HP = hP;
        }
    }
}

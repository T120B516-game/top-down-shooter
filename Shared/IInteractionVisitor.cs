﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public interface IInteractionVisitor
    {
        void Visit(Player player);
        void Visit(Obstacle obstacle);
    }

}
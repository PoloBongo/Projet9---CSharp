using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerGame
{
    public class Player
    {
        public int LocalX { get; set; }
        public int LocalY { get; set; }
        public int WorldX { get; set; }
        public int WorldY { get; set; }

        public Player(int worldX, int worldY, int localX, int localY)
        {
            WorldX = worldX;
            WorldY = worldY;
            LocalX = localX;
            LocalY = localY;
        }
    }
}

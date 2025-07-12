using System.Collections.Generic;

namespace GameRuntime.Room
{
    public class RoomGenerateNode
    {
        public int x;
        public int y;
        public RoomNode node;
        public HashSet<RoomGenerateDir> doorOpenDir;
    }
}
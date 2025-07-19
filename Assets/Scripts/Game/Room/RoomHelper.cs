using System.Collections.Generic;
using System.Linq;
using QFramework;

namespace GameRuntime.Room
{
    public class RoomHelper
    {
        public static List<RoomGenerateDir> GetCanSelects(DynaGrid<RoomGenerateNode> grid, int x, int y)
        {
            List<RoomGenerateDir> dirList = new List<RoomGenerateDir>();
            if (grid[x + 1, y] == null)
            {
                dirList.Add(RoomGenerateDir.Right);
            }
            if (grid[x - 1, y] == null)
            {
                dirList.Add(RoomGenerateDir.Left);
            }
            if (grid[x, y + 1] == null)
            {
                dirList.Add(RoomGenerateDir.Up);
            }
            if (grid[x, y - 1] == null)
            {
                dirList.Add(RoomGenerateDir.Down);
            }

            return dirList;
        }

        public class GenerateDirWeight
        {
            public int weight;
            public RoomGenerateDir roomGenerateDir;
        }
        public static List<GenerateDirWeight> GetCanSelectsSorting(DynaGrid<RoomGenerateNode> grid, RoomGenerateNode roomGenerateNode)
        {
            List<RoomGenerateDir> roomGenerateDirs = GetCanSelects(grid, roomGenerateNode.x, roomGenerateNode.y);
            List<GenerateDirWeight> dirWeightList = new List<GenerateDirWeight>();
            foreach (var roomGenerateDir in roomGenerateDirs)
            {
                if (roomGenerateDir == RoomGenerateDir.Left)
                {
                    List<RoomGenerateDir> UproomGenerateDirs = GetCanSelects(grid, roomGenerateNode.x - 1, roomGenerateNode.y);
                    dirWeightList.Add(new GenerateDirWeight() { weight = UproomGenerateDirs.Count, roomGenerateDir = roomGenerateDir });
                }
                else if (roomGenerateDir == RoomGenerateDir.Right)
                {
                    List<RoomGenerateDir> UproomGenerateDirs = GetCanSelects(grid, roomGenerateNode.x + 1, roomGenerateNode.y);
                    dirWeightList.Add(new GenerateDirWeight() { weight = UproomGenerateDirs.Count, roomGenerateDir = roomGenerateDir });
                }
                else if (roomGenerateDir == RoomGenerateDir.Up)
                {
                    List<RoomGenerateDir> UproomGenerateDirs = GetCanSelects(grid, roomGenerateNode.x, roomGenerateNode.y + 1);
                    dirWeightList.Add(new GenerateDirWeight() { weight = UproomGenerateDirs.Count, roomGenerateDir = roomGenerateDir });
                }
                else if (roomGenerateDir == RoomGenerateDir.Down)
                {
                    List<RoomGenerateDir> UproomGenerateDirs = GetCanSelects(grid, roomGenerateNode.x, roomGenerateNode.y - 1);
                    dirWeightList.Add(new GenerateDirWeight() { weight = UproomGenerateDirs.Count, roomGenerateDir = roomGenerateDir });
                }

            }
            dirWeightList = dirWeightList.FindAll(item => item.weight != 0).OrderBy(x => x.weight).Reverse().ToList();
            return dirWeightList;
        }

    }
}
using System.Collections.Generic;
using Markdig.Renderers.Html;
using QFramework;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GameRuntime.Room
{
    public partial class LevelController : ViewController
    {

        private static LevelController instance;
        public static LevelController Instance => instance;

        public List<TileBase> wallTiles;
        public List<TileBase> floorTiles;
        public Tilemap wallTileMap;
        public Tilemap floorTileMap;

        public GameObject playerPrefab;
        public GameObject enemyPrefab;
        public GameObject finishPrefab;
        public GameObject doorPrefab;



        private TileBase wallTile
        {
            get { return wallTiles[Random.Range(0, 4)]; }
        }
        private TileBase floorTile
        {
            get { return floorTiles[Random.Range(0, 4)]; }
        }

        private void Awake()
        {
            instance = this;
        }

        void OnDestroy()
        {
            instance = null;
        }


        void Start()
        {
            RoomNode roomNode = new RoomNode();
            roomNode.AddChildren(RoomType.Battle)
                .AddChildren(RoomType.Chest)
                .AddChildren(RoomType.Battle)
                .AddChildren(RoomType.Battle)
                .AddChildren(RoomType.Chest)
                .AddChildren(RoomType.Battle)
                .AddChildren(RoomType.Battle)
                .AddChildren(RoomType.Finish);

            DynaGrid<RoomGenerateNode> dynaGrid = GenerateRoomNodeBFS(roomNode);
            var offset = Vector2.zero;
            dynaGrid.ForEach((x, y, generateNode) =>
            {
                GenerateRoomNode(generateNode, new Vector2(x, y));
            });
        }




        private DynaGrid<RoomGenerateNode> GenerateRoomNodeBFS(RoomNode roomNode)
        {
            Queue<RoomGenerateNode> queue = new Queue<RoomGenerateNode>();
            DynaGrid<RoomGenerateNode> grid = new DynaGrid<RoomGenerateNode>();
            queue.Enqueue(new RoomGenerateNode()
            {
                x = 0,
                y = 0,
                node = roomNode,
                doorOpenDir = new HashSet<RoomGenerateDir>()
            });

            while (queue.Count > 0)
            {
                RoomGenerateNode roomGenerateNode = queue.Dequeue();
                grid[roomGenerateNode.x, roomGenerateNode.y] = roomGenerateNode;

                List<RoomGenerateDir> dirList = new List<RoomGenerateDir>();
                if (grid[roomGenerateNode.x + 1, roomGenerateNode.y] == null)
                {
                    dirList.Add(RoomGenerateDir.Right);
                }
                if (grid[roomGenerateNode.x - 1, roomGenerateNode.y] == null)
                {
                    dirList.Add(RoomGenerateDir.Left);
                }
                if (grid[roomGenerateNode.x, roomGenerateNode.y + 1] == null)
                {
                    dirList.Add(RoomGenerateDir.Up);
                }
                if (grid[roomGenerateNode.x, roomGenerateNode.y - 1] == null)
                {
                    dirList.Add(RoomGenerateDir.Down);
                }

                foreach (RoomNode children in roomGenerateNode.node.children)
                {
                    RoomGenerateDir roomGenerateDir = dirList.GetRandomItem();
                    if (roomGenerateDir == RoomGenerateDir.Right)
                    {
                        roomGenerateNode.doorOpenDir.Add(roomGenerateDir);
                        queue.Enqueue(new RoomGenerateNode()
                        {
                            x = roomGenerateNode.x + 1,
                            y = roomGenerateNode.y,
                            node = children,
                            doorOpenDir = new HashSet<RoomGenerateDir>()
                            {
                                RoomGenerateDir.Left
                            }
                        });
                    }
                    else if (roomGenerateDir == RoomGenerateDir.Left)
                    {
                        roomGenerateNode.doorOpenDir.Add(roomGenerateDir);
                        queue.Enqueue(new RoomGenerateNode()
                        {
                            x = roomGenerateNode.x - 1,
                            y = roomGenerateNode.y,
                            node = children,
                            doorOpenDir = new HashSet<RoomGenerateDir>()
                            {
                                RoomGenerateDir.Right
                            }
                        });

                    }
                    else if (roomGenerateDir == RoomGenerateDir.Up)
                    {
                        roomGenerateNode.doorOpenDir.Add(roomGenerateDir);
                        queue.Enqueue(new RoomGenerateNode()
                        {
                            x = roomGenerateNode.x,
                            y = roomGenerateNode.y + 1,
                            node = children,
                            doorOpenDir = new HashSet<RoomGenerateDir>()
                            {
                                RoomGenerateDir.Down
                            }
                        });

                    }
                    else if (roomGenerateDir == RoomGenerateDir.Down)
                    {
                        roomGenerateNode.doorOpenDir.Add(roomGenerateDir);
                        queue.Enqueue(new RoomGenerateNode()
                        {
                            x = roomGenerateNode.x,
                            y = roomGenerateNode.y - 1,
                            node = children,
                            doorOpenDir = new HashSet<RoomGenerateDir>()
                            {
                                RoomGenerateDir.Up
                            }
                        });

                    }
                }

            }
            return grid;
        }

        private Vector2 GenerateRoomNode(RoomGenerateNode roomNode, Vector2 MapPos)
        {
            if (roomNode.node.roomType == RoomType.Normal)
            {
                GenerateRoom(MapPos, Config.startCfg, roomNode);
            }
            else if (roomNode.node.roomType == RoomType.Battle)
            {
                GenerateRoom(MapPos, Config.normalCfg.GetRandomItem(), roomNode);
            }
            else if (roomNode.node.roomType == RoomType.Finish)
            {
                GenerateRoom(MapPos, Config.finishCfg, roomNode);
            }
            else if (roomNode.node.roomType == RoomType.Chest)
            {
                GenerateRoom(MapPos, Config.chestCfg, roomNode);
            }

            return MapPos;
        }

        private Vector2 GeneratePassage(Vector2 offset, RoomConfig roomConfig, int passageLength)
        {
            int roomHeight = roomConfig.RoomHeight;
            float PosX = offset.x;
            float PosY = roomHeight / 2 - offset.y;
            for (int i = 0; i < passageLength; i++)
            {
                wallTileMap.SetTile(new Vector3Int((int)PosX + i, (int)PosY - 1, 0), wallTile);
                floorTileMap.SetTile(new Vector3Int((int)PosX + i, (int)PosY, 0), floorTile);
                wallTileMap.SetTile(new Vector3Int((int)PosX + i, (int)PosY + 1, 0), wallTile);
            }
            offset += passageLength * Vector2.right;
            return offset;
        }

        /// <summary>
        /// O 表示墙壁
        /// @ 表示玩家
        /// # 表示敌人
        /// F 表示通过点
        /// D 表示门
        /// C 表示宝箱
        /// </summary>
        /// <param name="MapPos"></param>
        /// <param name="roomConfig"></param>
        /// <param name="roomNode"></param>
        public void GenerateRoom(Vector2 MapPos, RoomConfig roomConfig, RoomGenerateNode roomNode)
        {
            List<string> roomCfg = roomConfig.RoomLines;
            float roomWidth = roomConfig.RoomWidth;
            float roomHeight = roomConfig.RoomHeight;

            float roomPosX = roomWidth / 2 + (MapPos.x * roomWidth) + (2 * MapPos.x);
            float roomPosY = roomHeight / 2 + ((MapPos.y - 1) * roomHeight) + (2 * MapPos.y);

            Room room = Object.Instantiate(Room)
                            .WithRoomConfig(roomConfig)
                            .Position2D(roomPosX, roomPosY)
                            .Show();
            BoxCollider2D boxCollider2D = room.GetComponent<BoxCollider2D>();
            boxCollider2D.size = new Vector2(roomWidth - 2, roomHeight - 2);
            for (int y = 0; y < roomHeight; y++)
            {
                for (int x = 0; x < roomWidth; x++)
                {
                    int map_y = (Mathf.FloorToInt(MapPos.y) * roomConfig.RoomHeight) - 1 - y + (2 * (int)MapPos.y); // Unity的Tilemap坐标系Y轴向上为正
                    int map_x = x + (Mathf.FloorToInt(MapPos.x) * roomConfig.RoomWidth) + (2 * (int)MapPos.x);
                    floorTileMap.SetTile(new Vector3Int(map_x, map_y, 0), floorTile);
                    if (roomCfg[y][x] == '0')
                    {
                        wallTileMap.SetTile(new Vector3Int(map_x, map_y, 0), wallTile);
                    }
                    else if (roomCfg[y][x] == '@')
                    {
                        // 处理玩家
                        Vector3 playerPosition = new Vector3(map_x + 0.5f, map_y + 0.5f, 0);
                        GameObject player = Instantiate(playerPrefab);
                        player.transform.position = playerPosition;
                        player.gameObject.SetActive(true); // 确保玩家对象处于激活状态
                        Global.Player = player; // 将玩家对象存储到全局变量中
                    }
                    else if (roomCfg[y][x] == '#')
                    {
                        // 处理敌人
                        Vector3 enemyPosition = new Vector3(map_x + 0.5f, map_y + 0.5f, 0);
                        room.AddEnemyGeneratedPos(enemyPosition);
                        // GameObject enemy = Instantiate(enemyPrefab);
                        // enemy.transform.position = enemyPosition;
                        // enemy.gameObject.SetActive(true); // 确保敌人对象处于激活状态
                        // Global.Enmpy = enemy; // 将敌人对象存储到全局变量中
                    }
                    else if (roomCfg[y][x] == 'F')
                    {
                        // 处理通过点
                        Vector3 finishPosition = new Vector3(map_x + 0.5f, map_y + 0.5f, 0);
                        GameObject finish = Instantiate(finishPrefab);
                        finish.transform.position = finishPosition;
                        finish.gameObject.SetActive(true); // 确保通过点对象处于激活状态
                    }
                    else if (roomCfg[y][x] == 'D')
                    {
                        if (x == 0 && roomNode.doorOpenDir.Contains(RoomGenerateDir.Left))
                        {
                            GameObject door = Instantiate(doorPrefab)
                                                                .Position2D(map_x + 0.5f, map_y + 0.5f)
                                                                .Hide();
                            room.AddDoor(door);
                        }
                        else if (x == roomWidth - 1 && roomNode.doorOpenDir.Contains(RoomGenerateDir.Right))
                        {
                            GameObject door = Instantiate(doorPrefab)
                                                                .Position2D(map_x + 0.5f, map_y + 0.5f)
                                                                .Hide();
                            room.AddDoor(door);
                        }
                        else if (y == 0 && roomNode.doorOpenDir.Contains(RoomGenerateDir.Up))
                        {
                            GameObject door = Instantiate(doorPrefab)
                                                                .Position2D(map_x + 0.5f, map_y + 0.5f)
                                                                .Hide();
                            room.AddDoor(door);
                        }
                        else if (y == roomHeight - 1 && roomNode.doorOpenDir.Contains(RoomGenerateDir.Down))
                        {
                            GameObject door = Instantiate(doorPrefab)
                                                                .Position2D(map_x + 0.5f, map_y + 0.5f)
                                                                .Hide();
                            room.AddDoor(door);
                        }
                        else
                        {
                            wallTileMap.SetTile(new Vector3Int(map_x, map_y, 0), wallTile);

                        }

                    }
                    else if (roomCfg[y][x] == 'C')
                    {
                        Instantiate(Chest.gameObject)
                                    .Position2D(map_x + 0.5f, map_y + 0.5f)
                                    .Show();
                    }
                }
            }
        }

    }
}

using System.Collections.Generic;
using System.Linq;
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
            roomNode
                 .AddChildren(RoomType.Chest)
                .AddChildren(RoomType.Battle, (roomNode) =>
                {
                    roomNode
                    .AddChildren(RoomType.Battle)
                    .AddChildren(RoomType.Chest)
                    .AddChildren(RoomType.Battle)
                    .AddChildren(RoomType.Battle);
                })
                .AddChildren(RoomType.Battle, (roomNode) =>
                {
                    roomNode
                    .AddChildren(RoomType.Battle)
                    .AddChildren(RoomType.Battle)
                    .AddChildren(RoomType.Chest)
                    .AddChildren(RoomType.Battle)
                    .AddChildren(RoomType.Battle);
                })
                 .AddChildren(RoomType.Chest)
                 .AddChildren(RoomType.Chest)
                 .AddChildren(RoomType.Chest)
                 .AddChildren(RoomType.Chest)
                // .AddChildren(RoomType.Battle)
                .AddChildren(RoomType.Chest)
                .AddChildren(RoomType.Finish);

            DynaGrid<Room> roomGrid = new DynaGrid<Room>();
            int optimal = 0;
            DynaGrid<RoomGenerateNode> dynaGrid = new DynaGrid<RoomGenerateNode>();
            while (!GenerateRoomNodeBFS(roomNode, optimal++, dynaGrid))
            {
                dynaGrid.Clear();
            }
            var offset = Vector2.zero;
            dynaGrid.ForEach((x, y, generateNode) =>
            {
                Room room = GenerateRoomNode(generateNode, new Vector2(x, y));
                roomGrid[x, y] = room;
            });
            GeneratePassage(roomGrid);
        }




        private bool GenerateRoomNodeBFS(RoomNode roomNode, int optimal, DynaGrid<RoomGenerateNode> grid)
        {

            Queue<RoomGenerateNode> queue = new Queue<RoomGenerateNode>();
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

                var dirList = RoomHelper.GetCanSelectsSorting(grid, roomGenerateNode);

                if (dirList.Count < roomGenerateNode.node.children.Count)
                {
                    Debug.Log($"2候选列表{dirList.Count}，子节点数量{roomGenerateNode.node.children.Count}");
                    Debug.LogWarning("房间位置冲突");
                    return false;
                }

                foreach (RoomNode children in roomGenerateNode.node.children)
                {
                    RoomGenerateDir roomGenerateDir = dirList.First().roomGenerateDir;
                    if (Random.Range(0, 100) < optimal)
                    {
                        dirList.RemoveAt(0);
                    }
                    else
                    {
                        roomGenerateDir = dirList.GetRandomItem().roomGenerateDir;
                        dirList.RemoveAll(item => item.roomGenerateDir == roomGenerateDir);
                    }

                    if (roomGenerateDir == RoomGenerateDir.Right)
                    {
                        roomGenerateNode.doorOpenDir.Add(roomGenerateDir);
                        RoomGenerateNode newNode = new RoomGenerateNode()
                        {
                            x = roomGenerateNode.x + 1,
                            y = roomGenerateNode.y,
                            node = children,
                            doorOpenDir = new HashSet<RoomGenerateDir>()
                            {
                                RoomGenerateDir.Left
                            }
                        };
                        queue.Enqueue(newNode);
                        grid[newNode.x, newNode.y] = newNode;
                    }
                    else if (roomGenerateDir == RoomGenerateDir.Left)
                    {
                        roomGenerateNode.doorOpenDir.Add(roomGenerateDir);
                        RoomGenerateNode newNode = new RoomGenerateNode()
                        {
                            x = roomGenerateNode.x - 1,
                            y = roomGenerateNode.y,
                            node = children,
                            doorOpenDir = new HashSet<RoomGenerateDir>()
                            {
                                RoomGenerateDir.Right
                            }
                        };
                        queue.Enqueue(newNode);
                        grid[newNode.x, newNode.y] = newNode;
                    }
                    else if (roomGenerateDir == RoomGenerateDir.Up)
                    {
                        roomGenerateNode.doorOpenDir.Add(roomGenerateDir);
                        RoomGenerateNode newNode = new RoomGenerateNode()
                        {
                            x = roomGenerateNode.x,
                            y = roomGenerateNode.y + 1,
                            node = children,
                            doorOpenDir = new HashSet<RoomGenerateDir>()
                            {
                                RoomGenerateDir.Down
                            }
                        };
                        queue.Enqueue(newNode);
                        grid[newNode.x, newNode.y] = newNode;
                    }
                    else if (roomGenerateDir == RoomGenerateDir.Down)
                    {
                        roomGenerateNode.doorOpenDir.Add(roomGenerateDir);
                        RoomGenerateNode newNode = new RoomGenerateNode()
                        {
                            x = roomGenerateNode.x,
                            y = roomGenerateNode.y - 1,
                            node = children,
                            doorOpenDir = new HashSet<RoomGenerateDir>()
                            {
                                RoomGenerateDir.Up
                            }
                        };
                        queue.Enqueue(newNode);
                        grid[newNode.x, newNode.y] = newNode;
                    }
                }

            }
            return true;
        }


        private Room GenerateRoomNode(RoomGenerateNode roomNode, Vector2 MapPos)
        {
            if (roomNode.node.roomType == RoomType.Normal)
            {
                return GenerateRoom(MapPos, Config.startCfg, roomNode);
            }
            else if (roomNode.node.roomType == RoomType.Battle)
            {
                return GenerateRoom(MapPos, Config.normalCfg.GetRandomItem(), roomNode);
            }
            else if (roomNode.node.roomType == RoomType.Finish)
            {
                return GenerateRoom(MapPos, Config.finishCfg, roomNode);
            }
            else if (roomNode.node.roomType == RoomType.Chest)
            {
                return GenerateRoom(MapPos, Config.chestCfg, roomNode);
            }

            return null;
        }

        private void GeneratePassage(DynaGrid<Room> roomGrid)
        {
            roomGrid.ForEach((x, y, room) =>
            {
                foreach (var doorObj in room.Doors)
                {
                    Door door = doorObj.GetComponent<Door>();
                    if (door.DoorDir == RoomGenerateDir.Left)
                    {
                        Room targetRoom = roomGrid[x - 1, y];
                        Door targetDoor = targetRoom.Doors
                            .Find(d => d.GetComponent<Door>().DoorDir == RoomGenerateDir.Right).GetComponent<Door>();
                        int offset = (int)Mathf.Abs(door.DoorPos.x - targetDoor.DoorPos.x);
                        for (int i = 0; i < offset; i++)
                        {
                            wallTileMap.SetTile(new Vector3Int((int)targetDoor.DoorPos.x + i, (int)door.DoorPos.y + 2, 0), wallTile);
                            floorTileMap.SetTile(new Vector3Int((int)targetDoor.DoorPos.x + i, (int)door.DoorPos.y + 1, 0), floorTile);
                            floorTileMap.SetTile(new Vector3Int((int)targetDoor.DoorPos.x + i, (int)door.DoorPos.y, 0), floorTile);
                            floorTileMap.SetTile(new Vector3Int((int)targetDoor.DoorPos.x + i, (int)door.DoorPos.y - 1, 0), floorTile);
                            wallTileMap.SetTile(new Vector3Int((int)targetDoor.DoorPos.x + i, (int)door.DoorPos.y - 2, 0), wallTile);
                        }
                    }
                    else if (door.DoorDir == RoomGenerateDir.Up)
                    {
                        Room targetRoom = roomGrid[x, y + 1];
                        Door targetDoor = targetRoom.Doors
                            .Find(d => d.GetComponent<Door>().DoorDir == RoomGenerateDir.Down).GetComponent<Door>();
                        int offset = (int)Mathf.Abs(door.DoorPos.y - targetDoor.DoorPos.y);
                        for (int i = 0; i < offset; i++)
                        {
                            wallTileMap.SetTile(new Vector3Int((int)targetDoor.DoorPos.x + 2, (int)door.DoorPos.y + i, 0), wallTile);
                            floorTileMap.SetTile(new Vector3Int((int)targetDoor.DoorPos.x + 1, (int)door.DoorPos.y + i, 0), floorTile);
                            floorTileMap.SetTile(new Vector3Int((int)targetDoor.DoorPos.x, (int)door.DoorPos.y + i), floorTile);
                            floorTileMap.SetTile(new Vector3Int((int)targetDoor.DoorPos.x - 1, (int)door.DoorPos.y + i, 0), floorTile);
                            wallTileMap.SetTile(new Vector3Int((int)targetDoor.DoorPos.x - 2, (int)door.DoorPos.y + i, 0), wallTile);
                        }
                    }

                }
            });
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
        public Room GenerateRoom(Vector2 MapPos, RoomConfig roomConfig, RoomGenerateNode roomNode)
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
                            GameObject door = Instantiate(Door.gameObject)
                                                                .LocalScaleX(3)
                                                                .Position2D(map_x + 0.5f, map_y + 0.5f)
                                                                .Show();
                            door.LocalRotation(Quaternion.Euler(new Vector3(0, 0, 90)));
                            Door doorCs = door.GetComponent<Door>();
                            doorCs.DoorDir = RoomGenerateDir.Left;
                            doorCs.DoorPos = new Vector2(map_x, map_y);
                            ActionKit.DelayFrame(1, () =>
                            {
                                wallTileMap.SetTile(new Vector3Int(map_x, map_y + 1, 0), null);
                                wallTileMap.SetTile(new Vector3Int(map_x, map_y - 1, 0), null);
                            }).StartCurrentScene();
                            room.AddDoor(doorCs);
                        }
                        else if (x == roomWidth - 1 && roomNode.doorOpenDir.Contains(RoomGenerateDir.Right))
                        {
                            GameObject door = Instantiate(Door.gameObject)
                                                                .LocalScaleX(3)
                                                                .Position2D(map_x + 0.5f, map_y + 0.5f)
                                                                .Show();
                            Door doorCs = door.GetComponent<Door>();
                            doorCs.DoorDir = RoomGenerateDir.Right;
                            doorCs.DoorPos = new Vector2(map_x, map_y);
                            door.LocalRotation(Quaternion.Euler(new Vector3(0, 0, 90)));
                            ActionKit.DelayFrame(1, () =>
                            {
                                wallTileMap.SetTile(new Vector3Int(map_x, map_y + 1, 0), null);
                                wallTileMap.SetTile(new Vector3Int(map_x, map_y - 1, 0), null);
                            }).StartCurrentScene();

                            room.AddDoor(doorCs);
                        }
                        else if (y == 0 && roomNode.doorOpenDir.Contains(RoomGenerateDir.Up))
                        {
                            GameObject door = Instantiate(Door.gameObject)
                                                                .LocalScaleX(3)
                                                                .Position2D(map_x + 0.5f, map_y + 0.5f)
                                                                .Show();
                            Door doorCs = door.GetComponent<Door>();
                            doorCs.DoorDir = RoomGenerateDir.Up;
                            doorCs.DoorPos = new Vector2(map_x, map_y);
                            ActionKit.DelayFrame(1, () =>
                            {
                                wallTileMap.SetTile(new Vector3Int(map_x - 1, map_y, 0), null);
                                wallTileMap.SetTile(new Vector3Int(map_x + 1, map_y, 0), null);
                            }).StartCurrentScene();
                            room.AddDoor(doorCs);
                        }
                        else if (y == roomHeight - 1 && roomNode.doorOpenDir.Contains(RoomGenerateDir.Down))
                        {
                            GameObject door = Instantiate(Door.gameObject)
                                                                .LocalScaleX(3)
                                                                .Position2D(map_x + 0.5f, map_y + 0.5f)
                                                                .Show();
                            Door doorCs = door.GetComponent<Door>();
                            doorCs.DoorDir = RoomGenerateDir.Down;
                            doorCs.DoorPos = new Vector2(map_x, map_y);
                            ActionKit.DelayFrame(1, () =>
                            {
                                wallTileMap.SetTile(new Vector3Int(map_x - 1, map_y, 0), null);
                                wallTileMap.SetTile(new Vector3Int(map_x + 1, map_y, 0), null);
                            }).StartCurrentScene();
                            room.AddDoor(doorCs);
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
            return room;
        }

    }
}

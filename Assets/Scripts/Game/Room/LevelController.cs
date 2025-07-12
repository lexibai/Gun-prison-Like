using System.Collections.Generic;
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

            var offset = Vector2.zero;
            offset = GenerateRoomNode(roomNode, offset);
        }

        private Vector2 GenerateRoomNode(RoomNode roomNode, Vector2 offset)
        {
            if (roomNode.roomType == RoomType.Normal)
            {
                RoomConfig roomConfig = Config.startCfg;
                GenerateRoom(offset, roomConfig);
                offset += (roomConfig.RoomWidth) * Vector2.right;
                offset = GeneratePassage(offset, roomConfig, 3);

            }
            else if (roomNode.roomType == RoomType.Battle)
            {
                GenerateRoom(offset, Config.normalCfg.GetRandomItem());
                offset += (Config.startCfg.RoomWidth) * Vector2.right;
                offset = GeneratePassage(offset, Config.normalCfg.GetRandomItem(), 3);
            }
            else if (roomNode.roomType == RoomType.Finish)
            {
                GenerateRoom(offset, Config.finishCfg);
                offset += (Config.finishCfg.RoomWidth) * Vector2.right;
            }
            else if (roomNode.roomType == RoomType.Chest)
            {
                GenerateRoom(offset, Config.chestCfg);
                offset += (Config.chestCfg.RoomWidth) * Vector2.right;
                offset = GeneratePassage(offset, Config.chestCfg, 3);
            }

            foreach (var children in roomNode.children)
            {
                offset = GenerateRoomNode(children, offset);
            }

            return offset;
        }

        private Vector2 GeneratePassage(Vector2 offset, RoomConfig roomConfig, int passageLength)
        {
            int roomHeight = roomConfig.RoomHeight;
            float PosX = offset.x;
            float PosY = roomHeight / 2 - offset.y;
            for (int i = 0; i < passageLength; i++)
            {
                wallTileMap.SetTile(new Vector3Int((int)PosX + i, (int)PosY - 2, 0), wallTile);
                floorTileMap.SetTile(new Vector3Int((int)PosX + i, (int)PosY - 1, 0), floorTile);
                floorTileMap.SetTile(new Vector3Int((int)PosX + i, (int)PosY, 0), floorTile);
                floorTileMap.SetTile(new Vector3Int((int)PosX + i, (int)PosY + 1, 0), floorTile);
                wallTileMap.SetTile(new Vector3Int((int)PosX + i, (int)PosY + 2, 0), wallTile);
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
        /// <param name="offset">偏移量</param>
        /// <param name="roomCfg">房间配置</param>
        public void GenerateRoom(Vector2 offset, RoomConfig roomConfig)
        {
            List<string> roomCfg = roomConfig.RoomLines;
            float roomWidth = roomConfig.RoomWidth;
            float roomHeight = roomConfig.RoomHeight;

            float roomPosX = roomWidth / 2 + offset.x;
            float roomPosY = roomHeight / 2 - offset.y;

            Room room = Object.Instantiate(Room)
                            .WithRoomConfig(roomConfig)
                            .Position2D(roomPosX, roomPosY)
                            .Show();
            BoxCollider2D boxCollider2D = room.GetComponent<BoxCollider2D>();
            boxCollider2D.size = new Vector2(roomWidth - 2, roomHeight - 2);
            for (int y = 0; y < roomCfg.Count; y++)
            {
                for (int x = 0; x < roomCfg[y].Length; x++)
                {
                    int map_y = roomCfg.Count - 1 - y - Mathf.FloorToInt(offset.y); // Unity的Tilemap坐标系Y轴向上为正
                    int map_x = x + Mathf.FloorToInt(offset.x);
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
                        GameObject door = Instantiate(doorPrefab)
                                    .Position2D(map_x + 0.5f, map_y + 0.5f)
                                    .Hide();
                        room.AddDoor(door);
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

        void Update() { }
    }
}

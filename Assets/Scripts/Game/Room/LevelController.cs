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
            var offset = Vector2.zero;
            GenerateRoom(offset, Config.startCfg);
            offset += (Config.startCfg.RoomWidth) * Vector2.right;
            GenerateRoom(offset, Config.normalCfg.GetRandomItem());
            offset += (Config.startCfg.RoomWidth) * Vector2.right;
            GenerateRoom(offset, Config.normalCfg.GetRandomItem());
            offset += (Config.startCfg.RoomWidth) * Vector2.right;
            GenerateRoom(offset, Config.normalCfg.GetRandomItem());
            offset += (Config.startCfg.RoomWidth) * Vector2.right;
            offset += 4 * Vector2.down;
            GenerateRoom(offset, Config.finishCfg);
        }

        /// <summary>
        /// O 表示墙壁
        /// @ 表示玩家
        /// # 表示敌人
        /// F 表示通过点
        /// D 表示门
        /// </summary>
        /// <param name="offset">偏移量</param>
        /// <param name="roomCfg">房间配置</param>
        public void GenerateRoom(Vector2 offset, RoomConfig roomConfig)
        {
            List<string> roomCfg = roomConfig.RoomLines;
            float roomWidth = roomCfg[0].Length;
            float roomHeight = roomCfg.Count;

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
                }
            }
        }

        void Update() { }
    }
}

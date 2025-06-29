using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Lab
{
    public class LevelController : MonoBehaviour
    {
        public TileBase groutTile;
        public Tilemap tilemap;

        public GameObject playerPrefab;
        public GameObject enemyPrefab;
        public GameObject finishPrefab;


        public List<string> startCfg;
        public List<string> normalCfg;
        public List<string> finishCfg;



        void Start()
        {
            startCfg = new() {
                "0000000000",
                "0        0",
                "0  @     0",
                "0        0",
                "0         ",
                "0         ",
                "0        0",
                "0        0",
                "0        0",
                "0000000000",
            };

            normalCfg = new() {
                "0000000000",
                "0        0",
                "0     #  0",
                "0        0",
                "          ",
                "          ",
                "0        0",
                "0     #  0",
                "0        0",
                "0000000000",
            };

            finishCfg = new() {
                "0000000000",
                "0        0",
                "0        0",
                "0        0",
                "     F   0",
                "         0",
                "0        0",
                "0        0",
                "0        0",
                "0000000000",
            };

            GenerateRoom(Vector2.zero, startCfg);
            GenerateRoom(Vector2.zero + (Vector2.right * (startCfg[0].Length+2)), normalCfg);
            GenerateRoom(Vector2.zero + (Vector2.right * (startCfg[0].Length + normalCfg[0].Length + 4)), finishCfg);



        }


        /// <summary>
        /// O 表示墙壁
        /// @ 表示玩家
        /// # 表示敌人
        /// F 表示通过点
        /// </summary>
        /// <param name="offset">偏移量</param>
        /// <param name="roomCfg">房间配置</param>
        public void GenerateRoom(Vector2 offset ,List<string> roomCfg)
        {
            for (int y = 0; y < roomCfg.Count; y++)
            {
                for (int x = 0; x < roomCfg[y].Length; x++)
                {
                    int map_y = roomCfg.Count - 1 - y - Mathf.FloorToInt(offset.y); // Unity的Tilemap坐标系Y轴向上为正
                    int map_x = x + Mathf.FloorToInt(offset.x);
                    if (roomCfg[y][x] == '0')
                    {
                        tilemap.SetTile(new Vector3Int(map_x, map_y, 0), groutTile);
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
                        GameObject enemy = Instantiate(enemyPrefab);
                        enemy.transform.position = enemyPosition;
                        enemy.gameObject.SetActive(true); // 确保敌人对象处于激活状态
                        Global.Enmpy = enemy; // 将敌人对象存储到全局变量中
                    }
                    else if (roomCfg[y][x] == 'F')
                    {
                        // 处理通过点
                        Vector3 finishPosition = new Vector3(map_x + 0.5f, map_y + 0.5f, 0);
                        GameObject finish = Instantiate(finishPrefab);
                        finish.transform.position = finishPosition;
                        finish.gameObject.SetActive(true); // 确保通过点对象处于激活状态
                    }
                }
            }
        }

        void Update()
        {

        }
    }
}


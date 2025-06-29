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

        /// <summary>
        /// O 表示墙壁
        /// @ 表示玩家
        /// # 表示敌人
        /// </summary>
        public List<string> mapConfig;


        void Start()
        {
            mapConfig = new() {
            "0000000000",
            "0        0",
            "0  @     0",
            "0        0",
            "0        0",
            "0        0",
            "0        0",
            "0     #  0",
            "0        0",
            "0000000000",
        };
            for (int y = 0; y < mapConfig.Count; y++)
            {
                for(int x = 0; x < mapConfig[y].Length; x++)
                {
                    int map_y = mapConfig.Count - 1 - y; // Unity的Tilemap坐标系Y轴向上为正
                    if (mapConfig[y][x] == '0')
                    {
                        tilemap.SetTile(new Vector3Int(x, map_y, 0), groutTile);
                    }
                    else if(mapConfig[y][x] == '@')
                    {
                        // 处理玩家
                        Vector3 playerPosition = new Vector3(x+0.5f, map_y + 0.5f, 0);
                        GameObject player = Instantiate(playerPrefab);
                        player.transform.position = playerPosition;
                        player.gameObject.SetActive(true); // 确保玩家对象处于激活状态
                        Global.Player = player; // 将玩家对象存储到全局变量中
                       
                    }
                    else if (mapConfig[y][x] == '#')
                    {
                        // 处理敌人
                        Vector3 enemyPosition = new Vector3(x + 0.5f, map_y + 0.5f, 0);
                        GameObject enemy = Instantiate(enemyPrefab);
                        enemy.transform.position = enemyPosition;
                        enemy.gameObject.SetActive(true); // 确保敌人对象处于激活状态
                        Global.Enmpy = enemy; // 将敌人对象存储到全局变量中
                    }
                }
            }

        }

        void Update()
        {

        }
    }
}


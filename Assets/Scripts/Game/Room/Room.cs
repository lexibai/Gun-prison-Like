using UnityEngine;
using QFramework;
using System.Collections.Generic;
using GameRuntime.Actor;

namespace GameRuntime.Room
{

	public partial class Room : ViewController
	{

		private enum RoomState
		{
			Ready,
			Battle,
			Over,
		}

		private RoomState state = RoomState.Ready;

		private HashSet<Enemy> enemys = new HashSet<Enemy>();
		private List<Vector3> enemyPos = new List<Vector3>();
		private List<GameObject> doors = new List<GameObject>();

		private RoomConfig roomConfig = new RoomConfig();

		private List<EnemyWaveConfig> enemyWaveConfigs = new List<EnemyWaveConfig>()
		{
			new EnemyWaveConfig(),
			new EnemyWaveConfig(),
			new EnemyWaveConfig()
		};

		public Room WithRoomConfig(RoomConfig roomConfig)
		{
			this.roomConfig = roomConfig;
			return this;
		}


		public void AddEnemyGeneratedPos(Vector3 pos)
		{
			enemyPos.Add(pos);
		}

		public void AddDoor(GameObject door)
		{
			doors.Add(door);
		}


		void Update()
		{
			if (state == RoomState.Battle)
			{
				enemys.RemoveWhere(e => !e);
				if (enemys.Count <= 0)
				{
					if (enemyWaveConfigs.Count <= 0)
					{
						state = RoomState.Over;

					}
					else
					{
						GenerateEnemy();
						enemyWaveConfigs.RemoveAt(0);
					}
				}
			}

			//print(state.ToString());
			if (state == RoomState.Over)
			{
				//print("开始开门");
				for (int i = 0; i < doors.Count; i++)
				{
					print(i);
					var door = doors[i];
					door.Hide();
				}
				//print("开门结束");
			}
		}

		void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.CompareTag("Player"))
			{

				//关闭所有门
				if (roomConfig.roomType == RoomType.Battle)
				{
					if (state == RoomState.Ready)
					{
						state = RoomState.Battle;
						for (int i = 0; i < doors.Count; i++)
						{
							var door = doors[i];
							door.Show();
						}
					}

				}
			}
		}

		private void GenerateEnemy()
		{
			//显示所有敌人
			for (int i = 0; i < enemyPos.Count; i++)
			{
				var pos = enemyPos[i];
				GameObject enemy = Instantiate(LevelController.Instance.enemyPrefab)
				.Position2D(pos.ToVector2())
				.Show();
				enemys.Add(enemy.GetComponent<Enemy>());
			}
		}
	}
}

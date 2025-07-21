using UnityEngine;
using QFramework;
using System.Collections.Generic;
using GameRuntime.Actor;
using System.Linq;

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
		public HashSet<Enemy> Enemys => enemys;
		private List<Vector3> enemyPos = new List<Vector3>();
		public List<Door> Doors = new List<Door>();

		private RoomConfig roomConfig = new RoomConfig();

		private List<EnemyWaveConfig> enemyWaveConfigs = new List<EnemyWaveConfig>();
		void Awake()
		{
			int randomEnemyWaveNum = Random.Range(1, 4);
			for (int i = 0; i < randomEnemyWaveNum; i++)
			{
				enemyWaveConfigs.Add(new EnemyWaveConfig());
			}
		}

		public Room WithRoomConfig(RoomConfig roomConfig)
		{
			this.roomConfig = roomConfig;
			return this;
		}


		public void AddEnemyGeneratedPos(Vector3 pos)
		{
			enemyPos.Add(pos);
		}

		public void AddDoor(Door door)
		{
			Doors.Add(door);
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

			if (state == RoomState.Over)
			{
				for (int i = 0; i < Doors.Count; i++)
				{
					print(i);
					var door = Doors[i];
					door.OpenDoor();
				}
			}
		}

		void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.CompareTag("Player"))
			{
				Global.currentRoom = this;
				//关闭所有门
				if (roomConfig.roomType == RoomType.Battle)
				{
					if (state == RoomState.Ready)
					{
						state = RoomState.Battle;
						for (int i = 0; i < Doors.Count; i++)
						{
							var door = Doors[i];
							door.CloseDoor();
						}
					}

				}
			}
		}

		private void GenerateEnemy()
		{
			int enemyNum = Random.Range(3, 6);
			var currentPos = enemyPos.OrderByDescending(pos =>
			{
				return (pos - Player.Instance.transform.position).magnitude;
			}).Take(enemyNum);
			//显示所有敌人
			foreach (var pos in currentPos)
			{
				GameObject enemy = Instantiate(LevelController.Instance.enemyPrefab)
				.Position2D(pos.ToVector2())
				.Show();
				enemys.Add(enemy.GetComponent<Enemy>());
			}
		}
	}
}

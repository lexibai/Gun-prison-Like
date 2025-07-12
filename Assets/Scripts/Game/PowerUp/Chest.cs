using UnityEngine;
using QFramework;
using GameRuntime.Room;

namespace GameRuntime.PowerUp
{
	public partial class Chest : ViewController
	{
		void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.CompareTag("Player"))
			{
				Instantiate(LevelController.Instance.HP1)
					.Position(transform.position)
					.Show();
				AudioKit.PlaySound("resources://Chest");
				this.DestroyGameObjGracefully();
			}
		}
	}
}

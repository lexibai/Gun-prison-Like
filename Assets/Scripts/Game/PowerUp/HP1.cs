using UnityEngine;
using QFramework;
using Unity.VisualScripting;

namespace GameRuntime.PowerUp
{
	public partial class HP1 : ViewController
	{
		void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.CompareTag("Player"))
			{
				AudioKit.PlaySound("resources://HP1");
				Global.currentHp = Global.currentHp + 1;
				Global.hpChange();
				this.DestroyGameObjGracefully();
			}
		}
	}
}

using UnityEngine;
using QFramework;
using System.Collections.Generic;

namespace GameRuntime.Actor
{
	public partial class Aim : ViewController
	{
		private List<Sprite> sprites = new List<Sprite>();
		private int index = 0;
		private long frame = 0;
		void Awake()
		{
			sprites.Add(Aim1);
			sprites.Add(Aim2);
			sprites.Add(Aim3);
		}

		void Update()
		{
			frame++;
			if ((frame % 6) == 0)
			{
				print("切换图片" + index);
				SelfSpriteRenderer.sprite = sprites[index++];
				if (index >= sprites.Count)
				{
					print("图片归零" + index);
					index = 0;
				}
			}
		}


	}
}

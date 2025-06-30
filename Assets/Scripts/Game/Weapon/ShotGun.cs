using UnityEngine;
using QFramework;
using Lab;

// 1.请在菜单 编辑器扩展/Namespace Settings 里设置命名空间
// 2.命名空间更改后，生成代码之后，需要把逻辑代码文件（非 Designer）的命名空间手动更改
namespace GameRuntime.Weapon
{
	public partial class ShotGun : AbstractGun
    {

        public override AudioSource AudioSource => SelfAudioSource;

        public override GameObject Bullet => bullet;

        protected override void Shoot(Vector2 dir)
        {
            float angle = dir.ToAngle();
            angle -= 8;
            for (int i = 0; i < 4; i++)
            {
                GameObject bulletObj = Instantiate(Bullet);
                bulletObj.transform.position = Bullet.transform.position;
                Bullet playerBullet = bulletObj.GetComponent<Bullet>();
                playerBullet.Init(angle.AngleToDirection2D(), "enemy");
                angle += 4; // 每次增加4度
            }
        }


    }
}

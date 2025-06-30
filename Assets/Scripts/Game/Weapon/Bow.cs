using UnityEngine;
using QFramework;

// 1.请在菜单 编辑器扩展/Namespace Settings 里设置命名空间
// 2.命名空间更改后，生成代码之后，需要把逻辑代码文件（非 Designer）的命名空间手动更改
namespace GameRuntime.Weapon
{
    public partial class Bow : AbstractGun
    {
        public override AudioSource AudioSource => SelfAudioSource;
        public override GameObject Bullet => bullet;
        protected override float fireRate => 3f; // 每次射击间隔时间


        public override void FireDown(Vector2 dir)
        {
        }

        public override void FireHold(Vector2 dir)
        {

            fireTime += Time.deltaTime;
            if (fireTime > 1f)
            {
                bulletSprite.gameObject.SetActive(true);
                Shoot(dir);
                AudioPlay();
            }
            else
            {
                bulletSprite.gameObject.SetActive(false);
            }
            ResetFireTime();


        }

        public override void FireUp(Vector2 dir)
        {
            bulletSprite.gameObject.SetActive(false);
            fireTime = 0f; // 重置射击间隔
        }



        private void Update()
        {

        }
    }
}

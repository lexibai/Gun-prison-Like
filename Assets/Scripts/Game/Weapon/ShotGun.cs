using UnityEngine;
using QFramework;
using GameRuntime.UI;

// 1.请在菜单 编辑器扩展/Namespace Settings 里设置命名空间
// 2.命名空间更改后，生成代码之后，需要把逻辑代码文件（非 Designer）的命名空间手动更改
namespace GameRuntime.Weapon
{
    public partial class ShotGun : AbstractGun
    {

        public override AudioSource AudioSource => SelfAudioSource;

        public override GameObject Bullet => bullet;

        public override bool Reseting => clip.Reseting;
        //protected override float fireRate => 1f;
        private ShootDuration shootDuration = new ShootDuration(1f);

        private GunClip clip = new GunClip(5);

        private ShootLight shootLight = new ShootLight();

        public override void OnEquip()
        {
            base.OnEquip();
            GameUi.Instance.ShowBulletNum(clip);
        }


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

            shootLight.useLight();
        }


        public override void FireDown(Vector2 dir)
        {
            if (shootDuration.CanShoot() && clip.canShoot)
            {
                base.FireDown(dir);
                clip.useBullet();
                shootDuration.Reset();

            }
        }

        public override void FireHold(Vector2 dir)
        {
            if (shootDuration.CanShoot() && clip.canShoot)
            {
                base.FireHold(dir);
                clip.useBullet();
                shootDuration.Reset();

            }
        }



        public override void Reload()
        {
            base.Reload();
            clip.Reset(ReloadAudioSource);
        }

    }
}

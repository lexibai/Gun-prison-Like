using UnityEngine;
using QFramework;
using GameRuntime.UI;

// 1.请在菜单 编辑器扩展/Namespace Settings 里设置命名空间
// 2.命名空间更改后，生成代码之后，需要把逻辑代码文件（非 Designer）的命名空间手动更改
namespace GameRuntime.Weapon
{
    public partial class RocketGun : AbstractGun
    {
        public override AudioSource AudioSource => SelfAudioSource;

        public override GameObject Bullet => bullet;

        private ShootDuration shootDuration = new ShootDuration(3f);
        public override bool Reseting => clip.Reseting;

        public override BulletBag BulletBag { get; set; } = new BulletBag(500);



        private GunClip clip = new GunClip(10);
        private ShootLight shootLight = new ShootLight();
        public override void OnEquip()
        {
            base.OnEquip();
            GameUi.Instance.ShowBulletNum(clip);
        }



        public override void FireDown(Vector2 dir)
        {
            if (clip.canShoot)
            {

                if (shootDuration.CanShoot())
                {
                    base.FireDown(dir);
                    shootDuration.Reset();
                    clip.useBullet();
                    shootLight.useLight();
                }
            }
            else
            {
                Reload();
            }
        }

        public override void FireHold(Vector2 dir)
        {
            if (clip.canShoot)
            {

                if (shootDuration.CanShoot())
                {
                    base.FireHold(dir);
                    shootDuration.Reset();
                    clip.useBullet();
                    shootLight.useLight();
                }
            }
            else
            {
                if (Time.frameCount % 30 == 0)
                {
                    AudioKit.PlaySound("resources://EmptyBulletSound");
                }
            }
        }

        public override void Reload()
        {
            if (!Reseting)
            {

                base.Reload();
                BulletBag.Reset(clip, ReloadAudioSource);
            }
        }

        protected override void Shoot(Vector2 dir)
        {
            GameObject bulletObj = Instantiate(Bullet);
            bulletObj.transform.position = Bullet.transform.position;
            bulletObj.Rotation(Quaternion.AngleAxis(dir.ToAngle(), bulletObj.transform.forward));
            Bullet playerBullet = bulletObj.GetComponent<Bullet>();
            playerBullet.speed = 10;
            playerBullet.damage = 3;
            playerBullet.Init(dir, "enemy");
        }



    }
}

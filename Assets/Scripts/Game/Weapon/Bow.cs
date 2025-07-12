using UnityEngine;
using QFramework;
using GameRuntime.UI;

// 1.请在菜单 编辑器扩展/Namespace Settings 里设置命名空间
// 2.命名空间更改后，生成代码之后，需要把逻辑代码文件（非 Designer）的命名空间手动更改
namespace GameRuntime.Weapon
{
    public partial class Bow : AbstractGun
    {
        public override AudioSource AudioSource => SelfAudioSource;
        public override GameObject Bullet => bullet;

        public override bool Reseting => clip.Reseting;
        protected float fireRate = 3f; // 每次射击间隔时间
        protected float fireTime = 0; // 每次射击间隔时间

        public override BulletBag BulletBag { get; set; } = new BulletBag(500);



        private GunClip clip = new GunClip(2);
        public override void OnEquip()
        {
            base.OnEquip();
            GameUi.Instance.ShowBulletNum(clip);
        }


        public override void FireDown(Vector2 dir)
        {
        }

        public override void FireHold(Vector2 dir)
        {
            if (clip.canShoot)
            {
                fireTime += Time.deltaTime;
                if (fireTime > 3f)
                {
                    Shoot(dir);
                    AudioPlay();
                    clip.useBullet();
                    bulletSprite.gameObject.SetActive(false);
                    fireTime = 0f; // 重置射击间隔

                }
                else if (fireTime > 1f)
                {
                    bulletSprite.gameObject.SetActive(true);
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

        public override void FireUp(Vector2 dir)
        {
            if (fireTime < 1f)
                return;

            if (clip.canShoot)
            {

                Shoot(dir);
                AudioPlay();
                clip.useBullet();
                bulletSprite.gameObject.SetActive(false);
                fireTime = 0f; // 重置射击间隔
            }
            else
            {
                Reload();
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
            playerBullet.speed = 5;
            playerBullet.damage = 5;
            playerBullet.Init(dir, "enemy");
        }


    }
}

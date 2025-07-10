using GameRuntime.UI;
using QFramework;
using UnityEngine;

// 1.请在菜单 编辑器扩展/Namespace Settings 里设置命名空间
// 2.命名空间更改后，生成代码之后，需要把逻辑代码文件（非 Designer）的命名空间手动更改
namespace GameRuntime.Weapon
{
    public partial class MP5 : AbstractGun
    {
        public override AudioSource AudioSource => SelfAudioSource;

        public override GameObject Bullet => bullet;

        public override bool Reseting => clip.Reseting;

        public override BulletBag BulletBag { get; set; } = new BulletBag(120);



        //protected override float fireRate => 0.1f;
        private ShootDuration shootDuration = new ShootDuration(0.1f);

        private GunClip clip = new GunClip(30);

        private ShootLight shootLight = new ShootLight();
        public override void OnEquip()
        {
            base.OnEquip();
            GameUi.Instance.ShowBulletNum(clip);
        }


        public override void FireDown(Vector2 dir)
        {
            Shoot(dir);
        }

        public override void FireHold(Vector2 dir)
        {
            Shoot(dir);
        }

        public override void FireUp(Vector2 dir)
        {
            AudioSource.Stop();
        }

        protected override void Shoot(Vector2 dir)
        {
            if (shootDuration.CanShoot() && clip.canShoot)
            {
                GameObject bulletObj = Instantiate(Bullet);
                bulletObj.transform.position = Bullet.transform.position;
                bulletObj.Rotation(Quaternion.AngleAxis(dir.ToAngle(), bulletObj.transform.forward));
                Bullet playerBullet = bulletObj.GetComponent<Bullet>();
                playerBullet.speed = 15;
                playerBullet.damage = 3;
                playerBullet.Init(dir, "enemy");
                shootDuration.Reset();
                clip.useBullet();
                shootLight.useLight();
                if (!AudioSource.isPlaying)
                {
                    AudioSource.clip = fireAudios[Random.Range(0, fireAudios.Count)];
                    AudioSource.Play();
                }
            }
            if (!clip.canShoot)
            {
                AudioSource.Stop();
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
    }
}

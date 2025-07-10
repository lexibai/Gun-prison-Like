using UnityEngine;
using QFramework;
using GameRuntime.UI;

// 1.请在菜单 编辑器扩展/Namespace Settings 里设置命名空间
// 2.命名空间更改后，生成代码之后，需要把逻辑代码文件（非 Designer）的命名空间手动更改
namespace GameRuntime.Weapon
{
    public partial class AK : AbstractGun
    {
        public override AudioSource AudioSource => SelfAudioSource;

        public override GameObject Bullet => bullet;

        public override bool Reseting => clip.Reseting;

        public override BulletBag BulletBag { get; set; } = new BulletBag(500);

        //protected override float fireRate => 0.1f;
        private ShootDuration shootDuration = new ShootDuration(0.1f);

        private GunClip clip = new GunClip(60);

        private ShootLight shootLight = new ShootLight();



        public override void FireDown(Vector2 dir)
        {
            if (clip.canShoot)
            {
                if (shootDuration.CanShoot())
                {

                    base.FireDown(dir);
                    AudioSource.clip = fireAudios[Random.Range(0, fireAudios.Count)];
                    AudioSource.loop = true;
                    AudioSource.Play();
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
            if (clip.currentBulletNum == clip.totalBulletNum)
            {

                AudioSource.clip = fireAudios[Random.Range(0, fireAudios.Count)];
                AudioSource.loop = true;
                AudioSource.Play();
            }

            if (shootDuration.CanShoot() && clip.canShoot)
            {
                Shoot(dir);
                shootDuration.Reset();
                clip.useBullet();
                shootLight.useLight();
            }

            if (!clip.canShoot && AudioSource.clip != AKShootEnd)
            {

                AudioSource.Stop();
                AudioSource.clip = AKShootEnd;
                AudioSource.loop = false;
                AudioSource.Play();
            }

            if (!clip.canShoot)
            {
                Reload();
            }

        }

        public override void FireUp(Vector2 dir)
        {
            AudioSource.Stop();

            if (clip.canShoot)
            {
                AudioSource.clip = AKShootEnd;
                AudioSource.loop = false;
                AudioSource.Play();
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

        public override void OnEquip()
        {
            base.OnEquip();
            GameUi.Instance.ShowBulletNum(clip);
        }

        protected override void Shoot(Vector2 dir)
        {
            GameObject bulletObj = Instantiate(Bullet);
            bulletObj.transform.position = Bullet.transform.position;
            bulletObj.Rotation(Quaternion.AngleAxis(dir.ToAngle(), bulletObj.transform.forward));
            Bullet playerBullet = bulletObj.GetComponent<Bullet>();
            playerBullet.speed = 12;
            playerBullet.damage = 5;
            playerBullet.Init(dir, "enemy");
        }



    }
}

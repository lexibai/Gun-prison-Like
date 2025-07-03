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

        //protected override float fireRate => 0.1f;
        private ShootDuration shootDuration = new ShootDuration(0.1f);

        private GunClip clip = new GunClip(60);

        private void OnEnable()
        {
            GameUi.Instance.ShowBulletNum(clip);
        }

        public override void FireDown(Vector2 dir)
        {
            if (shootDuration.CanShoot() && clip.canShoot)
            {
                base.FireDown(dir);
                AudioSource.clip = fireAudios[Random.Range(0, fireAudios.Count)];
                AudioSource.loop = true;
                AudioSource.Play();
                shootDuration.Reset();
                clip.useBullet();
            }
        }

        public override void FireHold(Vector2 dir)
        {
            if (shootDuration.CanShoot() && clip.canShoot)
            {
                Shoot(dir);
                shootDuration.Reset();
                clip.useBullet();
            }

            if (!clip.canShoot)
            {
                AudioSource.Stop();
                AudioSource.clip = AKShootEnd;
                AudioSource.loop = false;
                AudioSource.Play();

            }
        }

        public override void FireUp(Vector2 dir)
        {
            //TODO: 声音存在bug，在没有子弹后依旧有停止开枪的声音
            AudioSource.Stop();
            AudioSource.clip = AKShootEnd;
            AudioSource.loop = false;
            AudioSource.Play();
        }

        public override void Reload()
        {
            clip.Reset();
        }
    }
}

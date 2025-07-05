using System.Collections.Generic;
using GameRuntime.UI;
using NUnit.Framework;
using QFramework;
using UnityEngine;

namespace GameRuntime.Weapon
{
    /// <summary>
    /// 手枪
    /// </summary>
    public partial class Pistol : AbstractGun
    {
        public override AudioSource AudioSource => SelfAudioSource;
        public override GameObject Bullet => bullet;
        private ShootDuration shootDuration = new ShootDuration(0.3f);

        private GunClip clip = new GunClip(10);

        private void OnEnable()
        {
            GameUi.Instance.ShowBulletNum(clip);
        }

        public override void FireDown(Vector2 dir)
        {
            if (shootDuration.CanShoot() && clip.canShoot)
            {
                base.FireDown(dir);
                shootDuration.Reset();
                clip.useBullet();
            }
        }

        public override void FireHold(Vector2 dir)
        {
            if (shootDuration.CanShoot() && clip.canShoot)
            {
                base.FireHold(dir);
                shootDuration.Reset();
                clip.useBullet();
            }
        }

        public override void Reload()
        {
            clip.Reset();
        }
    }
}

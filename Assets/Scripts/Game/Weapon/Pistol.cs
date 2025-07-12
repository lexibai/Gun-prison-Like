using System;
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

        public override bool Reseting => clip.Reseting;

        private ShootDuration shootDuration = new ShootDuration(0.3f);

        private ShootLight shootLight = new ShootLight();

        private GunClip clip = new GunClip(10);

        public override BulletBag BulletBag { get; set; } = new BulletBag(100);





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
                    base.FireDown(dir);
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
            playerBullet.damage = 1;
            playerBullet.Init(dir, "enemy");
        }

    }
}

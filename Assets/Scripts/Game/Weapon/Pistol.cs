using Lab;
using NUnit.Framework;
using QFramework;
using System.Collections.Generic;
using UnityEngine;

namespace GameRuntime.Weapon
{
    public partial class Pistol : AbstractGun
    {

        public override AudioSource AudioSource => SelfAudioSource;
        public override GameObject Bullet => bullet;
        private ShootDuration shootDuration = new ShootDuration(0.3f);

        public override void FireDown(Vector2 dir)
        {
            if (shootDuration.CanShoot())
            {
                base.FireDown(dir);
                shootDuration.Reset();

            }
        }

        public override void FireHold(Vector2 dir)
        {
            if (shootDuration.CanShoot())
            {
                base.FireHold(dir);
                shootDuration.Reset();

            }
        }
    }

}

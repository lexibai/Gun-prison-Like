using Lab;
using QFramework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


namespace GameRuntime.Weapon
{
    [ViewControllerChild]
    public abstract class AbstractGun : ViewController
    {
        public List<AudioClip> fireAudios;

        public abstract AudioSource AudioSource { get; }

        public abstract GameObject Bullet { get; }

        protected virtual float fireRate => 0.3f; // 每次射击间隔时间
        protected float fireTime = float.MaxValue;

        public virtual void FireDown(Vector2 dir)
        {
            Shoot(dir);
            AudioPlay();
            ResetFireTime();
        }

        public virtual void FireHold(Vector2 dir)
        {
            Shoot(dir);
            AudioPlay();
            ResetFireTime();

        }

        public virtual void FireUp(Vector2 dir)
        {
        }

        protected void ResetFireTime()
        {
            if (fireTime > fireRate)
            {
                fireTime = 0f; // 重置射击间隔
            }
        }

        protected virtual void Shoot(Vector2 dir)
        {
            if (fireTime >= fireRate)
            {
                GameObject bulletObj = Instantiate(Bullet);
                bulletObj.transform.position = Bullet.transform.position;
                bulletObj.Rotation(Quaternion.AngleAxis(dir.ToAngle(), bulletObj.transform.forward));
                Bullet playerBullet = bulletObj.GetComponent<Bullet>();
                playerBullet.Init(dir, "enemy");
            }
        }

        protected virtual void AudioPlay()
        {
            if (fireTime >= fireRate)
            {
                AudioSource.clip = fireAudios[Random.Range(0, fireAudios.Count)];
                AudioSource.Play();
            }
        }



        private void Update()
        {
            fireTime += Time.deltaTime;
        }
    }
}



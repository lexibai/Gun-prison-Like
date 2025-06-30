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

        protected float fireRate = 0f; // 每次射击间隔时间

        public virtual void FireDown(Vector2 dir)
        {
            Shoot(dir);
            AudioSource.clip = fireAudios[Random.Range(0, fireAudios.Count)];
            AudioSource.Play();
        }

        public virtual void FireHold(Vector2 dir)
        {
            //print("持续射击" + fireRate);
            if(fireRate >= 0.3)
            {
                Shoot(dir);
                AudioSource.clip = fireAudios[Random.Range(0, fireAudios.Count)];
                AudioSource.Play();
                fireRate = 0f; // 重置射击间隔
            }
            fireRate += Time.deltaTime;
        }

        public virtual void FireUp(Vector2 dir)
        {
            fireRate = 0f;
        }

        protected virtual void Shoot(Vector2 dir) {
            GameObject bulletObj = Instantiate(Bullet);
            bulletObj.transform.position = Bullet.transform.position;
            Bullet playerBullet = bulletObj.GetComponent<Bullet>();
            playerBullet.Init(dir, "enemy");
        }
    }
}



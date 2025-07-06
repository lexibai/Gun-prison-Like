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

        public AudioClip ReloadAudioSource;

        public virtual BulletBag BulletBag { get; set; }

        public abstract bool Reseting { get; }

        public virtual int damage { get; }


        public virtual void OnEquip()
        {

        }


        public virtual void FireDown(Vector2 dir)
        {
            Shoot(dir);
            AudioPlay();
        }

        public virtual void FireHold(Vector2 dir)
        {
            Shoot(dir);
            AudioPlay();

        }

        public virtual void FireUp(Vector2 dir)
        {
        }


        public virtual void Reload()
        {

        }

        protected virtual void Shoot(Vector2 dir)
        {

            GameObject bulletObj = Instantiate(Bullet);
            bulletObj.transform.position = Bullet.transform.position;
            bulletObj.Rotation(Quaternion.AngleAxis(dir.ToAngle(), bulletObj.transform.forward));
            Bullet playerBullet = bulletObj.GetComponent<Bullet>();
            playerBullet.Init(dir, "enemy");

        }

        protected virtual void AudioPlay()
        {
            AudioSource.clip = fireAudios[Random.Range(0, fireAudios.Count)];
            AudioSource.Play();
        }



    }
}



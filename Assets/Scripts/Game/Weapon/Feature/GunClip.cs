using GameRuntime.UI;
using QFramework;
using UnityEngine;

namespace GameRuntime.Weapon
{
    public class GunClip
    {
        public int totalBulletNum;

        public int currentBulletNum;

        public bool Reseting = false;

        public bool canShoot => currentBulletNum > 0 && !Reseting;

        public GunClip(int initBulletNum)
        {
            totalBulletNum = initBulletNum;
            currentBulletNum = initBulletNum;
        }

        public void useBullet()
        {
            currentBulletNum--;
            GameUi.Instance.ShowBulletNum(this);
        }

        public void Reset(AudioClip resetAudioClip)
        {
            if (!Reseting)
            {
                Reseting = true;
                ActionKit.Sequence().PlaySound(resetAudioClip).Delay(0.5f).Callback(() =>
                {
                    currentBulletNum = totalBulletNum;
                    GameUi.Instance.ShowBulletNum(this);
                    Reseting = false;
                }).StartCurrentScene();

            }
        }
    }

}
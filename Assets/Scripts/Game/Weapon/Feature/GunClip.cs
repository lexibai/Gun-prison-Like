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

        public int needBulletNum => totalBulletNum - currentBulletNum;

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

        public void Reset(AudioClip resetAudioClip, int bulletNum = -1)
        {
            if (bulletNum == -1)
            {
                bulletNum = totalBulletNum;
            }
            if (!Reseting)
            {
                Reseting = true;
                ActionKit.Sequence().PlaySound(resetAudioClip).Delay(0.5f).Callback(() =>
                {
                    currentBulletNum += bulletNum;
                    GameUi.Instance.ShowBulletNum(this);
                    Reseting = false;
                }).StartCurrentScene();

            }
        }
    }

}
using System;
using UnityEngine;

namespace GameRuntime.Weapon
{
    public class BulletBag
    {
        public int totleBullet;
        public int currentBullet;

        public bool isInfinite => totleBullet == -1;

        public BulletBag(int initBulletNum)
        {
            currentBullet = initBulletNum;
            totleBullet = initBulletNum;
        }

        public void Reset(GunClip clip, AudioClip audio)
        {
            int bulletNum = Math.Min(clip.needBulletNum, currentBullet);
            if (bulletNum > 0)
            {
                currentBullet -= bulletNum;
                clip.Reset(audio, bulletNum);
            }
        }

    }
}
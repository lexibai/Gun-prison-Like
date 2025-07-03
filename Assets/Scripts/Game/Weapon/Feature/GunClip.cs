using GameRuntime.UI;

namespace GameRuntime.Weapon
{
    public class GunClip
    {
        public int totalBulletNum;

        public int currentBulletNum;

        public bool canShoot => currentBulletNum > 0;

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

        public void Reset()
        {
            currentBulletNum = totalBulletNum;
            GameUi.Instance.ShowBulletNum(this);
        }
    }

}
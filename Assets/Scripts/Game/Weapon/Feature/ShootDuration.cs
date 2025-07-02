
using UnityEngine;

namespace GameRuntime.Weapon
{
    public class ShootDuration
    {
        
        //上次射击时间
        private float lastTime;

        /// <summary>
        /// 设计间隔时间
        /// </summary>
        private float waitTime;

        /// <summary>
        /// 初始化，设置射击间隔时间
        /// </summary>
        /// <param name="waitTime"></param>
        public ShootDuration(float waitTime)
        {
            this.waitTime = waitTime;
            lastTime = 0f;
        }

        /// <summary>
        /// 是否可以射击
        /// </summary>
        /// <returns></returns>
        public bool CanShoot()
        {
            if (Time.time - lastTime >= waitTime)
            {
                return true;
            }
            return false;
        }


        public void Reset()
        {
            lastTime = Time.time;
        }


    }
}
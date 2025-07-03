using GameRuntime.Weapon;
using QFramework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameRuntime.UI
{
    public partial class GameUi : ViewController
    {
        public static GameUi Instance;


        private void Awake()
        {
            Instance = this;
        }


        private void Start()
        {
            reStart.onClick.AddListener(() =>
            {
                Global.ReStart();
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                Time.timeScale = 1; // 重置时间缩放
            });
            ShowHp();
            Global.hpChange += ShowHp; // 订阅血量变化事件

        }

        private void OnDestroy()
        {
            Global.hpChange -= ShowHp; // 取消订阅血量变化事件
        }

        public void ShowGameOverPanel()
        {
            Time.timeScale = 0; // 暂停游戏
            GameOver?.gameObject?.SetActive(true);
        }

        public void ShowHp()
        {
            curHp.text = "Hp: " + Global.currentHp;
        }

        public void ShowBulletNum(GunClip clip)
        {
            bulletCount.text = $"子弹数量：{clip.currentBulletNum}/{clip.totalBulletNum}（R键重置）";
        }
    }

}

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Lab
{
    public class GameUi : MonoBehaviour
    {
        public static GameUi Instance;

        public GameObject gameOverPanel;
        public Button reStartButton;
        public Text hpText;

        private void Awake()
        {
            Instance = this;
        }


        private void Start()
        {
            reStartButton.onClick.AddListener(() =>
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
            gameOverPanel?.SetActive(true);
        }

        public void ShowHp()
        {
            hpText.text = "Hp: " + Global.currentHp;
        }
    }

}

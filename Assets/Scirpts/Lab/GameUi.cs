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

        private void Awake()
        {
            Instance = this;
        }


        private void Start()
        {
            reStartButton.onClick.AddListener(() =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            });
        }

        public void ShowGameOverPanel()
        {
            gameOverPanel?.SetActive(true);
        }
    }

}

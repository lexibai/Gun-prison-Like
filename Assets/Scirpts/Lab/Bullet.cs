using UnityEngine;

namespace Lab
{
    public class Bullet : MonoBehaviour
    {
        public bool isInit = false;
        public Vector2 dir = Vector2.zero;
        public string target = "";

        public void Init(Vector2 dir, string target)
        {
            isInit = true;
            this.dir = dir.normalized;
            this.target = target;
            gameObject.SetActive(true);
        }

        // Update is called once per frame
        void Update()
        {
            if (isInit)
            {
                transform.Translate(dir * Time.deltaTime);
            }

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision?.gameObject?.name.StartsWith(target)?? false)
            {
                Destroy(gameObject);
                collision.gameObject.SetActive(false);
                //GameUi.Instance.ShowGameOverPanel(); // 显示游戏结束面板
            }
        }
    }
}


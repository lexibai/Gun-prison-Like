using GameRuntime.Actor;
using GameRuntime.UI;
using UnityEngine;

namespace GameRuntime
{
    public class FinishController : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<Player>())
            {
                GameUi.Instance.ShowGameOverPanel();
            }
        }
    }
}

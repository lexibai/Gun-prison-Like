using Lab;
using UnityEngine;
using GameRuntime.UI;
using GameRuntime.Actor;


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

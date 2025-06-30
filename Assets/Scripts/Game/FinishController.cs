using Lab;
using UnityEngine;

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

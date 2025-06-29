using UnityEngine;


namespace Lab
{
    public class CameraController : MonoBehaviour
    {


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Global.Player)
            {
                Vector3 position = Global.Player.transform.position;
                position.z = transform.position.z;
                transform.position = position;
            }
            else
            {
                Debug.LogWarning("Target is not assigned in CameraController.");
            }

        }
    }
}


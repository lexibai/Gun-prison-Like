using UnityEngine;


namespace Lab
{
    public class CameraController : MonoBehaviour
    {
        public float smoothTime = 5f; // 平滑时间


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Global.Player)
            {
                Vector3 startPos = Camera.main.transform.position;
                Vector3 targetPos = Global.Player.transform.position;
                Vector3 curPos = Vector3.Lerp(startPos, targetPos, 1 - Mathf.Exp(-Time.deltaTime* smoothTime));
                curPos.z = startPos.z; // 保持相机的Z轴位置不变
                transform.position = curPos;
            }
            else
            {
                Debug.LogWarning("Target is not assigned in CameraController.");
            }

        }
    }
}


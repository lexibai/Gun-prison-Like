using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform target; // The target object to follow

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {

            Vector3 position = target.position;
            position.z = transform.position.z;
            transform.position = position;
        }
        else
        {
            Debug.LogWarning("Target is not assigned in CameraController.");
        }

    }
}

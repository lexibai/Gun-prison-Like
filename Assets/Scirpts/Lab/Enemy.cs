using UnityEngine;

namespace Lab
{
    public class Enemy : MonoBehaviour
    {
        public enum State
        {
            Tracking,
            Attack
        }

        public GameObject enemyBullet;

        public State state = State.Tracking;

        public float waitTime = 1.0f;



        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            Application.targetFrameRate = 60; // Set target frame rate to 60 FPS
        }

        // Update is called once per frame
        void Update()
        {
            if (state == State.Tracking)
            {
                if (waitTime <= 0)
                {
                    waitTime = Random.Range(1.0f, 3.0f); // Reset tracking time to a random value between 1 and 3 seconds
                    state = State.Attack;
                    return;
                }
                Vector3 dir = Global.Player.transform.position - transform.position;
                transform.Translate(dir.normalized * Time.deltaTime);
                waitTime -= Time.deltaTime;
            }
            else if (state == State.Attack)
            {
                if (waitTime <= 0)
                {
                    waitTime = Random.Range(1.0f, 3.0f); // Reset tracking time to a random value between 1 and 3 seconds
                    state = State.Tracking; // Switch back to tracking state
                    return;
                }
                waitTime -= Time.deltaTime;
                // Attack logic can be implemented here
                if (Time.frameCount % 20 == 0) // Attack every second
                {
                    Vector2 direction = (Global.Player.transform.position - transform.position);
                    GameObject bulletObj = Instantiate(enemyBullet);
                    bulletObj.transform.position = transform.position;
                    Bullet playerBullet = bulletObj.GetComponent<Bullet>();
                    playerBullet.Init(direction, "Player");
                }

            }

        }


    }
}



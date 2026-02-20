using GameRuntime.Actor;
using UnityEngine;

namespace GameRuntime.Weapon
{
    public class Bullet : MonoBehaviour
    {
        public bool isInit = false;
        public Vector2 dir = Vector2.zero;
        public Rigidbody2D rb;
        public int damage = 1;
        public string target = "";

        public void Init(Vector2 dir, string target)
        {
            isInit = true;
            this.dir = dir.normalized;
            this.target = target;
            rb = GetComponent<Rigidbody2D>();
            gameObject.SetActive(true);
        }

        // Update is called once per frame
        void Update()
        {
            if (isInit)
            {
                rb.linearVelocity = dir.normalized * 10;
            }

        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (isInit)
            {
                print($"Bullet collided with {collision.gameObject.name}");
                print($"Target is {target}");
                if (collision.gameObject.CompareTag(target))
                {
                    Destroy(gameObject);
                    collision.gameObject.GetComponent<ICanHurt>()?.Hurt(damage);
                }
                else
                {
                    Destroy(gameObject);
                }
            }

        }
    }
}


using UnityEngine;
using UnityEngine.InputSystem;

namespace Lab
{
    public class Player : MonoBehaviour, ICanHurt
    {
        public PlayerController playerController;
        public GameObject bullet;
        public SpriteRenderer sprite;
        public Rigidbody2D rb;


        public Vector2 moveInput = new Vector2(0, 0);

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();

            playerController = new PlayerController();
            playerController.Enable(); // 启用输入
            playerController.Player.Move.performed += Move;
            playerController.Player.Move.canceled += context =>
            {
                moveInput = context.ReadValue<Vector2>();
            };
            playerController.Player.Attack.performed += context =>
            {
                // 获取鼠标在世界坐标中的位置
                Vector3 mouseScreenPos = UnityEngine.InputSystem.Mouse.current.position.ReadValue();
                Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, Camera.main.nearClipPlane));
                Vector2 direction = (mouseWorldPos - transform.position);

                GameObject bulletObj = Instantiate(bullet);
                bulletObj.transform.position = transform.position;
                Bullet playerBullet = bulletObj.GetComponent<Bullet>();

                playerBullet.Init(direction, "enemy");
            };
        }

        private void Move(InputAction.CallbackContext ctx)
        {
            moveInput = ctx.ReadValue<Vector2>();
            if (moveInput.x < 0)
            {
                sprite.flipX = true;
            }
            if (moveInput.x > 0)
            {
                sprite.flipX = false;
            }
        }

        private void OnEnable()
        {
            playerController?.Enable(); // 确保在启用时输入被启用
        }


        private void OnDisable()
        {
            playerController?.Disable(); // 确保在禁用时输入被禁用
        }

        void OnDestroy()
        {

            playerController?.Disable(); // 确保在销毁时输入被禁用

        }

        void Update()
        {
            //transform.position += new Vector3(moveInput.x, moveInput.y, 0) * Time.deltaTime;
            rb.linearVelocity = moveInput.normalized * 3f; // 设置刚体速度
        }

        public void Hurt(int damage)
        {
            Global.currentHp -= damage;
            Global.hpChange?.Invoke();
            if (Global.currentHp <= 0)
            {
                GameUi.Instance.ShowGameOverPanel();
            }
        }
    }

}


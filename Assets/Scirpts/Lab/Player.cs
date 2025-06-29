using UnityEngine;

namespace Lab
{
    public class Player : MonoBehaviour
    {
        public PlayerController playerController;
        public GameObject bullet;
        public Vector2 moveInput = new Vector2(0, 0);

        void Start()
        {
            playerController = new PlayerController();
            playerController.Enable(); // 启用输入

            playerController.Player.Move.performed += context =>
            {
                moveInput = context.ReadValue<Vector2>();
            };
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

                playerBullet.Init(direction, "Enemy");
            };
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
            transform.position += new Vector3(moveInput.x, moveInput.y, 0) * Time.deltaTime;
        }
    }

}


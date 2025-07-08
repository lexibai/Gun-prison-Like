using System.Collections.Generic;
using System.Reflection;
using GameRuntime.UI;
using GameRuntime.Weapon;
using QFramework;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameRuntime.Actor
{
    public partial class Player : ViewController, ICanHurt
    {
        //Player的外部获取
        private static Player _instance;
        public static Player Instance => _instance;
        public PlayerController playerController;
        public SpriteRenderer sprite;
        public Rigidbody2D rb;
        public Transform weapon;
        public AbstractGun gun;
        public bool isfireHold = false;

        public List<AbstractGun> guns = new();
        public int currentGunIndex = 0;

        public Vector2 moveInput = new Vector2(0, 0);

        public List<AudioClip> audioClips = new List<AudioClip>();
        private AudioClip cutGunAudioClip => audioClips[Random.Range(0, audioClips.Count)];

        private void Awake()
        {
            _instance = this;

            guns.Add(this.Pistol);
            guns.Add(this.MP5);
            guns.Add(this.ShotGun);
            guns.Add(this.AK);
            guns.Add(this.AWP);
            guns.Add(this.Laser);
            guns.Add(this.Bow);
            guns.Add(this.RocketGun);


            audioClips.Add(this.GunTake1);
            audioClips.Add(this.GunTake2);
            audioClips.Add(this.GunTake3);
            audioClips.Add(this.GunTake4);
            audioClips.Add(this.GunTake5);
        }

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
                //print("开启"+isfireHold);
                isfireHold = true;
                gun.FireDown(LookDir());
            };
            playerController.Player.Attack.canceled += context =>
            {
                //print("关闭"+isfireHold);
                isfireHold = false;
                gun.FireUp(LookDir());
            };
            playerController.Player.ReClip.performed += context =>
            {
                gun.Reload();
            };
            playerController.Player.CutGun.performed += context =>
            {
                CutGun();
            };
            CutGun();
        }

        private void Move(InputAction.CallbackContext ctx)
        {
            moveInput = ctx.ReadValue<Vector2>();
            // if (moveInput.x < 0)
            // {
            //     sprite.flipX = true;
            // }
            // if (moveInput.x > 0)
            // {
            //     sprite.flipX = false;
            // }
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
            rb.linearVelocity = moveInput.normalized * 5f; // 设置刚体速度

            Vector2 direction = LookDir();
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            weapon.rotation = Quaternion.Euler(new Vector3(0, 0, angle)); // 调整武器朝向

            weapon.localScale = new Vector3(1, direction.x > 0 ? 1 : -1, 1); // 根据鼠标位置调整武器缩放
            sprite.flipX = direction.x < 0; // 根据鼠标位置调整人物朝向

            //print(isfireHold);
            if (isfireHold)
            {
                gun.FireHold(LookDir());
            }
        }

        private Vector2 LookDir()
        {
            Vector3 mouseScreenPos = Mouse.current.position.ReadValue();
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(
                new Vector3(mouseScreenPos.x, mouseScreenPos.y, Camera.main.nearClipPlane)
            );
            Vector2 direction = (mouseWorldPos - transform.position);
            return direction;
        }

        private void CutGun()
        {
            if (!gun.Reseting)
            {
                gun.gameObject.SetActive(false);
                gun = guns[currentGunIndex++];
                gun.gameObject.SetActive(true);
                gun.OnEquip();
                if (currentGunIndex >= guns.Count)
                {
                    currentGunIndex = 0;
                }
                this.SelfAudioSource.clip = cutGunAudioClip;
                this.SelfAudioSource.Play();
            }
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

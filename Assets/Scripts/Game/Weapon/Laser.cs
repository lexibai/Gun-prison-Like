using UnityEngine;
using QFramework;

// 1.请在菜单 编辑器扩展/Namespace Settings 里设置命名空间
// 2.命名空间更改后，生成代码之后，需要把逻辑代码文件（非 Designer）的命名空间手动更改
namespace GameRuntime.Weapon
{
    public partial class Laser : AbstractGun
    {
        public override AudioSource AudioSource => SelfAudioSource;
        public override GameObject Bullet => bullet;

        private ShootLight shootLight = new ShootLight();

        public override BulletBag BulletBag { get; set; } = new BulletBag(-1);



        public bool isFiring = false;

        public override bool Reseting => false;
        private void Start()
        {
            AudioSource.loop = true;
        }

        public override void FireDown(Vector2 dir)
        {
            SelfLineRenderer.enabled = true;
            isFiring = true;
            AudioPlay();
        }

        public override void FireHold(Vector2 dir)
        {
            int layer = LayerMask.GetMask("enemy", "wall");
            var hit = Physics2D.Raycast(Bullet.transform.position, dir, 100f, layer);
            SelfLineRenderer.SetPosition(0, Bullet.transform.position);
            SelfLineRenderer.SetPosition(1, hit.point);
        }

        public override void FireUp(Vector2 dir)
        {
            isFiring = false;
            AudioSource.Stop();
            SelfLineRenderer.enabled = false;
        }

    }
}

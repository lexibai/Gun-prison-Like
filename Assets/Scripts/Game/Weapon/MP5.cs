using Lab;
using QFramework;
using UnityEngine;

// 1.请在菜单 编辑器扩展/Namespace Settings 里设置命名空间
// 2.命名空间更改后，生成代码之后，需要把逻辑代码文件（非 Designer）的命名空间手动更改
namespace GameRuntime.Weapon
{
	public partial class MP5 : AbstractGun
    {
        public override AudioSource AudioSource => SelfAudioSource;

        public override GameObject Bullet => bullet;

        protected override float fireRate => 0.1f;

        public override void FireDown(Vector2 dir)
        {
            base.FireDown(dir);
            AudioSource.clip = fireAudios[Random.Range(0, fireAudios.Count)];
            AudioSource.Play();
        }

        public override void FireHold(Vector2 dir)
        {
            Shoot(dir);
            ResetFireTime();
        }

        public override void FireUp(Vector2 dir)
        {
            AudioSource.Stop();
        }
    }
}

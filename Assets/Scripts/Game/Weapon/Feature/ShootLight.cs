using GameRuntime.Actor;
using QFramework;

namespace GameRuntime.Weapon
{
    public class ShootLight
    {
        public void useLight()
        {
            Player.Instance.GunShootLight.SetActive(true);
            ActionKit.Sequence().DelayFrame(3)
            .Callback(() =>
            {
                Player.Instance.GunShootLight.SetActive(false);
            }).StartCurrentScene();
        }
    }
}
using Lab;
using NUnit.Framework;
using QFramework;
using System.Collections.Generic;
using UnityEngine;

namespace GameRuntime.Weapon
{
    public partial class Pistol : AbstractGun
    {

        public override AudioSource AudioSource => SelfAudioSource;
        public override GameObject Bullet => bullet;


    }

}

using UnityEngine;

namespace GameRuntime.Actor
{
    public interface ICanHurt
    {
        void Hurt(int damage);
    }
}

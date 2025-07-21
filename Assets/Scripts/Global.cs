using System;
using UnityEngine;

namespace GameRuntime
{
    public class Global
    {
        public static GameObject Player;
        public static GameObject Enmpy;
        public static Room.Room currentRoom;

        public static int currentHp = 10;

        public static Action hpChange;

        public static void ReStart()
        {
            currentHp = 10;
        }
    }
}

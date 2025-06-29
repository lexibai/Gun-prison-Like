using System;
using UnityEngine;

public class Global
{
    public static GameObject Player;
    public static GameObject Enmpy;

    public static int currentHp = 3;

    public static Action hpChange;

    public static void ReStart() {
        currentHp = 3;
    }
}

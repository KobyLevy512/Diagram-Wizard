using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConnectRef
{
    public static bool ConnectionMode = false;
    public static GameObject Object1;
    public static GameObject Object2;
    public static Tools Tools;

    public static void SpawnConnection()
    {
        ConnectionMode = false;
        Tools.AddConnection(Object1, Object2);
        Object1 = null;
        Object2 = null;
    }
}

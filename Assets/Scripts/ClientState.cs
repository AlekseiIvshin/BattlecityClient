﻿using UnityEngine;
using UnityEditor;

public static class ClientState
{
    public static string DEFAULT_SERVER = "codenjoy.juja.com.ua";
    public static string DEFAULT_INNER_SERVER = "epruizhsa0001t2:8080";
    public static long DEFAULT_TICK_TIME = 400;

    public static string serverAddress = DEFAULT_SERVER;
    public static long tickTime = DEFAULT_TICK_TIME;
}
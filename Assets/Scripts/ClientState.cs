using UnityEngine;
using UnityEditor;

public static class ClientState
{
    public static string DEFAULT_SERVER = "codenjoy.juja.com.ua";
    public static string DEFAULT_INNER_SERVER = "epruizhsa0001t2:8080";
    public static long DEFAULT_TICK_TIME = 1000;

    public static string serverAddress = DEFAULT_INNER_SERVER;
    public static long tickTime = DEFAULT_TICK_TIME;
}
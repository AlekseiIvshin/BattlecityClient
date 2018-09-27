using UnityEngine;
using UnityEditor;

public static class ClientState
{
    public static string DEFAULT_SERVER = "codenjoy.juja.com.ua/codenjoy-contest/";
    public static long DEFAULT_TICK_TIME = 1000;

    public static string serverAddress = DEFAULT_SERVER;
    public static long tickTime = DEFAULT_TICK_TIME;

    public static string MESSAGE = "{name: 'getScreen', allPlayersScreen: true, gameName:'battlecity'}";
}
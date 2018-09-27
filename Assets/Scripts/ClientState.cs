using UnityEngine;
using UnityEditor;

public static class ClientState 
{
    public static string serverAddress;
    public static float tickTime;


    public static string KEYS_URL = "http://codenjoy.juja.com.ua/codenjoy-contest/rest/sprites/battlecity";
    public static string DEFAULT_SERVER_URL = "ws://codenjoy.juja.com.ua/codenjoy-contest/screen-ws?user=test@test.com&code=20998118591535248716";
    public static string MESSAGE = "{name: 'getScreen', allPlayersScreen: true, gameName:'battlecity'}";
}
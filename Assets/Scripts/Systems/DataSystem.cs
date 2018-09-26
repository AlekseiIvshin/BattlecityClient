using Leopotam.Ecs;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using WebSocketSharp;
using SimpleJSON;
using UnityEngine.Networking;
using System.Collections;
using System.Threading;

[EcsInject]
public class DataSystem : IEcsInitSystem, IEcsRunSystem
{
    const string KEYS_URL = "http://codenjoy.juja.com.ua/codenjoy-contest/rest/sprites/battlecity";
    const string SERVER_URL = "ws://codenjoy.juja.com.ua/codenjoy-contest/screen-ws?user=test@test.com&code=20998118591535248716";
    const string MESSAGE = "{name: 'getScreen', allPlayersScreen: true, gameName:'battlecity'}";

    private MonoBehaviour _monoBehaviour;

    private long _stateVersion = 0;

    public DataSystem(MonoBehaviour monoBehaviour)
    {
        this._monoBehaviour = monoBehaviour;
    }

    WebSocket webSocket;

    const int MESSAGES_INTERVAL = 1000;

    long nextExchange = -1;

    void IEcsInitSystem.Initialize()
    {
        webSocket = new WebSocket(SERVER_URL);
        webSocket.OnMessage += (sender, e) =>
        {
            _stateVersion++;
            var node = JSON.Parse(e.Data);
            var values = node.Values;
            if (values.MoveNext())
            {
                var heroesData = values.Current["heroesData"];
                var field = MapUtils.to2Dimension(values.Current["board"]);
                var fieldSize = field.Length;
                var tanks = new Dictionary<string, TankData>();
                TankData tank;
                foreach(var tankNode in heroesData.Values)
                {
                    tank = new TankData();
                    var tankValue = tankNode.Values;
                    tankValue.MoveNext();
                    tank.column = tankValue.Current["coordinate"]["x"];
                    tank.row = fieldSize - tankValue.Current["coordinate"]["y"] -1;
                    tank.symbol = field[tank.row][tank.column];
                    var tankKey = tankNode.Keys;
                    tankKey.MoveNext();
                    tanks.Add(tankKey.Current, tank);
                }
                GameStateEventManager.getInstance().onUpdate(new BattlefieldState
                {
                    field = field,
                    tanks = tanks,
                }, _stateVersion);
            }
        };

        this._monoBehaviour.StartCoroutine(getFieldMapping());
    }

    void IEcsInitSystem.Destroy()
    {
        if (webSocket.IsConnected)
        {
            webSocket.Close();
        }
    }

    void IEcsRunSystem.Run()
    {
        if (webSocket.IsConnected && nextExchange <= DateTime.Now.Ticks)
        {
            nextExchange = DateTime.Now.Ticks + MESSAGES_INTERVAL;
            webSocket.Send(MESSAGE);
        }
    }

    private IEnumerator getFieldMapping()
    {
        var request = UnityWebRequest.Get(KEYS_URL);
        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }
        else
        {
            // Show results as text
            var node = JSON.Parse(request.downloadHandler.text);
            var index = 0;
            var keys = new Dictionary<char, string>();
            foreach( var val in node.Values)
            {
                keys.Add(MapItems.ALPHABET[index], val);
                index++;
            }
            MapItems.MAP_KEYS = keys;
            _stateVersion = 0;
            webSocket.Connect();
        }
    }
}
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
    const string MESSAGE = "{name: 'getScreen', allPlayersScreen: true, gameName:'battlecity'}";


    public static string getServerAddress()
    {
        return "ws://" + ClientState.serverAddress + "screen-ws?user=test@test.com&code=20998118591535248716";
    }
    public static string getKeysAddress()
    {
        return "http://" + ClientState.serverAddress + "rest/sprites/battlecity";
    }

    EcsFilterSingle<SharedGameState> _gameState = null;

    private MonoBehaviour _monoBehaviour;

    private long _stateVersion = 0;

    public DataSystem(MonoBehaviour monoBehaviour)
    {
        this._monoBehaviour = monoBehaviour;
    }

    WebSocket webSocket;

    long nextExchange = -1;

    void IEcsInitSystem.Initialize()
    {
        webSocket = new WebSocket(getServerAddress());
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
                var tanks = new Dictionary<string, MapItem>();
                MapItem tank;
                foreach (var tankNode in heroesData.Values)
                {
                    tank = new MapItem();
                    var tankValue = tankNode.Values;
                    tankValue.MoveNext();
                    tank.column = tankValue.Current["coordinate"]["x"];
                    tank.row = fieldSize - tankValue.Current["coordinate"]["y"] - 1;
                    tank.symbol = field[tank.row][tank.column];
                    var tankKey = tankNode.Keys;
                    tankKey.MoveNext();
                    tanks.Add(tankKey.Current, tank);
                }
                if (_gameState.Data.fieldSize != fieldSize)
                {
                    _gameState.Data.fieldSize = fieldSize;
                }
                GameStateEventManager.getInstance().onUpdate(new BattlefieldState
                {
                    field = field,
                    tanks = tanks,
                    size = fieldSize
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
            nextExchange = DateTime.Now.Ticks + ClientState.tickTime;
            webSocket.Send(MESSAGE);
        }
    }

    private IEnumerator getFieldMapping()
    {
        var request = UnityWebRequest.Get(getKeysAddress());
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
            foreach (var val in node.Values)
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
using Leopotam.Ecs;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using WebSocketSharp;
using SimpleJSON;

[EcsInject]
public class DataSystem : IEcsInitSystem, IEcsRunSystem
{
    const string SERVER_URL = "ws://codenjoy.juja.com.ua/codenjoy-contest/screen-ws?user=test@test.com&code=20998118591535248716";
    const string MESSAGE = "{name: 'getScreen', allPlayersScreen: true, gameName:'battlecity'}";

    WebSocket webSocket;

    const int MESSAGES_INTERVAL = 1000;

    long nextExchange = -1;

    void IEcsInitSystem.Initialize()
    {
        webSocket = new WebSocket(SERVER_URL);
        webSocket.OnMessage += (sender, e) =>
        {
            var node = JSON.Parse(e.Data);
            var values = node.Values;
            if (values.MoveNext())
            {
                var heroesData = values.Current["heroesData"];
                var field = BattleField.to2Dimension(values.Current["board"]);
                var tanks = new Dictionary<string, TankData>();
                TankData tank;
                foreach(var tankNode in heroesData.Values)
                {
                    tank = new TankData();
                    var tankValue = tankNode.Values;
                    tankValue.MoveNext();
                    tank.column = tankValue.Current["coordinate"]["x"];
                    tank.row = tankValue.Current["coordinate"]["y"];
                    tank.symbol = field[tank.row][tank.column];
                    var tankKey = tankNode.Keys;
                    tankKey.MoveNext();
                    tanks.Add(tankKey.Current, tank);
                }
                GameStateEventManager.getInstance().onUpdate(field, tanks);
            }
        };

        webSocket.Connect();

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

}
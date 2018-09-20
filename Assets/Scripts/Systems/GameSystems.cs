using Leopotam.Ecs;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[EcsInject]
public class GameSystems : IEcsInitSystem, IEcsRunSystem, GameStateEventManager.Handler
{
    private static char WITHOUT_CHANGES = '#';
    private static int GAME_WAIT_FOR_DATA = 0;
    private static int GAME_STARTED = 1;

    EcsWorld _world = null;
    EcsFilter<Tank> _tanksFilter = null;
    EcsFilter<Wall> _wallsFilter = null;
    EcsFilter<UndestroyableWall> _undestroyableWallsFilter = null;
    EcsFilter<Bullet> _bulletsFilter = null;

    private int _gameState = GAME_WAIT_FOR_DATA;
    private string lastBattlefield;
    private int _fieldSize;
    private bool wasUpdated = false;

    WallProcessor wallProcessor;
    TankProcessor tanksProcessor;
    UndestroyableWallProcessor undestroyableWallProcessor;
    BulletProcessor bulletProcessor;

    List<FieldHandler> fieldHandlers = new List<FieldHandler>();

    void IEcsInitSystem.Initialize()
    {
        fieldHandlers.Add(new WallProcessor(_world, _wallsFilter));
        fieldHandlers.Add(new TankProcessor(_world, _tanksFilter));
        fieldHandlers.Add(new UndestroyableWallProcessor(_world, _undestroyableWallsFilter));
        fieldHandlers.Add(new BulletProcessor(_world, _bulletsFilter));
        GameStateEventManager.getInstance().subscribe(this);
        connectToServer();

        onUpdateBattlefield(BattleField.SAMPLE_SMALL);
    }

    void IEcsInitSystem.Destroy()
    {
        GameStateEventManager.getInstance().unSubscibe(this);
    }

    void IEcsRunSystem.Run()
    {
        if (!wasUpdated)
        {
            handleUpdates(BattleField.SAMPLE_SMALL_1);
            wasUpdated = true;
        }
    }

    private void connectToServer()
    {
        // TODO: connect with web socket
    }

    public void onUpdateBattlefield(string battlefield)
    {
        if (_gameState == GAME_WAIT_FOR_DATA)
        {
            initBattlefield(battlefield);
        }
        else
        {
            // TODO: update field
        }
    }

    private void initBattlefield(string battlefield)
    {
        char[][] field = BattleField.to2Dimension(battlefield);
        for (var i = 0; i < field.Length; i++)
        {
            for (var j = 0; j < field[i].Length; j++)
            {
                foreach(var handler in fieldHandlers)
                {
                    handler.initItem(field[i][j], i, j);
                }
            }
        }

        this.lastBattlefield = battlefield;

        _fieldSize = field.Length;

        foreach (var handler in fieldHandlers)
        {
            handler.setFieldSize(_fieldSize);
        }
    }

    private void handleUpdates(string newBattlefield)
    {
        char[] prev = lastBattlefield.ToCharArray();
        char[] next = newBattlefield.ToCharArray();
        for (var i = 0; i < newBattlefield.Length; i++)
        {
            if (prev[i] == next[i])
            {
                prev[i] = WITHOUT_CHANGES;
                next[i] = WITHOUT_CHANGES;
            }
        }
        char[][] prevArray = BattleField.to2Dimension(prev);
        char[][] nextArray = BattleField.to2Dimension(next);

        for (var i = 0; i < _fieldSize; i++)
        {
            for (var j = 0; j < _fieldSize; j++)
            {
                if (prevArray[i][j] != WITHOUT_CHANGES || nextArray[i][j]!=WITHOUT_CHANGES)
                {
                    foreach (var handler in fieldHandlers)
                    {
                        if (handler.canProcess(prevArray[i][j]) || (handler.canProcess(nextArray[i][j]))){
                            handler.onFieldUpdates(prevArray, nextArray, i, j);
                        }
                    }
                }
            }
        }
    }

}
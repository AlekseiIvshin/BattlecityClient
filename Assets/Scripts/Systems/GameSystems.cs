using Leopotam.Ecs;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[EcsInject]
public class GameSystems : IEcsInitSystem, IEcsRunSystem, GameStateEventManager.Handler
{
    public static int GAME_WAIT_FOR_DATA = 0;
    public static int GAME_STARTED = 1;

    EcsWorld _world = null;
    EcsFilter<Tank> _tanksFilter = null;
    EcsFilter<Wall> _wallsFilter = null;
    EcsFilter<UndestroyableWall> _undestroyableWallsFilter = null;
    EcsFilter<Bullet> _bulletsFilter = null;

    private int _gameState = GAME_WAIT_FOR_DATA;
    private char[][] lastBattlefield;
    private int _fieldSize;
    private bool wasUpdated = false;

    TankProcessor tanksProcessor;

    List<FieldHandler> fieldHandlers = new List<FieldHandler>();
    private Dictionary<char, string> _mapKeys;

    void IEcsInitSystem.Initialize()
    {
        tanksProcessor = new TankProcessor(_world, _tanksFilter);
        fieldHandlers.Add(new WallProcessor(_world, _wallsFilter));
        fieldHandlers.Add(new UndestroyableWallProcessor(_world, _undestroyableWallsFilter));
        fieldHandlers.Add(new BulletProcessor(_world, _bulletsFilter));
        GameStateEventManager.getInstance().subscribe(this);
    }

    void IEcsInitSystem.Destroy()
    {
        GameStateEventManager.getInstance().unSubscibe(this);
    }

    void IEcsRunSystem.Run()
    {
        if (!wasUpdated)
        {
            wasUpdated = true;
            handleUpdates(BattleField.to2Dimension(BattleField.SAMPLE_SMALL_1));
        }
    }

    public void onUpdate(char[][] battlefield, Dictionary<string, TankData> tanks)
    {
        if (_gameState == GAME_WAIT_FOR_DATA)
        {
            Debug.Log("INIT!");
            tanksProcessor.setMapKeys(FieldItems.MAP_KEYS);
            tanksProcessor.initTanks(tanks);
            initBattlefield(battlefield);
            _gameState = GAME_STARTED;
        }
        else
        {
            Debug.Log("UPDATE!");
            tanksProcessor.onUpdate(tanks);
            handleUpdates(battlefield);
        }
    }

    private void initBattlefield(char[][] field)
    {
        for (var i = 0; i < field.Length; i++)
        {
            for (var j = 0; j < field[i].Length; j++)
            {
                foreach (var handler in fieldHandlers)
                {
                    handler.setMapKeys(FieldItems.MAP_KEYS);
                    handler.initItem(field[i][j], i, j);
                }
            }
        }

        this.lastBattlefield = field;

        _fieldSize = field.Length;

        foreach (var handler in fieldHandlers)
        {
            handler.setFieldSize(_fieldSize);
        }
    }

    private void handleUpdates(char[][] newBattlefield)
    {
        char[][] prev = new char[_fieldSize][];
        char[][] next = new char[_fieldSize][];
        for (var i = 0; i < _fieldSize; i++)
        {
            prev[i] = new char[_fieldSize];
            next[i] = new char[_fieldSize];
            for (var j = 0; j < _fieldSize; j++)
            {
                if (prev[i][j] == next[i][j])
                {
                    prev[i][j] = FieldItems.WITHOUT_CHANGES;
                    next[i][j] = FieldItems.WITHOUT_CHANGES;
                }
            }
        }

        for (var i = 0; i < _fieldSize; i++)
        {
            for (var j = 0; j < _fieldSize; j++)
            {
                if (prev[i][j] != FieldItems.WITHOUT_CHANGES || next[i][j] != FieldItems.WITHOUT_CHANGES)
                {
                    foreach (var handler in fieldHandlers)
                    {
                        if (handler.canProcess(prev[i][j]) || (handler.canProcess(next[i][j])))
                        {
                            handler.onFieldUpdates(prev, next, i, j);
                        }
                    }
                }
            }
        }
    }
}
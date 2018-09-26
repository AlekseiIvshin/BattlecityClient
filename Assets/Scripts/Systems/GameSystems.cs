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
    private int _fieldSize;

    TankProcessor tanksProcessor;

    List<FieldHandler> fieldHandlers = new List<FieldHandler>();
    private Dictionary<char, string> _mapKeys;

    private List<BattlefieldState> _states = new List<BattlefieldState>();

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
        if (_gameState == GAME_WAIT_FOR_DATA && _states.Count == 1)
        {
            Debug.Log("INIT!");
            _gameState = GAME_STARTED;
            _fieldSize = _states[0].field.Length;
            tanksProcessor.setMapKeys(MapItems.MAP_KEYS);
            tanksProcessor.initTanks(_states[0].tanks);
            initBattlefield(_states[0].field);
        }
        else if (_states.Count > 1)
        {
            var _nextState = _states[1];
            var _prevState = _states[0];
            _states.RemoveAt(0);
            Debug.Log("UPDATE!");
            tanksProcessor.onUpdate(_nextState.tanks);
            handleUpdates(_prevState.field, _nextState.field);
        }
    }

    public void onUpdate(BattlefieldState state, long version)
    {
        _states.Add(state);
    }

    private void initBattlefield(char[][] field)
    {
        foreach (var handler in fieldHandlers)
        {
            handler.setMapKeys(MapItems.MAP_KEYS);
            handler.setFieldSize(_fieldSize);
        }
        for (var i = 0; i < field.Length; i++)
        {
            for (var j = 0; j < field[i].Length; j++)
            {
                foreach (var handler in fieldHandlers)
                {
                    handler.initItem(field[i][j], i, j);
                }
            }
        }
    }

    private void handleUpdates(char[][] prevBattlefield, char[][] newBattlefield)
    {
        char[][] prev;
        char[][] next;

        getChanges(prevBattlefield, newBattlefield, out prev, out next);

        for (var i = 0; i < _fieldSize; i++)
        {
            for (var j = 0; j < _fieldSize; j++)
            {
                if (prev[i][j] != MapItems.WITHOUT_CHANGES || next[i][j] != MapItems.WITHOUT_CHANGES)
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

    private static void getChanges(char[][] prevBattlefield, char[][] newBattlefield, out char[][] prev, out char[][] next)
    {
        prev = deepClone(prevBattlefield);
        next = deepClone(newBattlefield);
        var fieldSize = prevBattlefield.Length;
        for (var i = 0; i < fieldSize; i++)
        {
            for (var j = 0; j < fieldSize; j++)
            {
                if (prev[i][j] == next[i][j])
                {
                    prev[i][j] = MapItems.WITHOUT_CHANGES;
                    next[i][j] = MapItems.WITHOUT_CHANGES;
                }
            }
        }
    }

    private static char[][] deepClone(char[][] source)
    {
        var res = new char[source.Length][];
        for (var i = 0; i < source.Length; i++)
        {
            res[i] = new char[source[i].Length];
            for (var j = 0; j < source[i].Length; j++)
            {
                res[i][j] = source[i][j];
            }
        }
        return res;
    }
}
using Leopotam.Ecs;
using System;
using System.Collections.Generic;
using System.Text;
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
    EcsFilter<BattleWall> _battleWallsFilter = null;
    EcsFilter<Bullet> _bulletsFilter = null;
    EcsFilter<Medkit> _medkitFilter = null;
    EcsFilter<AmmoBox> _ammoBoxFilter = null;

    private int _gameState = GAME_WAIT_FOR_DATA;
    private int _fieldSize;

    TankProcessor tanksProcessor;

    List<FieldHandler> fieldHandlers = new List<FieldHandler>();
    List<IUpdatesApplier> updatesHandlers = new List<IUpdatesApplier>();
    private Dictionary<char, string> _mapKeys;

    private List<BattlefieldState> _states = new List<BattlefieldState>();

    void IEcsInitSystem.Initialize()
    {
        updatesHandlers.Add(
            new MapItemProcessor<BattleWall>(
                new ItemManagerDelegate<BattleWall>(_world, _battleWallsFilter, MapItems.PREFAB_BATTLE_WALL), 
                new UpdatesHandler(), 
                MapItems.PREFAB_TO_KEYS[MapItems.PREFAB_BATTLE_WALL]
            )
        );
        updatesHandlers.Add(
            new MapItemProcessor<Wall>(
                new WallManagerDelegate(_world, _wallsFilter, MapItems.PREFAB_WALL),
                new UpdatesHandler(), 
                MapItems.PREFAB_TO_KEYS[MapItems.PREFAB_WALL]
            )
        );
        updatesHandlers.Add(
            new MapItemProcessor<Bullet>(
                new BulletManagerDelegate(_world, _bulletsFilter, MapItems.PREFAB_BULLET),
                new BulletsUpdatesHandler(),
                MapItems.PREFAB_TO_KEYS[MapItems.PREFAB_BULLET]
            )
        );
        updatesHandlers.Add(
            new MapItemProcessor<Medkit>(
                new ItemManagerDelegate<Medkit>(_world, _medkitFilter, MapItems.PREFAB_MEDKIT),
                new UpdatesHandler(),
                MapItems.PREFAB_TO_KEYS[MapItems.PREFAB_MEDKIT]
            )
        );
        updatesHandlers.Add(
            new MapItemProcessor<AmmoBox>(
                new ItemManagerDelegate<AmmoBox>(_world, _ammoBoxFilter, MapItems.PREFAB_AMMO_BOX),
                new UpdatesHandler(),
                MapItems.PREFAB_TO_KEYS[MapItems.PREFAB_AMMO_BOX]
            )
        );

        tanksProcessor = new TankProcessor(_world, _tanksFilter);
        //fieldHandlers.Add(new WallProcessor(_world, _wallsFilter));
        //fieldHandlers.Add(new BulletProcessor(_world, _bulletsFilter));
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
            initBattlefield(_states[0]);
        }
        else if (_states.Count > 1)
        {
            var _nextState = _states[1];
            var _prevState = _states[0];
            _states.RemoveAt(0);
            Debug.Log("UPDATE!");
            //tanksProcessor.onUpdate(_nextState.tanks);
            handleUpdates(_prevState.field, _nextState.field);

            handleUpdates(_prevState, _nextState);

            foreach (var handler in updatesHandlers)
            {
                handler.applyUpdates();
            }
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

    private void initBattlefield(BattlefieldState state)
    {
        var symbols = generateSymbols(MapItems.MAP_KEYS);
        foreach (var handler in updatesHandlers)
        {
            handler.setSymbols(symbols[handler.prefabName()]);
        }

        for (var i = 0; i < state.field.Length; i++)
        {
            for (var j = 0; j < state.field[i].Length; j++)
            {
                foreach (var handler in updatesHandlers)
                {
                    handler.initItem(state, i, j);
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

    private void handleUpdates(BattlefieldState prev, BattlefieldState next)
    {
        for (var i = 0; i < _fieldSize; i++)
        {
            for (var j = 0; j < _fieldSize; j++)
            {
                foreach (var handler in updatesHandlers)
                {
                    handler.accumulateUpdates(prev, next, i, j);
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

    private static Dictionary<string, string> generateSymbols(Dictionary<char, string> mapKeys)
    {
        var map = new Dictionary<string, string>();
        foreach (var key in MapItems.PREFAB_TO_KEYS.Keys)
        {
            map.Add(key, generateSymbols(mapKeys, key));
        }
        return map;
    }
    private static string generateSymbols(Dictionary<char, string> mapKeys, string prefab)
    {
        var map = new Dictionary<string, string>();
        StringBuilder symbols = new StringBuilder();
        var keys = new List<string>(MapItems.PREFAB_TO_KEYS[prefab]);
        foreach (var key in mapKeys.Keys)
        {
            if (keys.IndexOf(mapKeys[key]) >= 0)
            {
                symbols.Append(key);
            }
        }
        return symbols.ToString();
    }
}
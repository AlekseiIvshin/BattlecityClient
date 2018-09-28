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
    EcsFilter<Hedgehog> _hedgehogFilter = null;
    EcsFilter<Sand> _sandFilter = null;
    EcsFilter<Bog> _bogFilter = null;
    EcsFilter<Moat> _moatFilter = null;
    EcsFilter<WormHole> _wormHoleFilter = null;

    private int _gameState = GAME_WAIT_FOR_DATA;
    private int _fieldSize;

    List<IUpdatesApplier> updatesHandlers = new List<IUpdatesApplier>();
    private Dictionary<char, string> _mapKeys;

    private List<BattlefieldState> _states = new List<BattlefieldState>();

    private TankItemProcessor _tanksProcessor;

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
                new PickUpManagerDelegate<Medkit>(_world, _medkitFilter, MapItems.PREFAB_MEDKIT),
                new UpdatesHandler(),
                MapItems.PREFAB_TO_KEYS[MapItems.PREFAB_MEDKIT]
            )
        );
        updatesHandlers.Add(
            new MapItemProcessor<AmmoBox>(
                new PickUpManagerDelegate<AmmoBox>(_world, _ammoBoxFilter, MapItems.PREFAB_AMMO_BOX),
                new UpdatesHandler(),
                MapItems.PREFAB_TO_KEYS[MapItems.PREFAB_AMMO_BOX]
            )
        );
        updatesHandlers.Add(
            new MapItemProcessor<Hedgehog>(
                new ObstacleManagerDelegate<Hedgehog>(_world, _hedgehogFilter, MapItems.PREFAB_HEDGEHOG),
                new UpdatesHandler(),
                MapItems.PREFAB_TO_KEYS[MapItems.PREFAB_HEDGEHOG]
            )
        );
        updatesHandlers.Add(
            new MapItemProcessor<Sand>(
                new ObstacleManagerDelegate<Sand>(_world, _sandFilter, MapItems.PREFAB_SAND),
                new UpdatesHandler(),
                MapItems.PREFAB_TO_KEYS[MapItems.PREFAB_SAND]
            )
        );
        updatesHandlers.Add(
            new MapItemProcessor<Bog>(
                new ObstacleManagerDelegate<Bog>(_world, _bogFilter, MapItems.PREFAB_BOG),
                new UpdatesHandler(),
                MapItems.PREFAB_TO_KEYS[MapItems.PREFAB_BOG]
            )
        );
        updatesHandlers.Add(
            new MapItemProcessor<Moat>(
                new MoatManagerDelegate(_world, _moatFilter, MapItems.PREFAB_MOAT),
                new UpdatesHandler(),
                MapItems.PREFAB_TO_KEYS[MapItems.PREFAB_MOAT]
            )
        );
        updatesHandlers.Add(
            new MapItemProcessor<WormHole>(
                new ObstacleManagerDelegate<WormHole>(_world, _wormHoleFilter, MapItems.PREFAB_WORM_HOLE),
                new UpdatesHandler(),
                MapItems.PREFAB_TO_KEYS[MapItems.PREFAB_WORM_HOLE]
            )
        );
        _tanksProcessor = 
            new TankItemProcessor(
                new TanksManagerDelegate(_world, _tanksFilter, MapItems.PREFAB_TANK),
                new TanksUpdateHandler(),
                MapItems.PREFAB_TO_KEYS[MapItems.PREFAB_TANK]
        );

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
            _tanksProcessor.initItems(_states[0]);
            initBattlefield(_states[0]);
        }
        else if (_states.Count > 1)
        {
            var _nextState = _states[1];
            var _prevState = _states[0];
            _states.RemoveAt(0);
            Debug.Log("UPDATE!");
            handleUpdates(_prevState, _nextState);
            foreach (var handler in updatesHandlers)
            {
                handler.applyUpdates();
            }
            _tanksProcessor.accumulateUpdates(_prevState, _nextState);
            _tanksProcessor.applyUpdates();
        }
    }

    public void onUpdate(BattlefieldState state, long version)
    {
        _states.Add(state);
    }

    private void initBattlefield(BattlefieldState state)
    {
        var symbols = generateSymbols(MapItems.MAP_KEYS);
        foreach (var handler in updatesHandlers)
        {
            handler.setSymbols(symbols[handler.prefabName()]);
        }
        _tanksProcessor.setSymbols(symbols[_tanksProcessor.prefabName()]);

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
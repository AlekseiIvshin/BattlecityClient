﻿using Leopotam.Ecs;
using UnityEngine;

public class GameStartup : MonoBehaviour
{
    EcsWorld _world;

    EcsSystems _systems;

    void OnEnable()
    {
        _world = new EcsWorld();
#if UNITY_EDITOR
        Leopotam.Ecs.UnityIntegration.EcsWorldObserver.Create (_world);
#endif  

        var sharedData = EcsFilterSingle<SharedGameState>.Create(_world);

        _systems = new EcsSystems(_world)
            .Add(new GameSystems())
            .Add(new TanksControlSystem())
            .Add(new BulletControlSystem())
            .Add(new DataSystem(this))
            .Add(new MainCameraSystem());/*
            .Add(new NpcMovementSystem())
            .Add(new PlayerMovementSystem())
            .Add(new DamageSystem())
            .Add(new SquadSystem())
            .Add(new SelectionSystem());*/
        _systems.Initialize();
#if UNITY_EDITOR
        Leopotam.Ecs.UnityIntegration.EcsSystemsObserver.Create (_systems);
#endif
    }

    private void FixedUpdate()
    {
        _systems.Run();
    }

    void OnDisable()
    {
        _systems.Dispose();
    }
}
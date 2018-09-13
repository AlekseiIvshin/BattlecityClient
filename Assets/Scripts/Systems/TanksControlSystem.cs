using Leopotam.Ecs;
using UnityEngine;

[EcsInject]
public class TanksControlSystem : IEcsInitSystem, IEcsRunSystem
{
    EcsWorld _world = null;
    EcsFilter<Tank> _tanksFilter = null;

    void IEcsInitSystem.Initialize()
    {
        Tank entity = null;
        foreach (var unityObject in GameObject.FindGameObjectsWithTag("Tank"))
        {
            entity = SystemUtils.getComponent<Tank>(_world, unityObject);
            entity.transform = unityObject.transform;
            entity.characterController = unityObject.GetComponent<CharacterController>();
        }
    }

    void IEcsInitSystem.Destroy() { }

    void IEcsRunSystem.Run()
    {
        for (var index = 0; index < _tanksFilter.EntitiesCount; index++)
        {
            var tank = _tanksFilter.Components1[index];
            // Debug.Log("Tank " + tank.transform.position.x+", " + tank.transform.position.x);
        }
    }
}
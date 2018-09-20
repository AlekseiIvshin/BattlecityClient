using Leopotam.Ecs;
using UnityEngine;

[EcsInject]
public class TanksControlSystem : IEcsInitSystem, IEcsRunSystem
{
    EcsWorld _world = null;
    EcsFilter<Tank> _tanksFilter = null;

    private const float rotationSpeed = 50f;
    private const float movementSpeed = 2f;

    void IEcsInitSystem.Initialize()
    {
    }

    void IEcsInitSystem.Destroy() { }

    void IEcsRunSystem.Run()
    {
        for (var index = 0; index < _tanksFilter.EntitiesCount; index++)
        {
            var tank = _tanksFilter.Components1[index];
            if (tank.deltas.Count > 0)
            {
                if (tank.deltas[0].isRotaion)
                {
                    if (tank.transform.rotation == tank.deltas[0].rotationTarget)
                    {
                        tank.deltas.RemoveAt(0);
                    } else
                    {
                        float step = rotationSpeed * Time.deltaTime;
                        tank.transform.rotation = Quaternion.RotateTowards(tank.transform.rotation, tank.deltas[0].rotationTarget, step);
                    }
                } else
                {
                    if (tank.transform.position == tank.deltas[0].positionTarget)
                    {
                        tank.deltas.RemoveAt(0);
                    }
                    else
                    {
                        float step = movementSpeed * Time.deltaTime;
                        tank.transform.position = Vector3.MoveTowards(tank.transform.position, tank.deltas[0].positionTarget,step);
                    }
                }
            }
        }
    }
}
using Leopotam.Ecs;
using UnityEngine;

[EcsInject]
public class TanksControlSystem : IEcsInitSystem, IEcsRunSystem
{
    EcsFilter<Tank> _tanksFilter = null;
    EcsFilterSingle<MainCamera> _cameraFilter = null;

    const int actionsPerStep = 3;

    MainCamera camera;

    void IEcsInitSystem.Initialize()
    {
    }

    void IEcsInitSystem.Destroy() { }

    void IEcsRunSystem.Run()
    {
        var movementSpeed = Time.deltaTime * 1000 * MapUtils.tileSize / ClientState.tickTime * actionsPerStep;
        var rotationSpeed = Time.deltaTime * 1000 * 180 / ClientState.tickTime * actionsPerStep;
        for (var index = 0; index < _tanksFilter.EntitiesCount; index++)
        {
            var tank = _tanksFilter.Components1[index];
            tank.cloud.transform.LookAt(_cameraFilter.Data.camera.transform.position);
            if (tank.deltas.Count > 0)
            {
                if (tank.deltas[0].isRotaion)
                {
                    if (tank.transform.rotation == tank.deltas[0].rotationTarget)
                    {
                        tank.deltas.RemoveAt(0);
                    } else
                    {
                        tank.transform.rotation = Quaternion.RotateTowards(tank.transform.rotation, tank.deltas[0].rotationTarget, rotationSpeed);
                    }
                } else if (tank.deltas[0].wasTeleported) {
                    tank.transform.position = tank.deltas[0].positionTarget;
                    // TODO: Start animation
                    tank.deltas.RemoveAt(0);
                }
                else
                {
                    if (tank.transform.position == tank.deltas[0].positionTarget)
                    {
                        tank.deltas.RemoveAt(0);
                    }
                    else
                    {
                        tank.transform.position = Vector3.MoveTowards(tank.transform.position, tank.deltas[0].positionTarget, movementSpeed);
                    }
                }
            }
        }
    }
}
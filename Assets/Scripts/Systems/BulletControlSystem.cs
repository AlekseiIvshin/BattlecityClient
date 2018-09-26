using Leopotam.Ecs;
using System.Collections.Generic;
using UnityEngine;

[EcsInject]
public class BulletControlSystem : IEcsInitSystem, IEcsRunSystem, ObjectDestroyEventManager.Handler
{
    EcsWorld _world = null;
    EcsFilter<Bullet> _bulletsFilter = null;
    List<int> _bulletsToDestroy = new List<int>();

    private const float movementSpeed = 2f;

    void IEcsInitSystem.Initialize()
    {
        ObjectDestroyEventManager.getInstance().subscribe(this);
    }

    void IEcsInitSystem.Destroy()
    {
        ObjectDestroyEventManager.getInstance().unSubscibe(this);
    }

    void IEcsRunSystem.Run()
    {
        for (var index = 0; index < _bulletsFilter.EntitiesCount; index++)
        {
            var bullet = _bulletsFilter.Components1[index];
            bullet.transform.position = Vector3.MoveTowards(bullet.transform.position, bullet.expectedPosition, movementSpeed * Time.deltaTime);
        }

        if (_bulletsToDestroy.Count>0)
        {

            Bullet bullet;
            for (var i = 0; i < _bulletsFilter.EntitiesCount; i++)
            {
                bullet = _bulletsFilter.Components1[i];
                if (_bulletsToDestroy.IndexOf(bullet.entityId)>=0)
                {
                    if (bullet.transform != null)
                    {
                        Object.Destroy(bullet.transform.gameObject);
                    }
                    bullet.transform = null;
                    _world.RemoveEntity(bullet.entityId);
                    break;
                }
            }
            _bulletsToDestroy.Clear();
        }
    }

    public void onDestroyEntity(int entityId)
    {
        _bulletsToDestroy.Add(entityId);
    }
}
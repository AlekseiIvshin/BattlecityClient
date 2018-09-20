using Leopotam.Ecs;
using UnityEngine;

[EcsInject]
public class BulletControlSystem : IEcsInitSystem, IEcsRunSystem, ObjectDestroyEventManager.Handler
{
    EcsWorld _world = null;
    EcsFilter<Bullet> _bulletsFilter = null;

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
    }

    public void onDestroyEntity(int entityId)
    {
        Bullet bullet;
        for(var i = 0;i < _bulletsFilter.EntitiesCount;i++)
        {
            bullet = _bulletsFilter.Components1[i];
            if (bullet.entityId == entityId)
            {
                GameObject.Destroy(bullet.transform.gameObject);
                bullet.transform = null;
                _world.RemoveEntity(entityId);
                break;
            }
        }
    }
}
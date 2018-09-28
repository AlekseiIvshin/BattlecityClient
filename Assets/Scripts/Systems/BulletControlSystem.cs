using Leopotam.Ecs;
using System.Collections.Generic;
using UnityEngine;

[EcsInject]
public class BulletControlSystem : IEcsInitSystem, IEcsRunSystem, ObjectDestroyEventManager.Handler
{
    EcsWorld _world = null;
    EcsFilter<Bullet> _bulletsFilter = null;
    List<int> _bulletsToDestroy = new List<int>();

    private float _movementSpeed = 0;

    void IEcsInitSystem.Initialize()
    {
        _movementSpeed = MapUtils.tileSize * 2 / ClientState.tickTime * 1000;
        ObjectDestroyEventManager.getInstance().subscribe(this);
    }

    void IEcsInitSystem.Destroy()
    {
        ObjectDestroyEventManager.getInstance().unSubscibe(this);
    }

    void IEcsRunSystem.Run()
    {
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
                        GameObject explosion = Object.Instantiate(
                            Resources.Load("Effects/WFX_Explosion") as GameObject,
                            bullet.transform.position + Vector3.up*1.5f,
                            bullet.transform.rotation
                        ) as GameObject;
                        Object.Destroy(explosion, 5f);
                        Object.Destroy(bullet.transform.gameObject);
                    }
                    bullet.transform = null;
                    Debug.Log("Bullet was destroyed: " + bullet.entityId + ", ('" + bullet.row + "','" + bullet.column + "')");
                    _world.RemoveEntity(bullet.entityId);
                    break;
                }
            }
            _bulletsToDestroy.Clear();
        }

        Bullet updateBullet;
        for (var i = 0; i < _bulletsFilter.EntitiesCount; i++)
        {
            updateBullet = _bulletsFilter.Components1[i];
            if (updateBullet.transform != null)
            {
                updateBullet.transform.Translate(Vector3.forward * Time.deltaTime * 1000 * MapUtils.tileSize * 2/ ClientState.tickTime);
            }
        }
    }

    public void onDestroyEntity(int entityId)
    {
        _bulletsToDestroy.Add(entityId);
    }
}
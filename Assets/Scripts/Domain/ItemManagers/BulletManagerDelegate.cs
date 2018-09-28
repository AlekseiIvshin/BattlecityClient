using UnityEngine;
using UnityEditor;
using Leopotam.Ecs;

public class BulletManagerDelegate : ItemManagerDelegate<Bullet>
{

    public BulletManagerDelegate(EcsWorld world, EcsFilter<Bullet> filter, string prefabName) : base(world, filter, prefabName)
    {
    }

    public override Quaternion getDirection(MapItem item)
    {
        return MapUtils.getWorlRotation(MapUtils.getBulletDirection(item.symbol));
    }

    public override Bullet createItem(MapItem item)
    {
        var bullet = base.createItem(item);
        bullet.direction = MapUtils.getBulletDirection(item.symbol);
        //var rigidbody = bullet.transform.GetComponent<Rigidbody>();
        //rigidbody.velocity = MapUtils.getBulletVectorDirection(item.symbol);
        return bullet;
    }

    public override bool updateItem(MapItem prev, MapItem next)
    {
        if (!canProcess(prev.symbol) && canProcess(next.symbol))
        {
            createItem(next);
        }

        // Case when prev is bullet and next is not, handled via colliders and ObjectDestroyEventManager
        return true;
    }
}
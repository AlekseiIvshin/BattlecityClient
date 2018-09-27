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
        var positionDelta = MapUtils.calculatePositionDelta(bullet.direction, 2);
        bullet.expectedPosition = MapUtils.mapToWorld(item.row + positionDelta.rowDelta, item.column + positionDelta.columnDelta);
        return bullet;
    }

    public override bool updateItem(MapItem prev, MapItem next)
    {
        Debug.Log("Update bullet: prev " + prev + "; next " + next);
        if (next.symbol == MapItems.OUTBOUNDS)
        {
            destroyItem(prev.row, prev.column);
            return true;
        }
        if (canProcess(next.symbol) && canProcess(prev.symbol) && next.symbol != prev.symbol)
        {
            createItem(next);
            destroyItem(prev.row, prev.column);
            return true;
        }
        var bullet = findByPosition(prev.row, prev.column);
        if (bullet != null)
        {
            bullet.expectedPosition = MapUtils.mapToWorld(next.row, next.column);
            bullet.column = next.column;
            bullet.row = next.row;
            return true;
        }
        createItem(next);

        // Case when prev is bullet and next is not, handled via colliders and ObjectDestroyEventManager
        return true;
    }
}
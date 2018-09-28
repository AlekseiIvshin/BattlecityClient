using UnityEngine;
using UnityEditor;
using Leopotam.Ecs;

public class TanksManagerDelegate : ItemManagerDelegate<Tank>
{
    private static int getDirectionForMovement(int rowDelta, int columnDelta, int defaultValue)
    {
        if (rowDelta < 0)
        {
            return MapUtils.DIRECTION_DOWN;
        }
        else if (rowDelta > 0)
        {
            return MapUtils.DIRECTION_UP;
        }
        else if (columnDelta < 0)
        {
            return MapUtils.DIRECTION_LEFT;
        }
        else if (columnDelta > 0)
        {
            return MapUtils.DIRECTION_RIGHT;
        }
        return defaultValue;
    }

    public TanksManagerDelegate(EcsWorld world, EcsFilter<Tank> filter, string prefabName) : base(world, filter, prefabName)
    {
    }

    public override Quaternion getDirection(MapItem item)
    {
        return MapUtils.getWorlRotation(MapUtils.getTankDirection(item.symbol));
    }

    public override Tank createItem(MapItem item)
    {
        throw new System.Exception("Do not use it!");
    }

    public Tank createItem(string name, MapItem item)
    {
        var tank = base.createItem(item);
        tank.direction = MapUtils.getTankDirection(item.symbol);
        tank.name = name;
        return tank;
    }

    public override bool updateItem(MapItem prev, MapItem next)
    {
        throw new System.Exception("Do not use it!");
    }

    public bool updateItem(string name, MapItem prev, MapItem next)
    {
        if (next == null)
        {
            destroyItem(prev.row, prev.column);
            return true;
        }
        if (prev == null)
        {
            createItem(next);
            return true;
        }

        Tank tank = findByName(name);

        if (tank == null)
        {
            createItem(name, next);
            return true;
        }


        int moveByRow = tank.row - next.row;
        int moveByColumn = tank.column - next.column;
        int rotateBy = 0;
        int movementDirection;
        if (moveByColumn != 0 || moveByRow != 0)
        {
            if (Mathf.Abs(moveByColumn) > 1 || Mathf.Abs(moveByRow) > 1)
            {
                tank.deltas.Add(TankDelta.teleportTo(MapUtils.mapToWorld(next.row, next.column)));
            }
            else
            {
                movementDirection = getDirectionForMovement(moveByRow, -moveByColumn, rotateBy);
                tank.deltas.Add(TankDelta.rotateToDirection(movementDirection));
                tank.deltas.Add(TankDelta.moveTo(MapUtils.mapToWorld(next.row, next.column)));
                tank.row = next.row;
                tank.column = next.column;
            }
        }
        tank.deltas.Add(TankDelta.rotateToDirection(MapUtils.getTankDirection(next.symbol)));
        return true;
    }

    public Tank findByName(string name)
    {
        Tank tank = null;
        for (var entityId = 0; entityId < _filter.EntitiesCount; entityId++)
        {
            tank = _filter.Components1[entityId];
            if (tank.name == name)
            {
                return tank;
            }
        }
        return null;
    }
}
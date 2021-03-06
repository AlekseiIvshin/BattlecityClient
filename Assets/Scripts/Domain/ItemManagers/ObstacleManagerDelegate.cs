﻿using UnityEngine;
using UnityEditor;
using Leopotam.Ecs;

public class ObstacleManagerDelegate<T>: ItemManagerDelegate<T> where T : BaseEntity, new()
{

    public ObstacleManagerDelegate(EcsWorld world, EcsFilter<T> filter, string prefabName) : base(world, filter, prefabName)
    {
    }

    public override bool updateItem(MapItem prev, MapItem next)
    {
        if (MapItems.doesSymbolBelongToItems(next.symbol, new string[] { MapItems.PREFAB_TANK, MapItems.PREFAB_BULLET })
            || MapItems.doesSymbolBelongToItems(prev.symbol, new string[] { MapItems.PREFAB_TANK, MapItems.PREFAB_BULLET }))
        {
            // Do nothing until bullet or tank on obstacle
            return true;
        }
        if (!canProcess(prev.symbol) && canProcess(next.symbol) && findByPosition(prev.row, prev.column) == null)
        {
            // Hack: when on field is tank and item -> server returns only tank symbol
            // Hack: same for bullets 
            // If before there wasn't item and it added, then create item
            createItem(next);
            return true;
        }
        else if (canProcess(prev.symbol) && MapItems.MAP_KEYS[next.symbol] == MapItems.KEY_NONE)
        {
            // Hack: when on field is tank and item -> server returns only tank symbol
            // Hack: same for bullets
            // If before there was item and it became none field, then destroy item
            destroyItem(prev.row, prev.column);
            return true;
        }
        return false;
    }
}
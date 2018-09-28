using UnityEngine;
using UnityEditor;
using Leopotam.Ecs;

public class PickUpManagerDelegate<T>: ItemManagerDelegate<T> where T : BaseEntity, new()
{

    public PickUpManagerDelegate(EcsWorld world, EcsFilter<T> filter, string prefabName) : base(world, filter, prefabName)
    {
    }

    public override bool updateItem(MapItem prev, MapItem next)
    {
        if (MapItems.doesSymbolBelongToItem(next.symbol, MapItems.PREFAB_BULLET)
           || MapItems.doesSymbolBelongToItem(prev.symbol, MapItems.PREFAB_BULLET))
        {
            // Do nothing until bullet or tank on obstacle
            return true;
        }
        if (!canProcess(prev.symbol) && canProcess(next.symbol))
        {
            // Hack: when on field is tank and item -> server returns only tank symbol
            // If before there wasn't item and it added, then create item
            createItem(next);
            return true;
        }
        else if (canProcess(prev.symbol) && (MapItems.MAP_KEYS[next.symbol] == MapItems.KEY_NONE || MapItems.doesSymbolBelongToItem(next.symbol, MapItems.PREFAB_TANK)))
        {
            // Hack: when on field is tank and item -> server returns only tank symbol
            // If before there was item and it became none field, then destroy item
            destroyItem(prev.row, prev.column);
            return true;
        }
        return false;
    }
}
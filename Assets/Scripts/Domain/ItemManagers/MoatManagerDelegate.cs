using UnityEngine;
using UnityEditor;
using Leopotam.Ecs;

public class MoatManagerDelegate: ObstacleManagerDelegate<Moat>
{

    public MoatManagerDelegate(EcsWorld world, EcsFilter<Moat> filter, string prefabName) : base(world, filter, prefabName)
    {
    }

    public override Quaternion getDirection(MapItem item)
    {
        return MapUtils.getWorldRotation(MapUtils.getMoatDirection(item.symbol));
    }
}
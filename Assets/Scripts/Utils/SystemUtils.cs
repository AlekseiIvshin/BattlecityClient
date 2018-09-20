using Leopotam.Ecs;
using UnityEngine;

public class SystemUtils
{

    public static T getComponent<T>(EcsWorld world, GameObject gameObject, out int entityId) where T : class, new()
    {
        var entityMetaHolder = gameObject.GetComponent<EntityMetaHolder>();
        entityId = entityMetaHolder.getEntityId();
        if (entityId >= 0)
        {
            return world.AddComponent<T>(entityId);
        }
        T component;
        entityId = world.CreateEntityWith<T>(out component);
        entityMetaHolder.setEntityId(entityId);
        return component;
    }
}

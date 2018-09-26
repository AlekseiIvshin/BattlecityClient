using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyEventManager {

    public interface Handler
    {
        void onDestroyEntity(int entityId);
    }

    private static ObjectDestroyEventManager instance = new ObjectDestroyEventManager();

    public static ObjectDestroyEventManager getInstance()
    {
        return instance;
    }

    private List<Handler> handlers = new List<Handler>();

    public void subscribe(Handler handler)
    {
        handlers.Add(handler);
    }

    public void unSubscibe(Handler handler)
    {
        handlers.Remove(handler);
    }

    public void destroyEntity(int entityId)
    {
        foreach (var handler in handlers)
        {
            //handler.onDestroyEntity(entityId);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateEventManager  {

    public interface Handler
    {
        void onUpdate(BattlefieldState state, long version);
    }

    private static GameStateEventManager instance = new GameStateEventManager();

    public static GameStateEventManager getInstance()
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

    public void onUpdate(BattlefieldState state, long version)
    {
        foreach(var handler in handlers)
        {
            handler.onUpdate(state, version);
        }
    }
}

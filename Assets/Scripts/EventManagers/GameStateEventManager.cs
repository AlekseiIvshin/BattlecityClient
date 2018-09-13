using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateEventManager  {

    public interface Handler
    {
        void onUpdateBattlefield(string battlefield);
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

    public void onUpdatedBattlefield(string battlefield)
    {
        foreach(var handler in handlers)
        {
            handler.onUpdateBattlefield(battlefield);
        }
    }
}

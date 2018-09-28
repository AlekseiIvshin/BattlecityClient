using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class TanksUpdateHandler
{
    private List<TankMapItemDelta> _updates = new List<TankMapItemDelta>();

    private string _symbols;

    public void setSymbols(string symbols)
    {
        this._symbols = symbols;
    }
    
    public void accumulateUpdates(BattlefieldState prev, BattlefieldState next)
    {
        accumulateNewTanks(prev, next);
        accumulateDestroyedTanks(prev, next);
        accumulateUpdatedTanks(prev, next);
    }

    private void accumulateNewTanks(BattlefieldState prev, BattlefieldState next)
    {
        foreach (var name in next.tanks.Keys)
        {
            if (!prev.tanks.ContainsKey(name)){
                _updates.Add(new TankMapItemDelta { name= name, prev = null, next = next.tanks[name] });
            }
        }
    }

    private void accumulateDestroyedTanks(BattlefieldState prev, BattlefieldState next)
    {
        foreach (var name in prev.tanks.Keys)
        {
            if (!next.tanks.ContainsKey(name))
            {
                _updates.Add(new TankMapItemDelta { name = name, prev = prev.tanks[name], next = null });
            }
        }
    }

    private void accumulateUpdatedTanks(BattlefieldState prev, BattlefieldState next)
    {
        foreach (var name in prev.tanks.Keys)
        {
            if (next.tanks.ContainsKey(name))
            {
                _updates.Add(new TankMapItemDelta { name = name, prev = prev.tanks[name], next = next.tanks[name] });
            }
        }
    }

    public TankMapItemDelta[] getUpdatesAndClear()
    {
        TankMapItemDelta[] updates = new TankMapItemDelta[_updates.Count];
        _updates.CopyTo(updates);
        _updates.Clear();
        return updates;
    }
}
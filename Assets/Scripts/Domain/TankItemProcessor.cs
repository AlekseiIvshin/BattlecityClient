using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.Ecs;
using UnityEditor;
using System.Text;

public class TankItemProcessor
{
    private List<string> _itemKeys;
    private TanksManagerDelegate _itemManagerDelegate;
    private TanksUpdateHandler _accumulator;
    private string _symbols;

    public TankItemProcessor(TanksManagerDelegate itemManagerDelegate, TanksUpdateHandler accumulator, IEnumerable<string> itemKeys)
    {
        this._itemKeys = new List<string>(itemKeys);
        this._itemManagerDelegate = itemManagerDelegate;
        this._accumulator = accumulator;
    }

    public bool canProcess(char symbol)
    {
        return _symbols.IndexOf(symbol) >= 0;
    }

    protected virtual Quaternion getDirection(char symbol)
    {
        return MapUtils.getWorlRotation(MapUtils.getTankDirection(symbol));
    }

    public virtual void initItems(BattlefieldState state)
    {
        foreach (var tankName in state.tanks.Keys)
        {
            _itemManagerDelegate.createItem(tankName, state.tanks[tankName]);
        }
    }

    protected virtual Tank createItem(string name, MapItem item)
    {
        return this._itemManagerDelegate.createItem(name, item);
    }

    public void applyUpdates()
    {
        var updates = this._accumulator.getUpdatesAndClear();
        foreach (var update in updates)
        {
            this._itemManagerDelegate.updateItem(update.name, update.prev, update.next);
        }
    }

    public void accumulateUpdates(BattlefieldState prev, BattlefieldState next)
    {
        this._accumulator.accumulateUpdates(prev, next);
    }

    public string prefabName()
    {
        return _itemManagerDelegate.prefabName();
    }

    public void setSymbols(string symbols)
    {
        this._symbols = symbols.ToString();
        this._accumulator.setSymbols(this._symbols);
        this._itemManagerDelegate.setSymbols(this._symbols);
    }

    public MapItemDelta[] getUpdatesAndClear()
    {
        return _accumulator.getUpdatesAndClear();
    }

}

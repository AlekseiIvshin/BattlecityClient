using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.Ecs;
using UnityEditor;
using System.Text;

public class MapItemProcessor<T>: IUpdatesApplier where T : BaseEntity, new()
{
    private List<string> _itemKeys;
    private ItemManagerDelegate<T> _itemManagerDelegate;
    private IUpdatesAccumulator _accumulator;
    private string _symbols;

    public MapItemProcessor(ItemManagerDelegate<T> itemManagerDelegate, IUpdatesAccumulator accumulator, IEnumerable<string> itemKeys)
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
        return Quaternion.Euler(0, 0, 0);
    }

    protected virtual T createItem(MapItem item)
    {
        return this._itemManagerDelegate.createItem(item);
    }

    public void applyUpdates()
    {
        var updates = this._accumulator.getUpdatesAndClear();
        foreach(var update in updates)
        {
            this._itemManagerDelegate.updateItem(update.prev, update.next);
        }
    }

    public void accumulateUpdates(BattlefieldState prev, BattlefieldState next, int row, int column)
    {
        this._accumulator.accumulateUpdates(prev, next, row, column);
    }

    public virtual void initItem(BattlefieldState state, int row, int column)
    {
        if (canProcess(state.field[row][column]))
        {
            this._itemManagerDelegate.createItem(new MapItem
            {
                symbol = state.field[row][column],
                row = row,
                column = column
            });
        }
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

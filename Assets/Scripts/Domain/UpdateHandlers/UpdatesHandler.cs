using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class UpdatesHandler: IUpdatesAccumulator
{
    private List<MapItemDelta> _updates = new List<MapItemDelta>();

    private string _symbols;

    public void setSymbols(string symbols)
    {
        this._symbols = symbols;
    }
    
    public void accumulateUpdates(BattlefieldState prev, BattlefieldState next, int row, int column)
    {
        if (wasChanged(prev, next, row, column))
        {
            _updates.Add(getUpdates(prev, next, row, column));
        }
    }

    public MapItemDelta[] getUpdatesAndClear()
    {
        MapItemDelta[] updates = new MapItemDelta[_updates.Count];
        _updates.CopyTo(updates);
        _updates.Clear();
        return updates;
    }

    protected virtual bool wasChanged(BattlefieldState prev, BattlefieldState next, int row, int column)
    {
        return (_symbols.IndexOf(prev.field[row][column]) >= 0 || _symbols.IndexOf(next.field[row][column]) >= 0) &&
            prev.field[row][column] != next.field[row][column];
    }

    protected virtual MapItemDelta getUpdates(BattlefieldState prev, BattlefieldState next, int row, int column)
    {
        
        return new MapItemDelta
        {
            prev = new MapItem
            {
                row = row,
                column = column,
                symbol = prev.field[row][column]
            },
            next = new MapItem
            {
                row = row,
                column = column,
                symbol = next.field[row][column]
            }
        };
    }
}
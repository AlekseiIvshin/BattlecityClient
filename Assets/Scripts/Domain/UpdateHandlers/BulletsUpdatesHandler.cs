using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BulletsUpdatesHandler: IUpdatesAccumulator
{
    private Dictionary<string, MapItemDelta> _updates = new Dictionary<string, MapItemDelta>();

    private string _symbols;

    public void setSymbols(string symbols)
    {
        this._symbols = symbols;
    }
    
    public void accumulateUpdates(BattlefieldState prev, BattlefieldState next, int row, int column)
    {
        var fieldSize = prev.field.Length;
        var prevIsBullet = _symbols.IndexOf(prev.field[row][column]) >= 0;
        var nextIsBullet = _symbols.IndexOf(next.field[row][column]) >= 0;
        if (prevIsBullet)
        {
            var moveDelta = MapUtils.calculatePositionDelta(MapUtils.getBulletDirection(prev.field[row][column]), 2);
            int nextRow;
            int nextColumn;
            var isInsideField = MapUtils.getCoordinatesWithDelta(fieldSize, row, column, moveDelta, out nextRow, out nextColumn);
            addUpdate(new MapItem
                {
                    symbol = prev.field[row][column],
                    row = row,
                    column = column
                },
                new MapItem
                {
                    symbol = isInsideField ? next.field[nextRow][nextColumn] : MapItems.OUTBOUNDS,
                    row = nextRow,
                    column = nextColumn
                }
            );
        } else if (nextIsBullet)
        {
            var moveDelta = MapUtils.calculatePositionDelta(MapUtils.getBulletDirection(next.field[row][column]), -2);
            int prevRow;
            int prevColumn;
            var isInsideField = MapUtils.getCoordinatesWithDelta(fieldSize, row, column, moveDelta, out prevRow, out prevColumn);
            addUpdate(new MapItem
                {
                    symbol = isInsideField ? prev.field[prevRow][prevColumn] : MapItems.OUTBOUNDS,
                    row = prevRow,
                    column = prevColumn
                },
                new MapItem
                {
                    symbol = next.field[row][column],
                    row = row,
                    column = column
                }
            );
        }
    }

    public MapItemDelta[] getUpdatesAndClear()
    {
        MapItemDelta[] updates = new MapItemDelta[_updates.Count];
        new List<MapItemDelta>( _updates.Values).CopyTo(updates);
        _updates.Clear();
        return updates;
    }

    private void addUpdate(MapItem prev, MapItem next)
    {
        var key = prev.symbol + "." + prev.row + "."+prev.column+"-"+next.symbol + "." + next.row + "." + next.column;
        if (!_updates.ContainsKey(key))
        {
            _updates.Add(key, new MapItemDelta
            {
                prev = prev,
                next = next
            });
        }
    }
}
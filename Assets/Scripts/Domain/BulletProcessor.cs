using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEditor;
using UnityEngine;

public class BulletProcessor : ItemProcessor<Bullet>
{

    private static List<string> _keys = new List<string>(new string[]
    {
        MapItems.KEY_BULLET,
    });

    private string _symbols;

    private static int getLocalDirection(char symbol)
    {
        switch (MapItems.MAP_KEYS[symbol])
        {
            case MapItems.KEY_BULLET:
                return MapUtils.DIRECTION_UP;
        }

        // TODO: FIX IT: remove default value
        return MapUtils.DIRECTION_UP;
        //throw new System.Exception("No direction for '" + symbol + "'");
    }

    public BulletProcessor(EcsWorld world, EcsFilter<Bullet> filter) : base(world, filter)
    {
    }

    public override void onFieldUpdates(char[][] prev, char[][] next, int row, int column)
    {
        if (!canProcess(prev[row][column]) && !canProcess(next[row][column]))
        {
            return;
        }


        Debug.Log("Bullet updated: '" + MapItems.MAP_KEYS[prev[row][column]] + "' => '" + MapItems.MAP_KEYS[next[row][column]] + "'");
        // Bullet was updated 
        if (canProcess(prev[row][column]))
        {
            Debug.Log("Expect update bullet at (" + row + ", " + column + ")");
            int nextRow;
            int nextColumn;
            if (!getCoordinatesWithDelta(prev, row, column, 2, out nextRow, out nextColumn))
            {
                Debug.Log("Next expected position out of battlefield");
                removeItem(row, column);
                return;
            }
            var expectedNext = next[nextRow][nextColumn];
            Debug.Log("Update bullet: expected next is '" + expectedNext + "'");
            if (expectedNext == prev[row][column])
            {
                var bullet = findByPosition(row, column);
                if (bullet != null)
                {
                    bullet.expectedPosition = MapUtils.getWorldPosition(_fieldSize, nextRow, nextColumn);
                    bullet.column = nextColumn;
                    bullet.row = nextRow;
                    next[nextRow][nextColumn] = MapItems.WITHOUT_CHANGES;
                    return;
                }
            }
        }

        // Bullet was destroyed
        if (canProcess(prev[row][column]))
        {
            int nextRow;
            int nextColumn;
            if (!getCoordinatesWithDelta(prev, row, column, 2, out nextRow, out nextColumn))
            {
                Debug.Log("Next expected position out of battlefield");
                return;
            }
            if (next[nextRow][nextColumn] != prev[row][column])
            {
                Debug.Log("Remove bullet at (" + row + ", " + column + ")");
                removeItem(row, column);
            }
        }

        // Bullet was created
        if (canProcess(next[row][column]))
        {
            int nextRow;
            int nextColumn;
            if (!getCoordinatesWithDelta(prev, row, column, -2, out nextRow, out nextColumn))
            {
                Debug.Log("Add bullet at (" + row + ", " + column + ")");
                createItem(next[row][column], row, column);
            }
        }
    }

    public override bool canProcess(char symbol)
    {
        return _symbols.IndexOf(symbol) >= 0;
    }

    protected override Quaternion getDirection(char symbol)
    {
        return MapUtils.getWorlRotation(getLocalDirection(symbol));
    }

    protected override Bullet createItem(char symbol, int row, int column)
    {
        var bullet = base.createItem(symbol, row, column);
        bullet.direction = getLocalDirection(symbol);
        var positionDelta = MapUtils.calculatePositionDelta(bullet.direction, 2);
        bullet.expectedPosition = MapUtils.mapToWorld(row + positionDelta.rowDelta, column + positionDelta.columnDelta);
        return bullet;
    }

    public override void setMapKeys(Dictionary<char, string> mapKeys)
    {
        foreach (var key in mapKeys.Keys)
        {
            if (_keys.IndexOf(mapKeys[key]) >= 0)
            {
                _symbols += key;
            }
        }
        Debug.Log("Bullet symbols are '" + _symbols + "'");
    }

    protected override string getPrefabName()
    {
        return "Shell";
    }

    protected override void onItenUpdated(char prev, char next, int row, int column)
    {
        throw new System.NotImplementedException();
    }

    private bool getCoordinatesWithDelta(char[][] field, int row, int column, int step, out int nextRow, out int nextColumn)
    {
        var posDelta = MapUtils.calculatePositionDelta(getLocalDirection(field[row][column]), step);
        nextRow = row + posDelta.rowDelta;
        nextColumn = column + posDelta.columnDelta;
        return (nextRow >= 0 && nextRow < _fieldSize && nextColumn >= 0 && nextColumn < _fieldSize);
    }
}

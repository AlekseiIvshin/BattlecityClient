﻿using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEditor;
using UnityEngine;

public class BulletProcessor : FieldProcessor<Bullet>
{

    private static List<string> _keys = new List<string>(new string[]
    {
        FieldItems.KEY_BULLET,
    });

    private string _symbols;

    public static int getDirection(char symbol)
    {
        switch (FieldItems.MAP_KEYS[symbol])
        {
            case FieldItems.KEY_BULLET:
                return MapUtils.DIRECTION_UP;
        }
        throw new System.Exception("No direction for '" + symbol + "'");
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

        // Bullet was updated 
        if (canProcess(prev[row][column]))
        {
            var posDelta = calculatePositionDelta(getDirection(prev[row][column]), 2);
            var nextRow = row + posDelta.rowDelta;
            var nextColumn = column + posDelta.columnDelta;
            var expectedNext = next[nextRow][nextColumn];
            if (expectedNext == prev[row][column])
            {
                var bullet = findByPosition(row, column);
                if (bullet != null)
                {
                    bullet.expectedPosition = MapUtils.getWorldPosition(_fieldSize, nextRow, nextColumn);
                    bullet.column = nextColumn;
                    bullet.row = nextRow;
                    next[nextRow][nextColumn] = FieldItems.WITHOUT_CHANGES;
                    return;
                }
            }
        }

        // Bullet was destroyed
        if (canProcess(prev[row][column]))
        {
            var posDelta = calculatePositionDelta(getDirection(prev[row][column]), 2);
            if (next[row + posDelta.rowDelta][column + posDelta.columnDelta] != prev[row][column])
            {
                removeItem(row, column);
            }
        }

        // Bullet was created
        if (canProcess(next[row][column]))
        {
            var posDelta = calculatePositionDelta(getDirection(prev[row][column]), -2);
            if (next[row][column] != prev[row + posDelta.rowDelta][column + posDelta.columnDelta])
            {
                createItem(next[row][column], row, column);
            }
        }
    }

    public override bool canProcess(char symbol)
    {
        return _symbols.IndexOf(symbol) >= 0;
    }

    protected override Bullet createItem(char symbol, int row, int column)
    {
        int entityId;
        var direction = getDirection(symbol);
        var unityObject = Object.Instantiate(
            AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Shell.prefab", typeof(GameObject)),
            MapUtils.getWorldPosition(_fieldSize, row, column),
            MapUtils.getWorlRotation(direction)) as GameObject;
        var bullet = createOrGetComponent(unityObject, out entityId);
        bullet.direction =direction;
        var positionDelta = calculatePositionDelta(bullet.direction, 2);
        bullet.entityId = entityId;
        bullet.column = column;
        bullet.row = row;
        bullet.transform = unityObject.transform;
        bullet.expectedPosition = MapUtils.getWorldPosition(_fieldSize, row + positionDelta.rowDelta, column + positionDelta.columnDelta);
        return bullet;
    }

    protected override void removeItem(int row, int column)
    {
        var bullet = findByPosition(row, column);
        if (bullet != null)
        {
            Object.Destroy(bullet.transform.gameObject);
            _world.RemoveEntity(bullet.entityId);
        }
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
    }
}

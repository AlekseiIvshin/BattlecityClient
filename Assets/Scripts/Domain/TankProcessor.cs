using Leopotam.Ecs;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TankProcessor : FieldProcessor<Tank>
{
    private static string tankSymbols = FieldItems.SYMBOLS.Substring(FieldItems.TankUp, FieldItems.AiTankLeft + 1 - FieldItems.TankUp);
    private static char DEAD_TANK = FieldItems.SYMBOLS_ARRAY[FieldItems.Bang];

    public TankProcessor(EcsWorld world, EcsFilter<Tank> filter) : base(world, filter)
    {
    }

    public static bool isTank(char symbol)
    {
        return tankSymbols.IndexOf(symbol) >= 0;
    }

    public static bool isDesctroyedTank(char symbol)
    {
        return symbol == DEAD_TANK;
    }

    public static int getDirection(char symbol)
    {
        if (symbol == FieldItems.SYMBOLS[FieldItems.TankUp] || symbol == FieldItems.SYMBOLS[FieldItems.OtherTankUp])
        {
            return MapUtils.DIRECTION_UP;
        }
        if (symbol == FieldItems.SYMBOLS[FieldItems.TankDown] || symbol == FieldItems.SYMBOLS[FieldItems.OtherTankDown])
        {
            return MapUtils.DIRECTION_DOWN;
        }
        if (symbol == FieldItems.SYMBOLS[FieldItems.TankLeft] || symbol == FieldItems.SYMBOLS[FieldItems.OtherTankLeft])
        {
            return MapUtils.DIRECTION_LEFT;
        }
        if (symbol == FieldItems.SYMBOLS[FieldItems.TankRight] || symbol == FieldItems.SYMBOLS[FieldItems.OtherTankRight])
        {
            return MapUtils.DIRECTION_RIGHT;
        }
        throw new System.Exception("No direction for '" + symbol + "'");
    }

    private static int getDirectionForMovement(int rowDelta, int columnDelta, int defaultValue)
    {
        Debug.Log("getDirectionForMovement(" + rowDelta + ", " + columnDelta + ")");
        if (rowDelta < 0)
        {
            return MapUtils.DIRECTION_DOWN;
        }
        else if (rowDelta > 0)
        {
            return MapUtils.DIRECTION_UP;
        }
        else if (columnDelta < 0)
        {
            return MapUtils.DIRECTION_LEFT;
        }
        else if (columnDelta > 0)
        {
            return MapUtils.DIRECTION_RIGHT;
        }
        Debug.Log("Default value");
        return defaultValue;
    }

    public override bool canProcess(char symbol)
    {
        return isTank(symbol);
    }

    public override void onFieldUpdates(char[][] prev, char[][] next, int row, int column)
    {
        if (!isTank(prev[row][column]))
        {
            return;
        }
        var tank = findByPosition(row, column);
        if (tank == null || tank.deltas.Count > 0)
        {
            return;
        }
        var nextState = next[row][column];
        var moveByRow = 0;
        var moveByColumn = 0;
        var rotateBy = 0;
        if (!isTank(nextState))
        {
            if (isTank(next[row - 1][column]))
            {
                moveByRow -= 1;
                nextState = next[row - 1][column];
            }
            else if (isTank(next[row + 1][column]))
            {
                moveByRow += 1;
                nextState = next[row + 1][column];
            }
            else if (isTank(next[row][column - 1]))
            {
                moveByColumn -= 1;
                nextState = next[row][column - 1];
            }
            else if (isTank(next[row][column + 1]))
            {
                moveByColumn += 1;
                nextState = next[row][column + 1];
            }
        }
        if (!isDesctroyedTank(nextState))
        {
            // TODO: tank was destroyed
        }
        if (moveByColumn != 0 || moveByRow != 0)
        {
            var movementDirection = getDirectionForMovement(moveByRow, moveByColumn, rotateBy);
            tank.deltas.Add(TankDelta.rotateToDirection(movementDirection));
            tank.deltas.Add(TankDelta.moveTo(tank.transform.position + MapUtils.getWorldDelta(moveByRow, moveByColumn)));
        }
        if (isTank(nextState))
        {
            tank.deltas.Add(TankDelta.rotateToDirection(getDirection(nextState)));
        }
    }

    protected override Tank createItem(char symbol, int row, int column)
    {
        int entityId;
        int direction = getDirection(symbol);
        GameObject unityObject = Object.Instantiate(
            AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Tank.prefab", typeof(GameObject)), 
            MapUtils.getWorldPosition(_fieldSize, row, column), 
            MapUtils.getWorlRotation(direction)) as GameObject;
        Tank tank = createOrGetComponent(unityObject, out entityId);
        tank.entityId = entityId;
        tank.direction = direction;
        tank.column = column;
        tank.row = row;
        tank.transform = unityObject.transform;
        tank.characterController = unityObject.GetComponent<CharacterController>();
        return tank;
    }

    protected override void removeItem(int row, int column)
    {
        throw new System.NotImplementedException();
    }

    public void initTanks(Dictionary<string, TankData> tanks)
    {
        foreach(var tankName in tanks.Keys)
        {
            createItem(tanks[tankName].symbol, tanks[tankName].row, tanks[tankName].column);
        }
    }

    public void onUpdate(Dictionary<string, TankData> tanks)
    {
        Tank tank;
        int moveByRow;
        int moveByColumn;
        int rotateBy;
        int movementDirection;
        foreach (var name in tanks.Keys)
        {
            tank = findByName(name);

            if (tank ==null)
            {
                createItem(tanks[name].symbol, tanks[name].row, tanks[name].column);
            }

            moveByRow = tank.row-tanks[name].row;
            moveByColumn = tank.column - tanks[name].column;
            rotateBy = 0;
            if (moveByColumn != 0 || moveByRow != 0)
            {
                movementDirection = getDirectionForMovement(moveByRow, moveByColumn, rotateBy);
                tank.deltas.Add(TankDelta.rotateToDirection(movementDirection));
                tank.deltas.Add(TankDelta.moveTo(tank.transform.position + MapUtils.getWorldDelta(moveByRow, moveByColumn)));
            }
            tank.deltas.Add(TankDelta.rotateToDirection(getDirection(tanks[name].symbol)));
        }
    }

    public Tank findByName(string name)
    {
        Tank tank = null;
        for (var entityId = 0; entityId < _filter.EntitiesCount; entityId++)
        {
            tank = _filter.Components1[entityId];
            if (tank.name == name)
            {
                return tank;
            }
        }
        return null;
    }
}

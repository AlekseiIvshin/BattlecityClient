using Leopotam.Ecs;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TankProcessor : FieldProcessor<Tank>
{
    private static string tankSymbols =""+ FieldItems.TANK_UP +
            FieldItems.OTHER_TANK_UP +
            FieldItems.TANK_DOWN +
            FieldItems.OTHER_TANK_DOWN +
            FieldItems.TANK_LEFT +
            FieldItems.OTHER_TANK_LEFT +
            FieldItems.TANK_RIGHT +
            FieldItems.OTHER_TANK_RIGHT;

    const char DEAD_TANK = 'Ѡ';

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
        switch (symbol)
        {
            case FieldItems.TANK_UP:
            case FieldItems.OTHER_TANK_UP:
                return MapUtils.DIRECTION_UP;
            case FieldItems.TANK_DOWN:
            case FieldItems.OTHER_TANK_DOWN:
                return MapUtils.DIRECTION_DOWN;
            case FieldItems.TANK_LEFT:
            case FieldItems.OTHER_TANK_LEFT:
                return MapUtils.DIRECTION_LEFT;
            case FieldItems.TANK_RIGHT:
            case FieldItems.OTHER_TANK_RIGHT:
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
}

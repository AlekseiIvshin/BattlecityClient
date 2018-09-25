using Leopotam.Ecs;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TankProcessor : ItemProcessor<Tank>
{
    private static List<string> _keys = new List<string>(new string[]
    {
        FieldItems.KEY_TANK_UP,
        FieldItems.KEY_OTHER_TANK_UP,
        FieldItems.KEY_TANK_LEFT,
        FieldItems.KEY_OTHER_TANK_LEFT,
        FieldItems.KEY_TANK_RIGHT,
        FieldItems.KEY_OTHER_TANK_RIGHT,
        FieldItems.KEY_TANK_DOWN,
        FieldItems.KEY_OTHER_TANK_DOWN,
    });
    private static List<string> _deadKeys = new List<string>(new string[]
    {
        FieldItems.KEY_BANG,
    });

    private string _tankSymbols;
    private string _deadTankSymbols;

    public TankProcessor(EcsWorld world, EcsFilter<Tank> filter) : base(world, filter)
    {
    }

    public static int getLocalDirection(char symbol)
    {
        switch (FieldItems.MAP_KEYS[symbol])
        {
            case FieldItems.KEY_TANK_UP:
            case FieldItems.KEY_OTHER_TANK_UP:
                return MapUtils.DIRECTION_UP;
            case FieldItems.KEY_TANK_LEFT:
            case FieldItems.KEY_OTHER_TANK_LEFT:
                return MapUtils.DIRECTION_LEFT;
            case FieldItems.KEY_TANK_RIGHT:
            case FieldItems.KEY_OTHER_TANK_RIGHT:
                return MapUtils.DIRECTION_RIGHT;
            case FieldItems.KEY_TANK_DOWN:
            case FieldItems.KEY_OTHER_TANK_DOWN:
                return MapUtils.DIRECTION_DOWN;
        }

        // TODO: FIX IT: remove default value
        return MapUtils.DIRECTION_UP;
        //throw new System.Exception("No direction for '" + symbol + "'");
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
        // Do nothing
    }

    public bool isTank(char symbol)
    {
        return _tankSymbols.IndexOf(symbol) >= 0;
    }

    public bool isDesctroyedTank(char symbol)
    {
        return _deadTankSymbols.IndexOf(symbol) >= 0;
    }

    public override void setMapKeys(Dictionary<char, string> mapKeys)
    {
        foreach (var key in mapKeys.Keys)
        {
            if (_keys.IndexOf(mapKeys[key]) >= 0)
            {
                _tankSymbols += key;
            }
            if (_deadKeys.IndexOf( mapKeys[key])>=0)
            {
                _deadTankSymbols += key;
            }
        }
    }

    protected override Quaternion getDirection(char symbol)
    {
        return MapUtils.getWorlRotation(getLocalDirection(symbol));
    }

    protected override Tank createItem(char symbol, int row, int column)
    {
        var tank = base.createItem(symbol, row, column);
        tank.direction = getLocalDirection(symbol);
        tank.characterController = tank.transform.gameObject.GetComponent<CharacterController>();
        return tank;
    }

    public void initTanks(Dictionary<string, TankData> tanks)
    {
        foreach (var tankName in tanks.Keys)
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

            if (tank == null)
            {
                createItem(tanks[name].symbol, tanks[name].row, tanks[name].column);
                return;
            }

            moveByRow = tank.row - tanks[name].row;
            moveByColumn = tank.column - tanks[name].column;
            rotateBy = 0;
            if (moveByColumn != 0 || moveByRow != 0)
            {
                movementDirection = getDirectionForMovement(moveByRow, moveByColumn, rotateBy);
                tank.deltas.Add(TankDelta.rotateToDirection(movementDirection));
                tank.deltas.Add(TankDelta.moveTo(tank.transform.position + MapUtils.getWorldDelta(moveByRow, moveByColumn)));
            }
            tank.deltas.Add(TankDelta.rotateToDirection(getLocalDirection(tanks[name].symbol)));
        }
        var tankNames = new List<string>(tanks.Keys);
        for (var entityId = 0; entityId < _filter.EntitiesCount; entityId++)
        {
            tank = _filter.Components1[entityId];
            if (tankNames.IndexOf(tank.name)<0)
            {
                removeItem(tank);
            }
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

    protected override string getPrefabPath()
    {
        return "Assets/Prefabs/Tank.prefab";
    }

    protected override void onItenUpdated(char prev, char next, int row, int column)
    {
        throw new System.NotImplementedException();
    }
}

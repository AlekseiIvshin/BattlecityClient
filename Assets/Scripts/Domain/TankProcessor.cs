using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankProcessor : FieldProcessor<Tank>
{
    const char TANK_UP = '▲';
    const char TANK_RIGHT = '►';
    const char TANK_DOWN = '▼';
    const char TANK_LEFT = '◄';

    const string tankSymbols = "▲►▼◄";

    const char OTHER_TANK_UP = '˄';
    const char OTHER_TANK_RIGHT = '˃';
    const char OTHER_TANK_DOWN = '˅';
    const char OTHER_TANK_LEFT = '˂';

    const string otherRankSymbols = "˄˃˅˂";

    public static bool isTank(char symbol)
    {
        return tankSymbols.IndexOf(symbol) >= 0 || otherRankSymbols.IndexOf(symbol) >= 0;
    }

    public static int getDirection(char symbol)
    {
        switch (symbol)
        {
            case TANK_UP:
            case OTHER_TANK_UP:
                return MapUtils.DIRECTION_UP;
            case TANK_DOWN:
            case OTHER_TANK_DOWN:
                return MapUtils.DIRECTION_DOWN;
            case TANK_LEFT:
            case OTHER_TANK_LEFT:
                return MapUtils.DIRECTION_LEFT;
            case TANK_RIGHT:
            case OTHER_TANK_RIGHT:
                return MapUtils.DIRECTION_RIGHT;
        }
        throw new System.Exception("No direction for '" + symbol + "'");
    }

    protected override bool canProcess(char symbol)
    {
        return isTank(symbol);
    }

    protected override Tank getFieldValue(char[][] field, int row, int column)
    {
        return new Tank
        {
            direction = getDirection(field[row][column]),
            row = row,
            column = column
        };
    }

    
}

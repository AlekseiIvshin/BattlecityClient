using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUtils {

    public const float tileSize = 3f;

    public const int DIRECTION_UNKNOWN = -1;
    public const int DIRECTION_UP = 0;
    public const int DIRECTION_RIGHT = 1;
    public const int DIRECTION_DOWN = 2;
    public const int DIRECTION_LEFT = 3;

    // Deprecated
    public static Vector3 getWorldPosition(int size, int row, int column)
    {
        return new Vector3(row * tileSize + tileSize/2,0, column * tileSize + tileSize / 2);
    }

    public static Vector3 mapToWorld(int row, int column)
    {
        return new Vector3(row * tileSize + tileSize / 2, 0, column * tileSize + tileSize / 2);
    }

    public static Vector3 getWorldDelta(int row, int column)
    {
        return new Vector3(row * tileSize, 0, column * tileSize);
    }

    public static Quaternion getWorlRotation(int direction)
    {
        return Quaternion.Euler(0, getAngle(direction), 0);
    }

    public static int getAngle(int direction)
    {
        switch (direction)
        {
            case DIRECTION_UNKNOWN: // TODO: remove after
            case DIRECTION_UP: return -90;
            case DIRECTION_RIGHT: return 0; 
            case DIRECTION_DOWN: return 90; 
            case DIRECTION_LEFT: return 180; 
        }
        throw new System.Exception("No such direciton for code " + DIRECTION_UNKNOWN);
    }

    public static int getAngleDelta(int fromDirection, int toDirection)
    {
        return getAngle(toDirection) - getAngle(fromDirection);
    }

    public static PositionDelta calculatePositionDelta(int direction, int step)
    {
        switch (direction)
        {
            case DIRECTION_UP: return new PositionDelta { rowDelta = -step, columnDelta = 0 };
            case DIRECTION_DOWN: return new PositionDelta { rowDelta = step, columnDelta = 0 };
            case DIRECTION_LEFT: return new PositionDelta { rowDelta = 0, columnDelta = -step };
            case DIRECTION_RIGHT: return new PositionDelta { rowDelta = 0, columnDelta = step };
            case DIRECTION_UNKNOWN:
            default: return new PositionDelta { rowDelta = 0, columnDelta = 0 };
        }
    }

    public static char[][] to2Dimension(string fieldSource)
    {
        return to2Dimension(fieldSource.ToCharArray());
    }

    public static char[][] to2Dimension(char[] fieldSource)
    {
        var size = (int)Mathf.Sqrt(fieldSource.Length);
        char[][] field = new char[size][];
        for (var i = 0; i < size; i++)
        {
            field[i] = new char[size];
            for (var j = 0; j < size; j++)
            {
                field[i][j] = fieldSource[i * size + j];
            }
        }
        return field;
    }
}

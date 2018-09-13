using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUtils {

    const float tileSize = 3f;

    public const int DIRECTION_UNKNOWN = -1;
    public const int DIRECTION_UP = 0;
    public const int DIRECTION_RIGHT = 1;
    public const int DIRECTION_DOWN = 2;
    public const int DIRECTION_LEFT = 3;

    public static Vector3 getWorldPosition(int size, int row, int column)
    {
        return new Vector3(row * tileSize + tileSize/2,0, column * tileSize + tileSize / 2);
    }

    public static Quaternion getWorlRotation(int direction)
    {
        switch(direction)
        {
            case DIRECTION_UP: return Quaternion.Euler(0, -90, 0);
            case DIRECTION_RIGHT: return Quaternion.Euler(0, 00, 0);
            case DIRECTION_DOWN: return Quaternion.Euler(0, 90, 0);
            case DIRECTION_LEFT: return Quaternion.Euler(0, 180, 0);
        }
        throw new System.Exception("No such direciton for code " + DIRECTION_UNKNOWN);
    }
}

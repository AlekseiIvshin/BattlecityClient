using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall
{

    public const int NOT_DESTROYED = 0;
    public const int DESTROYED_UP = 1;
    public const int DESTROYED_CENTER = 1 << 1;
    public const int DESTROYED_DOWN = 1 << 2;
    public const int DESTROYED_LEFT = 1 << 3;
    public const int DESTROYED_RIGHT = 1 << 4;

    public int entityId;
    public int hp;
    public int destroyed = NOT_DESTROYED;
    public int row;
    public int column;

    public Vector3 position;
}

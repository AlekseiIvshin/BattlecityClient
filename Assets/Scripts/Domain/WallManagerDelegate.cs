using UnityEngine;
using UnityEditor;
using Leopotam.Ecs;

public class WallManagerDelegate: ItemManagerDelegate<Wall>
{
    public const int NOT_DESTROYED = 0;
    public const int DESTROYED_UP = 1;
    public const int DESTROYED_CENTER_VERT = 1 << 1;
    public const int DESTROYED_CENTER_HOR = 1 << 2;
    public const int DESTROYED_DOWN = 1 << 3;
    public const int DESTROYED_LEFT = 1 << 4;
    public const int DESTROYED_RIGHT = 1 << 5;

    public WallManagerDelegate(EcsWorld world, EcsFilter<Wall> filter, string prefabName) : base(world, filter, prefabName)
    {
    }

    public override Wall createItem(MapItem item)
    {
        var wall = base.createItem(item);
        wall.destroyed = getDamages(item.symbol);
        var damages = getDamages(wall.destroyed);
        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                if (damages[i][j])
                {
                    applyDamageAt(wall, i, j);
                }
            }
        }
        return wall;
    }

    public override bool updateItem(MapItem prev, MapItem next)
    {
        if (base.updateItem(prev,next))
        {
            return true;
        }
        var wall = findByPosition(prev.row, prev.column);
        if (wall == null)
        {
            return false;
        }
        var nextState = new Wall
        {
            destroyed = getDamages(next.symbol),
            row = prev.row,
            column = prev.column
        };
        var prevDamages = getDamages(wall.destroyed);
        var nextDamages = getDamages(nextState.destroyed);
        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                if (!prevDamages[i][j] && nextDamages[i][j])
                {
                    applyDamageAt(wall, i, j);
                }
            }
        }
        return true;
    }

    private void applyDamageAt(Wall wall, int row, int col)
    {
        var wallPart = wall.transform.Find(row + "." + col);
        if (wallPart != null)
        {
            wallPart.transform.position = new Vector3(wallPart.transform.position.x, -2, wallPart.transform.position.z);
        }
    }

    private static bool[][] getDamages(int destroyed)
    {
        var destroyedLeft = (destroyed & DESTROYED_LEFT) > 0;
        var destroyedRight = (destroyed & DESTROYED_RIGHT) > 0;
        var destroyedDown = (destroyed & DESTROYED_DOWN) > 0;
        var destroyedUp = (destroyed & DESTROYED_UP) > 0;
        var destroyedCenterVert = (destroyed & DESTROYED_CENTER_VERT) > 0;
        var destroyedCenterHor = (destroyed & DESTROYED_CENTER_HOR) > 0;
        return new[] {
            new[] { destroyedLeft || destroyedUp, destroyedUp || destroyedCenterVert, destroyedUp || destroyedRight },
            new[] { destroyedLeft || destroyedCenterHor, destroyedCenterHor || destroyedCenterVert, destroyedRight || destroyedCenterHor },
            new[] { destroyedLeft || destroyedDown, destroyedDown || destroyedCenterVert, destroyedDown || destroyedRight }
        };
    }

    private int getDamages(char symbol)
    {
        switch (MapItems.MAP_KEYS[symbol])
        {
            case MapItems.KEY_CONSTRUCTION: return 0;
            case MapItems.KEY_CONSTRUCTION_DESTROYED_DOWN: return DESTROYED_DOWN;
            case MapItems.KEY_CONSTRUCTION_DESTROYED_UP: return DESTROYED_UP;
            case MapItems.KEY_CONSTRUCTION_DESTROYED_LEFT: return DESTROYED_LEFT;
            case MapItems.KEY_CONSTRUCTION_DESTROYED_RIGHT: return DESTROYED_RIGHT;
            case MapItems.KEY_CONSTRUCTION_DESTROYED_DOWN_TWICE: return DESTROYED_DOWN | DESTROYED_CENTER_HOR;
            case MapItems.KEY_CONSTRUCTION_DESTROYED_UP_TWICE: return DESTROYED_UP | DESTROYED_CENTER_HOR;
            case MapItems.KEY_CONSTRUCTION_DESTROYED_LEFT_TWICE: return DESTROYED_LEFT | DESTROYED_CENTER_VERT;
            case MapItems.KEY_CONSTRUCTION_DESTROYED_RIGHT_TWICE: return DESTROYED_RIGHT | DESTROYED_CENTER_VERT;
            case MapItems.KEY_CONSTRUCTION_DESTROYED_LEFT_RIGHT: return DESTROYED_RIGHT | DESTROYED_LEFT;
            case MapItems.KEY_CONSTRUCTION_DESTROYED_UP_DOWN: return DESTROYED_UP | DESTROYED_DOWN;
            case MapItems.KEY_CONSTRUCTION_DESTROYED_UP_LEFT: return DESTROYED_UP | DESTROYED_CENTER_VERT | DESTROYED_CENTER_HOR | DESTROYED_LEFT;
            case MapItems.KEY_CONSTRUCTION_DESTROYED_RIGHT_UP: return DESTROYED_UP | DESTROYED_CENTER_VERT | DESTROYED_CENTER_HOR | DESTROYED_RIGHT;
            case MapItems.CONSTRUCTION_DESTROYED_DOWN_LEFT: return DESTROYED_DOWN | DESTROYED_CENTER_VERT | DESTROYED_CENTER_HOR | DESTROYED_LEFT;
            case MapItems.KEY_CONSTRUCTION_DESTROYED_DOWN_RIGHT: return DESTROYED_DOWN | DESTROYED_CENTER_VERT | DESTROYED_CENTER_HOR | DESTROYED_RIGHT;
            case MapItems.KEY_CONSTRUCTION_DESTROYED: return DESTROYED_CENTER_HOR | DESTROYED_UP | DESTROYED_DOWN;
        }
        throw new System.Exception("No such map for " + symbol);
    }
}
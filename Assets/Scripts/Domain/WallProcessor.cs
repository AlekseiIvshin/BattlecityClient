using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Leopotam.Ecs;
using System.Text;

public class WallProcessor : FieldProcessor<Wall>
{
    public const int NOT_DESTROYED = 0;
    public const int DESTROYED_UP = 1;
    public const int DESTROYED_CENTER_VERT = 1 << 1;
    public const int DESTROYED_CENTER_HOR = 1 << 2;
    public const int DESTROYED_DOWN = 1 << 3;
    public const int DESTROYED_LEFT = 1 << 4;
    public const int DESTROYED_RIGHT = 1 << 5;

    private static string wallSymbols = FieldItems.SYMBOLS.Substring(FieldItems.Construction, FieldItems.ConstructionDestroyed + 1 - FieldItems.Construction);

    public WallProcessor(EcsWorld world, EcsFilter<Wall> filter) : base(world, filter)
    {
    }

    public static bool isWall(char symbol)
    {
        return wallSymbols.IndexOf(symbol) >= 0;
    }

    public override bool canProcess(char symbol)
    {
        return wallSymbols.IndexOf(symbol) >= 0;
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

    private static void printDamages(bool[][] damages, string tag)
    {
        Debug.Log("Print damages: " + tag);
        for (var i = 0; i < 3; i++)
        {
            var builder = new StringBuilder();
            for (var j = 0; j < 3; j++)
            {
                builder.Append((damages[i][j] ? 1 : 0) + " ");
            }
            Debug.Log(builder.ToString());
        }
    }

    public override void onFieldUpdates(char[][] prev, char[][] next, int row, int column)
    {
        if (!isWall(prev[row][column]))
        {
            return;
        }
        var wall = findByPosition(row, column);
        if (wall == null)
        {
            return;
        }
        var nextState = getFieldValue(next, row, column);
        var prevDamages = getDamages(wall.destroyed);
        var nextDamages = getDamages(nextState.destroyed);
        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                if (!prevDamages[i][j] && nextDamages[i][j])
                {
                    var wallPart = wall.transform.Find(i + "." + j);
                    if (wallPart != null)
                    {
                        wallPart.transform.position = new Vector3(wallPart.transform.position.x, -2, wallPart.transform.position.z);
                    }
                }
            }
        }
    }

    protected override Wall createItem(char symbol, int row, int column)
    {
        int entityId;
        var unityObject = Object.Instantiate(
            AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Wall.prefab", typeof(GameObject)),
            MapUtils.getWorldPosition(_fieldSize, row, column),
            Quaternion.Euler(0, 0, 0)
        ) as GameObject;
        Wall wall = createOrGetComponent(unityObject, out entityId); ;
        wall.entityId = entityId;
        wall.column = column;
        wall.row = row;
        wall.transform = unityObject.transform;
        return wall;
    }

    protected override void removeItem(int row, int column)
    {
        throw new System.NotImplementedException();
    }

    protected Wall getFieldValue(char[][] field, int row, int column)
    {
        int damaged = NOT_DESTROYED;

        if (FieldItems.SYMBOLS_ARRAY[FieldItems.Construction] == field[row][column])
        {
            damaged = 0;
        }
        if (FieldItems.SYMBOLS_ARRAY[FieldItems.ConstructionDestroyedDown] == field[row][column])
        {
            damaged = DESTROYED_DOWN;
        }
        if (FieldItems.SYMBOLS_ARRAY[FieldItems.ConstructionDestroyedUp] == field[row][column])
        {
            damaged = DESTROYED_UP;
        }
        if (FieldItems.SYMBOLS_ARRAY[FieldItems.ConstructionDestroyedLeft] == field[row][column])
        {
            damaged = DESTROYED_LEFT;
        }
        if (FieldItems.SYMBOLS_ARRAY[FieldItems.ConstructionDestroyedRight] == field[row][column])
        {
            damaged = DESTROYED_RIGHT;
        }
        if (FieldItems.SYMBOLS_ARRAY[FieldItems.ConstructionDestroyedDownTwice] == field[row][column])
        {
            damaged = DESTROYED_DOWN | DESTROYED_CENTER_HOR;
        }
        if (FieldItems.SYMBOLS_ARRAY[FieldItems.ConstructionDestroyedUpTwice] == field[row][column])
        {
            damaged = DESTROYED_UP | DESTROYED_CENTER_HOR;
        }
        if (FieldItems.SYMBOLS_ARRAY[FieldItems.ConstructionDestroyedLeftTwice] == field[row][column])
        {
            damaged = DESTROYED_LEFT | DESTROYED_CENTER_VERT;
        }
        if (FieldItems.SYMBOLS_ARRAY[FieldItems.ConstructionDestroyedRightTwice] == field[row][column])
        {
            damaged = DESTROYED_RIGHT | DESTROYED_CENTER_VERT;
        }
        if (FieldItems.SYMBOLS_ARRAY[FieldItems.ConstructionDestroyedLeftRight] == field[row][column])
        {
            damaged = DESTROYED_RIGHT | DESTROYED_LEFT;
        }
        if (FieldItems.SYMBOLS_ARRAY[FieldItems.ConstructionDestroyedUpDown] == field[row][column])
        {
            damaged = DESTROYED_UP | DESTROYED_DOWN;
        }
        if (FieldItems.SYMBOLS_ARRAY[FieldItems.ConstructionDestroyedUpLeft] == field[row][column])
        {
            damaged = DESTROYED_UP | DESTROYED_CENTER_VERT | DESTROYED_CENTER_HOR | DESTROYED_LEFT;
        }
        if (FieldItems.SYMBOLS_ARRAY[FieldItems.ConstructionDestroyedRightUp] == field[row][column])
        {
            damaged = DESTROYED_UP | DESTROYED_CENTER_VERT | DESTROYED_CENTER_HOR | DESTROYED_RIGHT;
        }
        if (FieldItems.SYMBOLS_ARRAY[FieldItems.ConstructionDestroyedLeft] == field[row][column])
        {
            damaged = DESTROYED_DOWN | DESTROYED_CENTER_VERT | DESTROYED_CENTER_HOR | DESTROYED_LEFT;
        }
        if (FieldItems.SYMBOLS_ARRAY[FieldItems.ConstructionDestroyedRight] == field[row][column])
        {
            damaged = DESTROYED_DOWN | DESTROYED_CENTER_VERT | DESTROYED_CENTER_HOR | DESTROYED_RIGHT;
        }
        if (FieldItems.SYMBOLS_ARRAY[FieldItems.ConstructionDestroyed] == field[row][column])
        {
            damaged = DESTROYED_CENTER_HOR | DESTROYED_UP | DESTROYED_DOWN;
        }

        return new Wall
        {
            destroyed = damaged,
            row = row,
            column = column
        };
    }
}

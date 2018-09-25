using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Leopotam.Ecs;
using System.Text;

public class WallProcessor : ImmobileItemProcessor<Wall>
{
    public const int NOT_DESTROYED = 0;
    public const int DESTROYED_UP = 1;
    public const int DESTROYED_CENTER_VERT = 1 << 1;
    public const int DESTROYED_CENTER_HOR = 1 << 2;
    public const int DESTROYED_DOWN = 1 << 3;
    public const int DESTROYED_LEFT = 1 << 4;
    public const int DESTROYED_RIGHT = 1 << 5;

    private static List<string> _keys = new List<string>(new string[]
    {
        FieldItems.KEY_CONSTRUCTION,
        FieldItems.KEY_CONSTRUCTION_DESTROYED_DOWN,
        FieldItems.KEY_CONSTRUCTION_DESTROYED_UP,
        FieldItems.KEY_CONSTRUCTION_DESTROYED_LEFT,
        FieldItems.KEY_CONSTRUCTION_DESTROYED_RIGHT,
        FieldItems.KEY_CONSTRUCTION_DESTROYED_DOWN_TWICE,
        FieldItems.KEY_CONSTRUCTION_DESTROYED_UP_TWICE,
        FieldItems.KEY_CONSTRUCTION_DESTROYED_LEFT_TWICE,
        FieldItems.KEY_CONSTRUCTION_DESTROYED_RIGHT_TWICE,
        FieldItems.KEY_CONSTRUCTION_DESTROYED_LEFT_RIGHT,
        FieldItems.KEY_CONSTRUCTION_DESTROYED_UP_DOWN,
        FieldItems.KEY_CONSTRUCTION_DESTROYED_UP_LEFT,
        FieldItems.KEY_CONSTRUCTION_DESTROYED_RIGHT_UP,
        FieldItems.CONSTRUCTION_DESTROYED_DOWN_LEFT,
        FieldItems.KEY_CONSTRUCTION_DESTROYED_DOWN_RIGHT,
        FieldItems.KEY_CONSTRUCTION_DESTROYED,
    });

    private string _symbols;

    public WallProcessor(EcsWorld world, EcsFilter<Wall> filter) : base(world, filter)
    {
    }

    public override bool canProcess(char symbol)
    {
        return _symbols.IndexOf(symbol) >= 0;
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

    private int getDamages(char symbol)
    {
        switch (FieldItems.MAP_KEYS[symbol])
        {
            case FieldItems.KEY_CONSTRUCTION: return 0;
            case FieldItems.KEY_CONSTRUCTION_DESTROYED_DOWN: return DESTROYED_DOWN;
            case FieldItems.KEY_CONSTRUCTION_DESTROYED_UP: return DESTROYED_UP;
            case FieldItems.KEY_CONSTRUCTION_DESTROYED_LEFT: return DESTROYED_LEFT;
            case FieldItems.KEY_CONSTRUCTION_DESTROYED_RIGHT: return DESTROYED_RIGHT;
            case FieldItems.KEY_CONSTRUCTION_DESTROYED_DOWN_TWICE: return DESTROYED_DOWN | DESTROYED_CENTER_HOR;
            case FieldItems.KEY_CONSTRUCTION_DESTROYED_UP_TWICE: return DESTROYED_UP | DESTROYED_CENTER_HOR;
            case FieldItems.KEY_CONSTRUCTION_DESTROYED_LEFT_TWICE: return DESTROYED_LEFT | DESTROYED_CENTER_VERT;
            case FieldItems.KEY_CONSTRUCTION_DESTROYED_RIGHT_TWICE: return DESTROYED_RIGHT | DESTROYED_CENTER_VERT;
            case FieldItems.KEY_CONSTRUCTION_DESTROYED_LEFT_RIGHT: return DESTROYED_RIGHT | DESTROYED_LEFT;
            case FieldItems.KEY_CONSTRUCTION_DESTROYED_UP_DOWN: return DESTROYED_UP | DESTROYED_DOWN;
            case FieldItems.KEY_CONSTRUCTION_DESTROYED_UP_LEFT: return DESTROYED_UP | DESTROYED_CENTER_VERT | DESTROYED_CENTER_HOR | DESTROYED_LEFT;
            case FieldItems.KEY_CONSTRUCTION_DESTROYED_RIGHT_UP: return DESTROYED_UP | DESTROYED_CENTER_VERT | DESTROYED_CENTER_HOR | DESTROYED_RIGHT;
            case FieldItems.CONSTRUCTION_DESTROYED_DOWN_LEFT: return DESTROYED_DOWN | DESTROYED_CENTER_VERT | DESTROYED_CENTER_HOR | DESTROYED_LEFT;
            case FieldItems.KEY_CONSTRUCTION_DESTROYED_DOWN_RIGHT: return DESTROYED_DOWN | DESTROYED_CENTER_VERT | DESTROYED_CENTER_HOR | DESTROYED_RIGHT;
            case FieldItems.KEY_CONSTRUCTION_DESTROYED: return DESTROYED_CENTER_HOR | DESTROYED_UP | DESTROYED_DOWN;
        }
        throw new System.Exception("No such map for " + symbol);
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
        Debug.Log("Wall symbols are '" + _symbols + "'");
    }

    protected override string getPrefabPath()
    {
        return "Assets/Prefabs/Wall.prefab";
    }

    protected override void onItenUpdated(char prev, char next, int row, int column)
    {
        Debug.Log("Wall updated: '"+FieldItems.MAP_KEYS[prev]+"' => '"+ FieldItems.MAP_KEYS[next] +"'");
        var wall = findByPosition(row, column);
        if (wall == null)
        {
            return;
        }
        var nextState = new Wall
        {
            destroyed = getDamages(next),
            row = row,
            column = column
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
    }

    protected override Wall createItem(char symbol, int row, int column)
    {
        var wall = base.createItem(symbol, row, column);
        wall.destroyed = getDamages(symbol);
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

    private void applyDamageAt(Wall wall, int row, int col)
    {
        var wallPart = wall.transform.Find(row + "." + col);
        if (wallPart != null)
        {
            wallPart.transform.position = new Vector3(wallPart.transform.position.x, -2, wallPart.transform.position.z);
        }
    }
}

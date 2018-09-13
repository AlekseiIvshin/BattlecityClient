using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallProcessor : FieldProcessor<Wall>
{

    private const string wallSymbols = "╬╩╦╠╣╨╥╞╡│─┌┐└┘";
    const char CONSTRUCTION = '╬';
    const char CONSTRUCTION_DESTROYED_DOWN = '╩';
    const char CONSTRUCTION_DESTROYED_UP = '╦';
    const char CONSTRUCTION_DESTROYED_LEFT = '╠';
    const char CONSTRUCTION_DESTROYED_RIGHT = '╣';
    const char CONSTRUCTION_DESTROYED_DOWN_TWICE = '╨';
    const char CONSTRUCTION_DESTROYED_UP_TWICE = '╥';
    const char CONSTRUCTION_DESTROYED_LEFT_TWICE = '╞';
    const char CONSTRUCTION_DESTROYED_RIGHT_TWICE = '╡';
    const char CONSTRUCTION_DESTROYED_LEFT_RIGHT = '│';
    const char CONSTRUCTION_DESTROYED_UP_DOWN = '─';
    const char CONSTRUCTION_DESTROYED_UP_LEFT = '┌';
    const char CONSTRUCTION_DESTROYED_RIGHT_UP = '┐';
    const char CONSTRUCTION_DESTROYED_DOWN_LEFT = '└';
    const char CONSTRUCTION_DESTROYED_DOWN_RIGHT = '┘';

    public static bool isWall(char symbol)
    {
        return wallSymbols.IndexOf(symbol) >= 0;
    }

    protected override bool canProcess(char symbol)
    {
        return wallSymbols.IndexOf(symbol) >= 0;
    }

    protected override Wall getFieldValue(char[][] field, int row, int column)
    {
        var hp = 0;
        int damaged = Wall.NOT_DESTROYED;
        switch (field[row][column])
        {
            case CONSTRUCTION:
                {
                    hp = 3;
                    damaged = 0;
                    break;
                }
            case CONSTRUCTION_DESTROYED_DOWN:
                {
                    hp = 2;
                    damaged = Wall.DESTROYED_DOWN;
                    break;
                }
            case CONSTRUCTION_DESTROYED_UP:
                {
                    hp = 2;
                    damaged = Wall.DESTROYED_UP;
                    break;
                }
            case CONSTRUCTION_DESTROYED_LEFT:
                {
                    hp = 2;
                    damaged = Wall.DESTROYED_LEFT;
                    break;
                }
            case CONSTRUCTION_DESTROYED_RIGHT:
                {
                    hp = 2;
                    damaged = Wall.DESTROYED_RIGHT;
                    break;
                }
            case CONSTRUCTION_DESTROYED_DOWN_TWICE:
                {
                    hp = 1;
                    damaged = Wall.DESTROYED_DOWN | Wall.DESTROYED_CENTER;
                    break;
                }
            case CONSTRUCTION_DESTROYED_UP_TWICE:
                {
                    hp = 1;
                    damaged = Wall.DESTROYED_UP | Wall.DESTROYED_CENTER;
                    break;
                }
            case CONSTRUCTION_DESTROYED_LEFT_TWICE:
                {
                    hp = 1;
                    damaged = Wall.DESTROYED_LEFT | Wall.DESTROYED_CENTER;
                    break;
                }
            case CONSTRUCTION_DESTROYED_RIGHT_TWICE:
                {
                    hp = 1;
                    damaged = Wall.DESTROYED_RIGHT | Wall.DESTROYED_CENTER;
                    break;
                }
            case CONSTRUCTION_DESTROYED_LEFT_RIGHT:
                {
                    hp = 1;
                    damaged = Wall.DESTROYED_RIGHT | Wall.DESTROYED_LEFT;
                    break;
                }
            case CONSTRUCTION_DESTROYED_UP_DOWN:
                {
                    hp = 1;
                    damaged = Wall.DESTROYED_UP | Wall.DESTROYED_DOWN;
                    break;
                }
            case CONSTRUCTION_DESTROYED_UP_LEFT:
                {
                    hp = 1;
                    damaged = Wall.DESTROYED_UP | Wall.DESTROYED_CENTER | Wall.DESTROYED_LEFT;
                    break;
                }
            case CONSTRUCTION_DESTROYED_RIGHT_UP:
                {
                    hp = 1;
                    damaged = Wall.DESTROYED_UP | Wall.DESTROYED_CENTER | Wall.DESTROYED_RIGHT;
                    break;
                }
            case CONSTRUCTION_DESTROYED_DOWN_LEFT:
                {
                    hp = 1;
                    damaged = Wall.DESTROYED_DOWN | Wall.DESTROYED_CENTER | Wall.DESTROYED_LEFT;
                    break;
                }
            case CONSTRUCTION_DESTROYED_DOWN_RIGHT:
                {
                    hp = 1;
                    damaged = Wall.DESTROYED_DOWN | Wall.DESTROYED_CENTER | Wall.DESTROYED_RIGHT;
                    break;
                }
        }
        return new Wall
        {
            destroyed = damaged,
            hp = hp,
            row = row,
            column = column
        };
    }

    private static bool[][] getDamages(int destroyed)
    {
        var destroyedLeft = (destroyed & Wall.DESTROYED_LEFT) > 0;
        var destroyedRight = (destroyed & Wall.DESTROYED_RIGHT) > 0;
        var destroyedDown = (destroyed & Wall.DESTROYED_LEFT) > 0;
        var destroyedUp = (destroyed & Wall.DESTROYED_UP) > 0;
        var destroyedCenter = (destroyed & Wall.DESTROYED_DOWN) > 0;
        return new[] {
            new[] { destroyedLeft || destroyedUp, destroyedUp || destroyedCenter, destroyedUp || destroyedRight },
            new[] { destroyedLeft || destroyedCenter, destroyedCenter, destroyedRight || destroyedCenter },
            new[] { destroyedLeft || destroyedDown, destroyedDown || destroyedCenter, destroyedRight || destroyedRight }
        };
    }
}

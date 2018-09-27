using UnityEngine;
using UnityEditor;

public class MapItem
{
    public char symbol;
    public int row;
    public int column;

    public override string ToString()
    {
        return "('" + symbol + "' > " + row + ", " + column+")";
    }
}
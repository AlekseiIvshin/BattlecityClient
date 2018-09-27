using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BattlefieldState
{
    public char[][] field;
    public Dictionary<string, MapItem> tanks;
    public int size;
}
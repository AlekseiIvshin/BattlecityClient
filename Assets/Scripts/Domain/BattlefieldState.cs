using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class BattlefieldState
{
    public char[][] field;
    public Dictionary<string, TankData> tanks;
}
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Leopotam.Ecs;
using System.Text;

public class HedgehogProcessor : ImmobileItemProcessor<Wall>
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
        MapItems.KEY_HEDGEHOG,
    });

    private string _symbols;

    public HedgehogProcessor(EcsWorld world, EcsFilter<Wall> filter) : base(world, filter)
    {
    }

    public override bool canProcess(char symbol)
    {
        return _symbols.IndexOf(symbol) >= 0;
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
        Debug.Log("Hedgehog symbols are '" + _symbols + "'");
    }

    protected override string getPrefabPath()
    {
        return "Assets/Prefabs/Hedgehog.prefab";
    }

    protected override void onItenUpdated(char prev, char next, int row, int column)
    {
        // Do nothing
    }
}

using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEditor;
using UnityEngine;

public class UndestroyableWallProcessor : ImmobileItemProcessor<UndestroyableWall>
{
    private static List<string> _keys = new List<string>(new string[]
    {
        MapItems.KEY_BATTLE_WALL,
    });

    private string _symbols;

    public UndestroyableWallProcessor(EcsWorld world, EcsFilter<UndestroyableWall> filter) : base(world, filter)
    {
    }

    public override bool canProcess(char symbol)
    {
        return _symbols.IndexOf(symbol)>=0;
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
    }

    protected override string getPrefabPath()
    {
        return "Assets/Prefabs/UndestroyableWall.prefab";
    }

    protected override void onItenUpdated(char prev, char next, int row, int column)
    {
        // Do nothing
    }
}

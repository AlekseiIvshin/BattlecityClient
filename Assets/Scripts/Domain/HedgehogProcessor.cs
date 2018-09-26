using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Leopotam.Ecs;
using System.Text;

public class AmmoBoxProcessor : ImmobileItemProcessor<Wall>
{

    private static List<string> _keys = new List<string>(new string[]
    {
        MapItems.KEY_HEDGEHOG,
    });

    private string _symbols;

    public AmmoBoxProcessor(EcsWorld world, EcsFilter<Wall> filter) : base(world, filter)
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

    protected override string getPrefabName()
    {
        return "Hedgehog";
    }

    protected override void onItenUpdated(char prev, char next, int row, int column)
    {
        // Do nothing
    }
}

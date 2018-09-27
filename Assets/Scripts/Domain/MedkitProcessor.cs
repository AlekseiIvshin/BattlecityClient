using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Leopotam.Ecs;
using System.Text;

public class MedkitProcessor : ImmobileItemProcessor<Medkit>
{

    private static List<string> _keys = new List<string>(new string[]
    {
        MapItems.KEY_MEDKIT,
    });

    private string _symbols;

    public MedkitProcessor(EcsWorld world, EcsFilter<Medkit> filter) : base(world, filter)
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
        Debug.Log("Medkit symbols are '" + _symbols + "'");
    }

    protected override string getPrefabName()
    {
        return "Medkit";
    }

    protected override void onItenUpdated(char prev, char next, int row, int column)
    {
        // Do nothing
    }
}

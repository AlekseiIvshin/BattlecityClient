﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Leopotam.Ecs;
using System.Text;

public class HedgehogProcessor : ImmobileItemProcessor<Wall>
{
    private static List<string> _keys = new List<string>(new string[]
    {
        MapItems.KEY_BONUS_AMMO,
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
        Debug.Log("Ammo box symbols are '" + _symbols + "'");
    }

    protected override string getPrefabName()
    {
        return "AmmoBox.prefab";
    }

    protected override void onItenUpdated(char prev, char next, int row, int column)
    {
        // Do nothing
    }
}

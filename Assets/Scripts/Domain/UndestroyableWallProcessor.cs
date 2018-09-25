using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEditor;
using UnityEngine;

public class UndestroyableWallProcessor : FieldProcessor<UndestroyableWall>
{
    private static List<string> _keys = new List<string>(new string[]
    {
        FieldItems.KEY_BATTLE_WALL,
    });

    private string _symbols;

    public UndestroyableWallProcessor(EcsWorld world, EcsFilter<UndestroyableWall> filter) : base(world, filter)
    {
    }

    public override void onFieldUpdates(char[][] prev, char[][] next, int row, int column)
    {
        // Do nothing
    }

    public override bool canProcess(char symbol)
    {
        return _symbols.IndexOf(symbol)>=0;
    }

    protected override UndestroyableWall createItem(char symbol, int row, int column)
    {
        int entityId;
        GameObject unityObject = Object.Instantiate(AssetDatabase.LoadAssetAtPath("Assets/Prefabs/UndestroyableWall.prefab", typeof(GameObject)), MapUtils.getWorldPosition(_fieldSize, row, column), Quaternion.Euler(0, 0, 0)) as GameObject;
        UndestroyableWall wall = createOrGetComponent(unityObject, out entityId);
        wall.entityId = entityId;
        wall.column = column;
        wall.row = row;
        return wall;
    }

    protected override void removeItem(int row, int column)
    {
        throw new System.NotImplementedException();
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
}

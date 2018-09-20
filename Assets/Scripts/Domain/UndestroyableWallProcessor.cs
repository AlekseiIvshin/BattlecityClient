using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEditor;
using UnityEngine;

public class UndestroyableWallProcessor : FieldProcessor<UndestroyableWall>
{
    public UndestroyableWallProcessor(EcsWorld world, EcsFilter<UndestroyableWall> filter) : base(world, filter)
    {
    }

    public override void onFieldUpdates(char[][] prev, char[][] next, int row, int column)
    {
        // Do nothing
    }

    public override bool canProcess(char symbol)
    {
        return '☼' == symbol;
    }

    protected override UndestroyableWall createItem(char symbol, int row, int column)
    {
        int entityId;
        GameObject unityObject = Object.Instantiate(AssetDatabase.LoadAssetAtPath("Assets/Prefabs/UndestroyableWall.prefab", typeof(GameObject)), MapUtils.getWorldPosition(_fieldSize, row, column), Quaternion.Euler(0, 0, 0)) as GameObject;
        UndestroyableWall wall = createOrGetComponent(unityObject, out entityId);
        wall.column = column;
        wall.row = row;
        return wall;
    }

    protected override void removeItem(int row, int column)
    {
        throw new System.NotImplementedException();
    }
}

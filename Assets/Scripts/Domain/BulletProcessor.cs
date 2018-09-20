using System.Collections;
using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEditor;
using UnityEngine;

public class BulletProcessor : FieldProcessor<Bullet>
{
    const char BULLET = '•';

    public BulletProcessor(EcsWorld world, EcsFilter<Bullet> filter) : base(world, filter)
    {
    }

    public override void onFieldUpdates(char[][] prev, char[][] next, int row, int column)
    {
        if (canProcess(prev[row][column]) && !canProcess(next[row][column]))
        {
            // Bullet was disappeared
            removeItem(row, column);
        }
        else if (!canProcess(prev[row][column]) && canProcess(next[row][column]))
        {
            // Bullet was added
            createItem(next[row][column], row, column);
        }
    }

    public override bool canProcess(char symbol)
    {
        return symbol == BULLET;
    }

    protected override Bullet createItem(char symbol, int row, int column)
    {
        int entityId;
        var unityObject = Object.Instantiate(
            AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Shell.prefab", typeof(GameObject)),
            MapUtils.getWorldPosition(_fieldSize, row, column),
             Quaternion.Euler(0, 0, 0)) as GameObject;
        var bullet = createOrGetComponent(unityObject, out entityId);
        bullet.column = column;
        bullet.row = row;
        bullet.transform = unityObject.transform;
        return bullet;
    }

    protected override void removeItem(int row, int column)
    {
        var bullet = findByPosition(row, column);
        if (bullet != null)
        {
            Object.Destroy(bullet.transform.gameObject);
            _world.RemoveEntity(bullet.entityId);
        }
    }
}

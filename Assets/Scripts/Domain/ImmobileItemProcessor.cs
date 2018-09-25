using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.Ecs;
using UnityEditor;

public abstract class ImmobileItemProcessor<T> : ItemProcessor<T>, FieldHandler where T : BaseEntity, new()
{
    public ImmobileItemProcessor(EcsWorld world, EcsFilter<T> filter) : base(world, filter)
    {
    }

    public override void onFieldUpdates(char[][] prev, char[][] next, int row, int column)
    {
        bool canProcessPrev = canProcess(prev[row][column]);
        bool canProcessNext = canProcess(next[row][column]);
        if (!canProcessPrev && !canProcessNext)
        {
            return;
        }
        if (!canProcessPrev && canProcessNext)
        {
            Debug.Log("Add item at (" + row + ", " + column + ")");
            onItemAdded(next[row][column], row, column);
        }
        else if (canProcessPrev && !canProcessNext)
        {
            Debug.Log("Remove item at (" + row + ", " + column + ")");
            onItenRemoved(row, column);
        }
        else if (prev[row][column] != next[row][column])
        {
            Debug.Log("Update item at (" + row + ", " + column + ")");
            onItenUpdated(prev[row][column], next[row][column], row, column);
        }
    }
}

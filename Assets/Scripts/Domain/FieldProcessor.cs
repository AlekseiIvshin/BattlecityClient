using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.Ecs;

public abstract class FieldProcessor<T>: FieldHandler where T : BaseEntity, new()
{
    protected EcsWorld _world;
    protected EcsFilter<T> _filter;
    protected int _fieldSize;

    public FieldProcessor(EcsWorld world, EcsFilter<T> filter)
    {
        this._world = world;
        this._filter = filter;
    }

    public static PositionDelta calculatePositionDelta(int direction, int step)
    {
        switch(direction)
        {
            case MapUtils.DIRECTION_UP:return new PositionDelta { rowDelta = -step, columnDelta = 0 };
            case MapUtils.DIRECTION_DOWN: return new PositionDelta { rowDelta = step, columnDelta = 0 };
            case MapUtils.DIRECTION_LEFT: return new PositionDelta { rowDelta = 0, columnDelta = -step };
            case MapUtils.DIRECTION_RIGHT: return new PositionDelta { rowDelta = 0, columnDelta = step };
            case MapUtils.DIRECTION_UNKNOWN:
            default: return new PositionDelta { rowDelta = 0, columnDelta = 0 };
        }
    }

    public abstract void onFieldUpdates(char[][] prev, char[][] next, int row, int column);
    public abstract bool canProcess(char symbol);
    protected abstract T createItem(char symbol, int row, int column);
    protected abstract void removeItem(int row, int column);

    public T findByPosition(int row, int column)
    {
        T item = null;
        for (var entityId = 0; entityId < _filter.EntitiesCount; entityId++)
        {
            item = _filter.Components1[entityId];
            if (item.row == row && item.column == column)
            {
                return item;
            }
        }
        return null;
    }

    public void initItem(char symbol, int row, int column)
    {
        if (canProcess(symbol))
        {
            createItem(symbol, row, column);
        }
    }

    public void setFieldSize(int fieldSize)
    {
        this._fieldSize = fieldSize;
    }

    protected T createOrGetComponent(GameObject unityObject, out int entityId)
    {
        var entity=SystemUtils.getComponent<T>(_world, unityObject, out entityId);
        entity.entityId = entityId;
        return entity;
    }
}

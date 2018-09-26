using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leopotam.Ecs;
using UnityEditor;

public abstract class ItemProcessor<T> : FieldHandler where T : BaseEntity, new()
{
    protected EcsWorld _world;
    protected EcsFilter<T> _filter;
    protected int _fieldSize;

    public ItemProcessor(EcsWorld world, EcsFilter<T> filter)
    {
        this._world = world;
        this._filter = filter;
    }

    public abstract bool canProcess(char symbol);
    public abstract void setMapKeys(Dictionary<char, string> keys);

    protected abstract string getPrefabPath();

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

    public abstract void onFieldUpdates(char[][] prev, char[][] next, int row, int column);

    public void setFieldSize(int fieldSize)
    {
        this._fieldSize = fieldSize;
    }

    public void initItem(char symbol, int row, int column)
    {
        if (canProcess(symbol))
        {
            createItem(symbol, row, column);
        }
    }

    protected void onItemAdded(char symbol, int row, int column)
    {
        createItem(symbol, row, column);
    }
    protected void onItenRemoved(int row, int column)
    {
        removeItem(row, column);
    }
    protected abstract void onItenUpdated(char prev, char next, int row, int column);

    protected virtual void removeItem(T entity)
    {
        Object.Destroy(entity.transform.gameObject);
        entity.transform = null;
        _world.RemoveEntity(entity.entityId);
    }

    protected void removeItem(int row, int column)
    {
        var entity = findByPosition(row, column);
        if (entity != null)
        {
            Debug.Log("Remove item: id=" + entity.entityId+"; type="+ typeof(T));
            removeItem(entity);
        }
    }

    protected virtual Quaternion getDirection(char symbol)
    {
        return Quaternion.Euler(0, 0, 0);
    }

    protected virtual T createItem(char symbol, int row, int column)
    {
        int entityId;
        GameObject unityObject = Object.Instantiate(
            AssetDatabase.LoadAssetAtPath(getPrefabPath(), typeof(GameObject)),
            MapUtils.mapToWorld(row, column),
            getDirection(symbol)
        ) as GameObject;
        T entity = SystemUtils.getComponent<T>(_world, unityObject, out entityId);
        entity.entityId = entityId;
        entity.column = column;
        entity.row = row;
        entity.transform = unityObject.transform;
        return entity;
    }
}

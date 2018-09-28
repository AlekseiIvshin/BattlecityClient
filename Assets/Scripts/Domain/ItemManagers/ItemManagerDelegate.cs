using UnityEngine;
using UnityEditor;
using Leopotam.Ecs;

public class ItemManagerDelegate<T>: IItemManagerDelegate<T> where T : BaseEntity, new()
{
    private EcsWorld _world;
    protected EcsFilter<T> _filter;
    private string _prefabName;
    private string _symbols;

    public ItemManagerDelegate(EcsWorld world, EcsFilter<T> filter, string prefabName)
    {
        this._world = world;
        this._filter = filter;
        this._prefabName = prefabName;
    }

    public virtual T createItem(MapItem item)
    {
        int entityId;
        GameObject unityObject = Object.Instantiate(
            Resources.Load("Prefabs/" + this._prefabName) as GameObject,
            MapUtils.mapToWorld(item),
            getDirection(item)
        ) as GameObject;
        T entity = SystemUtils.getComponent<T>(_world, unityObject, out entityId);
        entity.entityId = entityId;
        entity.column = item.column;
        entity.row = item.row;
        entity.transform = unityObject.transform;
        return entity;
    }

    public virtual void destroyItem(T entity)
    {
        Object.Destroy(entity.transform.gameObject);
        entity.transform = null;
        _world.RemoveEntity(entity.entityId);
    }

    public void destroyItem(int row, int column)
    {
        var entity = findByPosition(row, column);
        if (entity != null)
        {
            destroyItem(entity);
        }
    }

    public virtual bool updateItem(MapItem prev, MapItem next)
    {
        if (!canProcess(prev.symbol) && canProcess(next.symbol))
        {
            createItem(next);
            return true;
        } else if (canProcess(prev.symbol) && !canProcess(next.symbol))
        {
            destroyItem(prev.row, prev.column);
            return true;
        }
        return false;
    }

    public void setSymbols(string symbols)
    {
        this._symbols = symbols;
    }

    public virtual Quaternion getDirection(MapItem item)
    {
        return Quaternion.Euler(0, 0, 0);
    }

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

    public bool canProcess(char symbol)
    {
        return _symbols.IndexOf(symbol) >= 0;
    }

    public string prefabName()
    {
        return _prefabName;
    }
}
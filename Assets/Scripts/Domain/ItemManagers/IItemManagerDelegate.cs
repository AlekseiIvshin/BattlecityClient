using UnityEngine;
using UnityEditor;
using Leopotam.Ecs;

public interface IItemManagerDelegate<T> where T : BaseEntity, new()
{
    T createItem(MapItem item);
    void destroyItem(T entity);

    void destroyItem(int row, int column);

    bool updateItem(MapItem prev, MapItem next);

    void setSymbols(string symbols);

    Quaternion getDirection(MapItem item);

    T findByPosition(int row, int column);

    bool canProcess(char symbol);

    string prefabName();
}
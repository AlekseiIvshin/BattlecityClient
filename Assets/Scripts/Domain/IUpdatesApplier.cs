using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public interface IUpdatesApplier: IUpdatesAccumulator
{
    string prefabName();
    void initItem(BattlefieldState state, int row, int column);
    void applyUpdates();
}
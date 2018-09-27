using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public interface IUpdatesAccumulator
{
    void setSymbols(string symbols);
    void accumulateUpdates(BattlefieldState prev, BattlefieldState next, int row, int column);
    MapItemDelta[] getUpdatesAndClear();
}
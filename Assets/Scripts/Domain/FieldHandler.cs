using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface FieldHandler {

    bool canProcess(char symbol);
    void initItem(char symbol, int row, int column);
    void onFieldUpdates(char[][] prev, char[][] next, int row, int column);
    void setFieldSize(int fieldSize);
    void setMapKeys(Dictionary<char, string> keys);
}

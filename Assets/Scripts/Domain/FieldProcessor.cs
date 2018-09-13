using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FieldProcessor<T>
{
    private List<T> accumulator;

    public FieldProcessor()
    {
        this.accumulator = new List<T>();
    }

    protected abstract bool canProcess(char symbol);
    protected abstract T getFieldValue(char[][] field, int row, int column);

    public void process(char[][] field, int row, int column)
    {
        if (canProcess(field[row][column]))
        {
            accumulator.Add(getFieldValue(field, row, column));
        }
    }

    public List<T> getItems()
    {
        return accumulator;
    }

}

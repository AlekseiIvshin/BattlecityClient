using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndestroyableWallProcessor : FieldProcessor<UndestroyableWall>
{
    protected override bool canProcess(char symbol)
    {
        return '☼' == symbol;
    }

    protected override UndestroyableWall getFieldValue(char[][] field, int row, int column)
    {
        return new UndestroyableWall
        {
            row = row,
            column = column
        };
    }
}

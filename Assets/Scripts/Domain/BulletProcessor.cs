using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProcessor : FieldProcessor<Bullet>
{
    const char BULLET = '•';

    protected override bool canProcess(char symbol)
    {
        return symbol == BULLET;
    }

    protected override Bullet getFieldValue(char[][] field, int row, int column)
    {
        return new Bullet
        {
            direction = MapUtils.DIRECTION_UNKNOWN,
            row = row,
            column = column,
        };
    }
}

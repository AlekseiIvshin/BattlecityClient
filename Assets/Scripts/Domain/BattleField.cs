using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleField
{

    public const string SAMPLE = "☼☼☼☼☼☼☼☼☼☼☼☼☼☼☼☼             ☼☼ ╬ ╬ ╬ ╬ ╬ ╬ ☼☼ ╬ ╬ ╬ ╬ ╬ ╬ ☼☼ ╬ ╬ ╬ ╬ ╬ ╬ ☼☼ ╬ ╬ ╬ ╬ ╬˅╬ ☼☼ ╬ ╬˂╬ ╬ ╬ ╬ ☼☼ ╬ ╬ ╬ ╬ ╬•╬ ☼☼ ╬ ╬ ╬ ╬ ╬ ╬ ☼☼ ╬ ╬ ╠ ╬ ╬ ╬ ☼☼ ╬ ╬ ╬ ╬ ╬ ╬ ☼☼ ╬ ╬ ╬ ╬ ╬ ╬ ☼☼ ╬▲╬ ╬ ╬ ╬ ╬ ☼☼             ☼☼☼☼☼☼☼☼☼☼☼☼☼☼☼☼";
    public const string SAMPLE_SMALL = "☼☼☼☼☼☼☼╬ ˅☼☼ ˂╬☼☼ ╬╬☼☼☼☼☼☼";
    /*01234                           
    0 ☼☼☼☼☼                           
    1 ☼╬ ˅☼                           
    2 ☼ ˂╬☼                           
    3 ☼ ╬╬☼                           
    4 ☼☼☼☼☼ */

    private char[] field;

    private List<FieldProcessor<object>> fieldProcessors = new List<FieldProcessor<object>>();

    public static char[][] to2Dimension(string fieldSource)
    {
        var size = (int)Mathf.Sqrt(fieldSource.Length);
        char[][] field = new char[size][];
        for (var i = 0; i < size; i++)
        {
            field[i] = new char[size];
            for (var j = 0; j < size; j++)
            {
                field[i][j] = fieldSource[i * size + j];
            }
        }
        return field;
    }

    public void init(string fieldSource)
    {
        var wallProcessor = new WallProcessor();
        var tanksProcessor = new TankProcessor();
        var undestroyableWallProcessor = new UndestroyableWallProcessor();
        var bulletProcessor = new BulletProcessor();

        char[][] field = to2Dimension(fieldSource);
        for (var i = 0; i < field.Length; i++)
        {
            for (var j = 0; j < field[i].Length; j++)
            {
                wallProcessor.process(field, i, j);
                tanksProcessor.process(field, i, j);
                undestroyableWallProcessor.process(field, i, j);
                bulletProcessor.process(field, i, j);
            }
        }
        // TODO: parse and map to inner values
    }

}

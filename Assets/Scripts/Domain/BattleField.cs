using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class BattleField
{

    public const string SAMPLE = "☼☼☼☼☼☼☼☼☼☼☼☼☼☼☼☼             ☼☼ ╬ ╬ ╬ ╬ ╬ ╬ ☼☼ ╬ ╬ ╬ ╬ ╬ ╬ ☼☼ ╬ ╬ ╬ ╬ ╬ ╬ ☼☼ ╬ ╬ ╬ ╬ ╬˅╬ ☼☼ ╬ ╬˂╬ ╬ ╬ ╬ ☼☼ ╬ ╬ ╬ ╬ ╬•╬ ☼☼ ╬ ╬ ╬ ╬ ╬ ╬ ☼☼ ╬ ╬ ╠ ╬ ╬ ╬ ☼☼ ╬ ╬ ╬ ╬ ╬ ╬ ☼☼ ╬ ╬ ╬ ╬ ╬ ╬ ☼☼ ╬▲╬ ╬ ╬ ╬ ╬ ☼☼             ☼☼☼☼☼☼☼☼☼☼☼☼☼☼☼☼";

    /*01234                           
    0 ☼☼☼☼☼                           
    1 ☼╬•˅☼                           
    2 ☼ ˂╬☼                           
    3 ☼ ╬╬☼                           
    4 ☼☼☼☼☼ */
    public const string SAMPLE_SMALL = "☼☼☼☼☼☼╬•˅☼☼ ˂╬☼☼ ╬╬☼☼☼☼☼☼";
    /*01234                           
    0 ☼☼☼☼☼                           
    1 ☼╨•˅☼                           
    2 ☼˅ ╬☼                           
    3 ☼ ╬╬☼                           
    4 ☼☼☼☼☼ */
    public const string SAMPLE_SMALL_1 = "☼☼☼☼☼☼╨•˅☼☼˅ ╬☼☼ ╬╬☼☼☼☼☼☼";

    private char[] field;

    public static char[][] to2Dimension(string fieldSource)
    {
        return to2Dimension(fieldSource.ToCharArray());
    }

    public static void printCharsField(char[][] field)
    {
        for (var i = 0; i < field.Length; i++)
        {
            var builder = new StringBuilder();
            for (var j = 0; j < field[i].Length; j++)
            {
                builder.Append(field[i][j]);
            }
            Debug.Log(builder.ToString());
        }
    }

    public static char[][] to2Dimension(char[] fieldSource)
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
}

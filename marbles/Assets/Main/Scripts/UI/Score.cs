using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Score : IComparable<Score>
{
    public Score(string name, float time)
    {
        Name = name;
        Time = time;
    }

    public string Name;
    public float Time;

    public int CompareTo(Score other)
    {
        if (this.Time < other.Time)
        {
            return -1;
        }
        else
        {
            return 1;
        }
    }
}

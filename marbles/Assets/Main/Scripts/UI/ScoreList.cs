using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScoreList
{
    public ScoreList()
    {
        Scores = new();
    }

    public List<Score> Scores;
}

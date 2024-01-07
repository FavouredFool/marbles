using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    [SerializeField] Leaderboard leaderboard;

    float _startTime = float.PositiveInfinity;
    float _bestTime = float.PositiveInfinity;

    public void GoalReached()
    {
        if (_startTime != float.PositiveInfinity)
        {
            float endTime = Time.time - _startTime;
            leaderboard.AddScore(endTime);

            if (endTime < _bestTime)
            {
                _bestTime = endTime;
            }
        }

        _startTime = Time.time;
    }

    public float GetStartTime()
    {
        return _startTime;
    }

    public float GetBestTime()
    {
        return _bestTime;
    }
}

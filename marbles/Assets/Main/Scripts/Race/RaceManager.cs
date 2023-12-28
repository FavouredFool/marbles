using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    float _startTime = float.PositiveInfinity;

    bool _isGoal = false;

    public void GoalReached()
    {
        if (!_isGoal)
        {
            _startTime = Time.time;
            Debug.Log("start!");
            _isGoal = true;
        }
        else
        {
            float endTime = Time.time - _startTime;
            Debug.Log($"end! Time: {endTime}");
        }
    }
}

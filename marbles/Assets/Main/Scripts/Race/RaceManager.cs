using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    float _startTime = float.PositiveInfinity;

    public void GoalReached()
    {
        if (_startTime == float.PositiveInfinity)
        {
            //Debug.Log("Start!");
        }
        else
        {
            float endTime = Time.time - _startTime;
            Debug.Log($"Laptime: {endTime}");
        }

        _startTime = Time.time;
    }
}

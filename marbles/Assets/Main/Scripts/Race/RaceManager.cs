using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    float _startTime = float.PositiveInfinity;

    public void RaceReached(bool isGoal)
    {
        if (!isGoal)
        {
            _startTime = Time.time;
            Debug.Log("start!");
        }
        else
        {
            float endTime = Time.time - _startTime;
            Debug.Log($"end! Time: {endTime}");
        }
    }
}

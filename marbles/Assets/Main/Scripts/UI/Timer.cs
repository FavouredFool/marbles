using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] RaceManager raceManager;
    [SerializeField] TMP_Text timerText;
    [SerializeField] TMP_Text bestTimeText;

    public void Update()
    {
        float timeToDisplay = Time.time - raceManager.GetStartTime();

        if (timeToDisplay != float.PositiveInfinity && timeToDisplay != float.NegativeInfinity)
        {
            timerText.text = timeToDisplay.ToString("00.000") + "s";
        }
        else
        {
            timerText.text = "00:000";
        }

        // Best Time Display
        float bestTime = raceManager.GetBestTime();

        if (bestTime != float.PositiveInfinity && bestTime != float.NegativeInfinity)
        {
            bestTimeText.text = bestTime.ToString("00.000") + "s";
        }
        else
        {
            bestTimeText.text = "XX:XXX";
        }

        
    }
}

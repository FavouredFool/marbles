using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] RaceManager raceManager;
    [SerializeField] TMP_Text timerText;
    [SerializeField] TMP_Text bestTimeText;
    [SerializeField] TMP_Text lastRoundText;

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


        float lastRoundTime = raceManager.GetLastRoundTime();

        if (lastRoundTime != float.PositiveInfinity && lastRoundTime != float.NegativeInfinity)
        {
            lastRoundText.text = lastRoundTime.ToString("00.000") + "s";
        }
        else
        {
            lastRoundText.text = "XX:XXX";
        }

        
    }
}

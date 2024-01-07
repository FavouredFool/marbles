using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Reset reset;
    [SerializeField] Leaderboard leaderboard;
    [SerializeField] Timer timer;
    [SerializeField] Countdown countdown;
    [SerializeField] Settings settings;

    public void StartRacing()
    {
        reset.gameObject.SetActive(true);
        gameObject.SetActive(false);
        leaderboard.ToggleVisuals(false);
        timer.gameObject.SetActive(true);
        countdown.gameObject.SetActive(true);
    }
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

    public void Settings()
    {
        settings.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Reset reset;
    [SerializeField] Leaderboard leaderboard;
    [SerializeField] Timer timer;
    [SerializeField] Countdown countdown;
    [SerializeField] GameObject settingsVisuals;

    AudioManager audioManager;

    public void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void StartRacing()
    {
        reset.gameObject.SetActive(true);
        gameObject.SetActive(false);
        leaderboard.ToggleVisuals(false);
        timer.gameObject.SetActive(true);
        countdown.gameObject.SetActive(true);
        audioManager.Play("Menu");
    }
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif

        audioManager.Play("Menu");
    }

    public void Settings()
    {
        settingsVisuals.SetActive(true);
        gameObject.SetActive(false);

        audioManager.Play("Menu");
    }
}

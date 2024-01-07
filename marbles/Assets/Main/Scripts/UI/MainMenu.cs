using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] Reset reset;
    [SerializeField] Leaderboard leaderboard;
    [SerializeField] Timer timer;

    public void StartRacing()
    {
        player.AllowStartRacing(true);
        reset.gameObject.SetActive(true);
        gameObject.SetActive(false);
        leaderboard.ToggleVisuals(false);
        timer.gameObject.SetActive(true);
    }
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}

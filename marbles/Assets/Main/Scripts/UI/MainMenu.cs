using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] Reset reset;
    [SerializeField] Leaderboard leaderboard;

    public void StartRacing()
    {
        player.AllowStartRacing(true);
        reset.gameObject.SetActive(true);
        gameObject.SetActive(false);
        leaderboard.ToggleVisuals(false);
    }
}

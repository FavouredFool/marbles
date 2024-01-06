using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] Reset reset;

    public void StartRacing()
    {
        player.AllowStartRacing(true);
        reset.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}

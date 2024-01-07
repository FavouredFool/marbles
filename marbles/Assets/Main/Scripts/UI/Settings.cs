using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    [SerializeField] MainMenu mainMenu;


    public void SetVolume(float volume)
    {
        
    }

    public void Back()
    {
        mainMenu.gameObject.SetActive(true);
        gameObject.SetActive(false);
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

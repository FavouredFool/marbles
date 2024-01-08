using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] MainMenu mainMenu;
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] string fileName = "Settings";

    [SerializeField] Toggle fullscreenToggle;
    [SerializeField] Slider volumeSlider;

    SettingsSerialized settings;
    string dataPath;


    public void Awake()
    {
        settings = new();
    }

    public void Start()
    {
        // Read Settings from File
        dataPath = Application.persistentDataPath + "/" + fileName + ".json";

        if (File.Exists(dataPath))
        {
            string fileContents = File.ReadAllText(dataPath);
            settings = JsonUtility.FromJson<SettingsSerialized>(fileContents);
            SetFullscreen(settings.fullscreen);
            SetVolume(settings.volume);
        }
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        settings.fullscreen = isFullscreen;
        fullscreenToggle.isOn = isFullscreen;

        // Write to file
        string jsonString = JsonUtility.ToJson(settings);
        File.WriteAllText(dataPath, jsonString);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", Mathf.Log10(volume) * 20);
        settings.volume = volume;
        volumeSlider.value = volume;

        // Write to file
        string jsonString = JsonUtility.ToJson(settings);
        File.WriteAllText(dataPath, jsonString);
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

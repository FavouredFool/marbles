using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    public float totalBGVolume = 0.5f;
    public Sound[] sounds;

    [Range(0.01f, 1)]
    public float BGMoveDelta = 0.1f;

    float currentSpeed = 0;

    Rigidbody playerRB;

    private void Awake()
    {
        AudioManager[] audioManagers;
        audioManagers = FindObjectsOfType<AudioManager>();

        if (audioManagers.Length > 1)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

            s.source.playOnAwake = false;
        }
    }

    public void Start()
    {
        Play("AmbientBG");
        Play("DrumBG");
        Play("FullBG");
    }

    public void Update()
    {
        if (playerRB == null)
        {
            playerRB = FindObjectOfType<Player>().GetComponent<Rigidbody>();
        }

        DynamicBGVolume();
    }

    public void DynamicBGVolume()
    {
        float speed = Vector3.Dot(playerRB.velocity, playerRB.transform.forward);

        currentSpeed = Mathf.MoveTowards(currentSpeed, speed, BGMoveDelta);
        // ranges: Ambient 0-1: 15, 5
        // ranges: Drum 0-1: 5, 25
        // ranges: Full 0-1: 15, 35

        float ambientT = Mathf.Clamp01(Remap(currentSpeed, 22, 5, 0, 1));
        // This one keeps being at t = 1 even over 25
        float drumT = Mathf.Clamp01(Remap(currentSpeed, 15, 28, 0, 1));
        float fullT = Mathf.Clamp01(Remap(currentSpeed, 25.5f, 45, 0, 1));

        float totalT = ambientT + drumT + fullT;

        float ambientPercentage = ambientT / totalT;
        float drumPercentage = drumT / totalT;
        float fullPercentage = fullT / totalT;

        float ambientVolume = ambientPercentage * totalBGVolume;
        float drumVolume = drumPercentage * totalBGVolume;
        float fullVolume = fullPercentage * totalBGVolume;

        Volume("AmbientBG", ambientVolume);
        Volume("DrumBG", drumVolume);
        Volume("FullBG", fullVolume);
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound not found");
            return;
        }

        s.source.Play();
    }

    public void Mute(string name, bool mute)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound not found");
            return;
        }

        s.source.mute = mute;
    }

    public void Volume(string name, float volume)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound not found");
            return;
        }

        s.source.volume = volume;
    }

    public AudioSource GetSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound not found");
            return null;
        }
        return s.source;
    }

    float Remap(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class Countdown : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] TMP_Text countdownText;
    

    void Start()
    {
        StartCoroutine(CountdownCoroutine());
    }

    IEnumerator CountdownCoroutine()
    {
        AudioManager audioManager = Object.FindObjectOfType<AudioManager>();

        countdownText.text = "";
        yield return new WaitForSeconds(1);

        countdownText.fontSize = 50;
        countdownText.DOFontSize(countdownText.fontSize + 50, 1);
        audioManager.Play("Countdown");
        audioManager.Pitch("Countdown", 0.65f);
        audioManager.Volume("Countdown", 0.5f);

        countdownText.text = "3";
        yield return new WaitForSeconds(1);

        countdownText.fontSize = 100;
        countdownText.DOFontSize(countdownText.fontSize + 50, 1);
        audioManager.Play("Countdown");
        audioManager.Pitch("Countdown", 0.75f);
        audioManager.Volume("Countdown", 0.6f);

        countdownText.text = "2";
        yield return new WaitForSeconds(1);

        countdownText.fontSize = 150;
        countdownText.DOFontSize(countdownText.fontSize + 50, 1);
        audioManager.Play("Countdown");
        audioManager.Pitch("Countdown", 0.85f);
        audioManager.Volume("Countdown", 0.7f);

        countdownText.text = "1";
        yield return new WaitForSeconds(1);

        countdownText.fontSize = 200;
        countdownText.DOFontSize(countdownText.fontSize + 25, 0.5f);
        audioManager.Play("Countdown");
        audioManager.Pitch("Countdown", 1f);
        audioManager.Volume("Countdown", 0.9f);

        countdownText.text = "GO!!";
        player.AllowStartRacing(true);

        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);

        
    }
}

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
        countdownText.text = "";
        yield return new WaitForSeconds(1);

        countdownText.fontSize = 50;
        countdownText.DOFontSize(countdownText.fontSize + 50, 1);

        countdownText.text = "3";
        yield return new WaitForSeconds(1);

        countdownText.fontSize = 100;
        countdownText.DOFontSize(countdownText.fontSize + 50, 1);

        countdownText.text = "2";
        yield return new WaitForSeconds(1);

        countdownText.fontSize = 150;
        countdownText.DOFontSize(countdownText.fontSize + 50, 1);

        countdownText.text = "1";
        yield return new WaitForSeconds(1);

        countdownText.fontSize = 200;
        countdownText.DOFontSize(countdownText.fontSize + 25, 0.5f);

        countdownText.text = "GO!!";
        player.AllowStartRacing(true);

        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);

        
    }
}

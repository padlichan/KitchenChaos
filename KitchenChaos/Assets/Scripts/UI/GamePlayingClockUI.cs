using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayingClockUI : MonoBehaviour
{
    [SerializeField] private Image timerImage;

    private void Start()
    {
        timerImage.fillAmount = 1f;
    }
    private void Update()
    {
        timerImage.fillAmount = GameManager.Instance.GetGamePlayingTimerNormalised();
        if (timerImage.fillAmount <= 0.15f && timerImage.color != Color.red) timerImage.color = Color.red;
    }
}

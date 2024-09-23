using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI recipesDeliveredNumberText;

    private void Start()
    {
        GameManager.Instance.OnStateChange += GameManager_OnStateChange;
        Hide();
    }

    private void GameManager_OnStateChange(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsGameOver()) Show();
        else Hide();
    }

    private void Show()
    {
        gameObject.SetActive(true);
        recipesDeliveredNumberText.text = DeliveryManager.Instance.correctOrdersDeliveredAmount.ToString();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

}

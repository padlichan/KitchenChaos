using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterViual : MonoBehaviour
{
    [SerializeField] private GameObject stoveOnGameObject;
    [SerializeField] private GameObject particlesGameObject;
    [SerializeField] private StoveCounter stoveCounter;

    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        if (e.state == StoveState.Frying || e.state == StoveState.Fried) Show();
        else Hide();
    }

    private void Show()
    {
        particlesGameObject.SetActive(true);
        stoveOnGameObject.SetActive(true);
    }

    private void Hide()
    {
        particlesGameObject.SetActive(false);
        stoveOnGameObject.SetActive(false);
    }
}

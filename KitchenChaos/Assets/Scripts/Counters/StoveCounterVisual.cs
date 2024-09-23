using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    [SerializeField] private Transform[] stoveCounterVisualArray;

    private void Start()
    {
        stoveCounter.OnStateChange += StoveCounter_OnStateChange; ;
    }

    private void StoveCounter_OnStateChange(object sender, StoveCounter.OnStateChangeEventArgs e)
    {
        if (e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried)
        {
            Show();
        }
        else Hide();
    }

    private void Show()
    {
        foreach (Transform t in stoveCounterVisualArray) { t.gameObject.SetActive(true); }
    }

    private void Hide()
    {
        foreach (Transform t in stoveCounterVisualArray) { t.gameObject.SetActive(false); }
    }




}

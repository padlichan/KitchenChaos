using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter counter;
    [SerializeField] private GameObject[] selectedCounterVisual;

    private void Start()
    {
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
        
    }

    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if(counter == e.selectedCounter) { Show(); }
        else { Hide(); }
    }

    private void Show()
    {
        foreach(GameObject g in  selectedCounterVisual) 
        {
            g.SetActive(true);
        }
        
    }

    private void Hide()
    {
        foreach (GameObject g in selectedCounterVisual)
        {
            g.SetActive(false);
        }
    }
}

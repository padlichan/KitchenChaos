using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private Image barImage;
    [SerializeField] private GameObject hasProgressGameObject;
    private IHasProgress hasProgress;

    private void Start()
    {
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
        if (hasProgress == null)
        {
            Debug.LogError($"Game object {hasProgressGameObject} does not have a component that implements IHasProgress!");
        }

        hasProgress.OnProgressChange += IHasProgress_OnProgressChange;
        Hide();
    }

    private void IHasProgress_OnProgressChange(object sender, IHasProgress.OnProgresschangeEventArgs e)
    {
        barImage.fillAmount = e.progressNormalized;
        if (e.progressNormalized == 0 || e.progressNormalized >= 1) Hide();
        else Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);

    }
}

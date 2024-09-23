using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{

    private IHasProgress hasProgress;
    [SerializeField] GameObject hasProgressGameObject;
    [SerializeField] private Image progressBar;
    [SerializeField] private Image progressBarBackground;



    private void Start()
    {
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
        if (hasProgress == null) Debug.LogError("Game Object "+hasProgressGameObject+" does not have IHasProgress component.");
        hasProgress.OnProgressChanged += HasProgress_OnProgressChanged;
        HideVisuals();
    }

 
    private void HasProgress_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        progressBar.fillAmount = e.progressBarValueNormalized;
        if(progressBar.fillAmount== 0f || progressBar.fillAmount >= 1f)
        {
            HideVisuals();
        }
        else ShowVisuals();
    }

    
    public void ShowVisuals()
    {
        progressBar.gameObject.SetActive(true);
        progressBarBackground.gameObject.SetActive(true);
    }

    public void HideVisuals()
    {
        progressBar.gameObject.SetActive(false);
        progressBarBackground.gameObject.SetActive(false);
    }
}

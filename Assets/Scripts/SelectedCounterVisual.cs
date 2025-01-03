using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField] private BaseCounter counter;
    [SerializeField] private GameObject[] visualGameObjectArray;
    private void Start()
    {
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
        Hide();
    }

    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
    {
        if(e.selectedCounter == counter) Show();
        else Hide();
    }

    private void Show()
    {
        foreach(var visualGameObject in visualGameObjectArray) visualGameObject.SetActive(true);
    }

    private void Hide()
    {
        foreach(var visualGameObject in visualGameObjectArray) visualGameObject.SetActive(false);
    }
}

using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Start()
    {
        GameManager.Instance.OnGameStateChange += GameManager_OnGameStateChange;
        Hide();
    }

    private void GameManager_OnGameStateChange(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGameOver())
        {
            scoreText.text = DeliveryManager.Instance.getCorrectOrdersDelivered().ToString();
            Show();
        }
        else Hide();
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

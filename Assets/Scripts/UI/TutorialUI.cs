using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI KeyMoveUpText;
    [SerializeField] TextMeshProUGUI KeyMoveDownText;
    [SerializeField] TextMeshProUGUI KeyMoveLeftText;
    [SerializeField] TextMeshProUGUI KeyMoveRightText;
    [SerializeField] TextMeshProUGUI KeyInteractText;
    [SerializeField] TextMeshProUGUI KeyInteractAltText;
    [SerializeField] TextMeshProUGUI KeyPauseText;
    [SerializeField] TextMeshProUGUI GamepadKeyInteractText;
    [SerializeField] TextMeshProUGUI GamepadKeyInteractAltText;
    [SerializeField] TextMeshProUGUI GamepadKeyPauseText;

    private void Start()
    {
        InputHandler.Instance.OnBindingRebind += InputHandler_OnBindingRebind;
        GameManager.Instance.OnGameStateChange += InputHandler_OnGameStateChange;
        UpdateVisual();
        Show();
    }

    private void InputHandler_OnGameStateChange(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsCountdownToStartActive())
        {
            Hide();
        }
    }

    private void InputHandler_OnBindingRebind(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        KeyMoveUpText.text = InputHandler.Instance.GetBindingText(Binding.MoveUp);
        KeyMoveDownText.text = InputHandler.Instance.GetBindingText(Binding.MoveDown);
        KeyMoveLeftText.text = InputHandler.Instance.GetBindingText(Binding.MoveLeft);
        KeyMoveRightText.text = InputHandler.Instance.GetBindingText(Binding.MoveRight);
        KeyInteractText.text = InputHandler.Instance.GetBindingText(Binding.Interact);
        KeyInteractAltText.text = InputHandler.Instance.GetBindingText(Binding.InteractAlt);
        KeyPauseText.text = InputHandler.Instance.GetBindingText(Binding.Pause);
        GamepadKeyInteractText.text = InputHandler.Instance.GetBindingText(Binding.GamepadInteract);
        GamepadKeyInteractAltText.text = InputHandler.Instance.GetBindingText(Binding.GamepadInteractAlt);
        GamepadKeyPauseText.text = InputHandler.Instance.GetBindingText(Binding.GamepadPause);
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

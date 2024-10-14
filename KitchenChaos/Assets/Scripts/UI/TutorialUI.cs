using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI keyUpKeyboard;
    [SerializeField] private TextMeshProUGUI keyLeftKeyboard;
    [SerializeField] private TextMeshProUGUI keyDownKeyboard;
    [SerializeField] private TextMeshProUGUI keyRightKeyboard;
    [SerializeField] private TextMeshProUGUI keyInteractKeyboard;
    [SerializeField] private TextMeshProUGUI keyAltInteractKeyboard;

    [SerializeField] private TextMeshProUGUI keyPauseKeyboard;

    [SerializeField] private TextMeshProUGUI keyMoveController;
    [SerializeField] private TextMeshProUGUI keyInterctController;
    [SerializeField] private TextMeshProUGUI keyAltInteractController;
    [SerializeField] private TextMeshProUGUI keyPauseController;

    private void Start()
    {
        GameInput.Instance.OnKeyRebind += GameInput_OnKeyRebind;
        GameManager.Instance.OnStateChange += GameManager_OnStateChange;
        UpdateVisual();
        Show();
    }

    private void GameManager_OnStateChange(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsCountDownToStartActive()) Hide();
    }

    private void GameInput_OnKeyRebind(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        keyUpKeyboard.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveUp);

        keyDownKeyboard.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveDown);

        keyLeftKeyboard.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveLeft);

        keyRightKeyboard.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveRight);

        keyInteractKeyboard.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);

        keyAltInteractKeyboard.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlt);

        keyPauseKeyboard.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);

        keyInterctController.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Interact);

        keyAltInteractController.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_InteractAlt);

        keyPauseController.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Pause);
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

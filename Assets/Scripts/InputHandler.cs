using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    public static InputHandler Instance { get; private set; }

    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPause;

    private PlayerInputActions playerInputActions;

    private const string PLAYER_PREFS_BINDINGS = "InputBindings";

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Debug.LogError("Multiple instances of InputHandler in scene");

        playerInputActions = new PlayerInputActions();
        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))
        {
            playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        }
        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += playerInputActions_Interact_performed;
        playerInputActions.Player.InteractAlternate.performed += playerInputActions_InteractAlternate_performed;
        playerInputActions.Player.Pause.performed += playerInputActions_Pause_performed;

    }

    private void OnDestroy()
    {
        playerInputActions.Player.Interact.performed -= playerInputActions_Interact_performed;
        playerInputActions.Player.InteractAlternate.performed -= playerInputActions_InteractAlternate_performed;
        playerInputActions.Player.Pause.performed -= playerInputActions_Pause_performed;

        playerInputActions.Dispose();
    }

    private void playerInputActions_Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPause?.Invoke(this, EventArgs.Empty);
    }

    private void playerInputActions_InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void playerInputActions_Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        return inputVector;
    }

    public string GetBindingText(Binding binding)
    {
        switch (binding)
        {
            default:
            case Binding.MoveUp:
            return playerInputActions.Player.Move.bindings[1].ToDisplayString();
            case Binding.MoveDown:
            return playerInputActions.Player.Move.bindings[2].ToDisplayString();
            case Binding.MoveLeft:
            return playerInputActions.Player.Move.bindings[3].ToDisplayString();
            case Binding.MoveRight:
            return playerInputActions.Player.Move.bindings[4].ToDisplayString();
            case Binding.Interact:
            return playerInputActions.Player.Interact.bindings[0].ToDisplayString();
            case Binding.InteractAlt:
            return playerInputActions.Player.InteractAlternate.bindings[0].ToDisplayString();
            case Binding.Pause:
            return playerInputActions.Player.Pause.bindings[0].ToDisplayString();
        }
    }

    public void RebindBinding(Binding binding, Action onActionRebound)
    {
        playerInputActions.Player.Disable();

        InputAction inputAction;
        int bindingIndex;

        switch (binding)
        {
            default:
            case Binding.MoveUp:
            inputAction = playerInputActions.Player.Move;
            bindingIndex = 1;
            break;
            case Binding.MoveDown:
            inputAction = playerInputActions.Player.Move;
            bindingIndex = 2;
            break;
            case Binding.MoveLeft:
            inputAction = playerInputActions.Player.Move;
            bindingIndex = 3;
            break;
            case Binding.MoveRight:
            inputAction = playerInputActions.Player.Move;
            bindingIndex = 4;
            break;
            case Binding.Interact:
            inputAction = playerInputActions.Player.Interact;
            bindingIndex = 0;
            break;
            case Binding.InteractAlt:
            inputAction = playerInputActions.Player.InteractAlternate;
            bindingIndex = 0;
            break;
            case Binding.Pause:
            inputAction = playerInputActions.Player.Pause;
            bindingIndex = 0;
            break;
        }

        inputAction.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(callback =>
            {
                callback.Dispose();
                playerInputActions.Player.Enable();
                onActionRebound();
                PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerInputActions.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();
            })
            .Start();
    }
}

public enum Binding
{
    MoveUp,
    MoveDown,
    MoveLeft,
    MoveRight,
    Interact,
    InteractAlt,
    Pause
}

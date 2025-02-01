using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private enum GameState
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver
    }
    public static GameManager Instance { get; private set; }

    public event EventHandler OnGameStateChange;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;

    private GameState state;
    private GameState State
    {
        get { return state; }
        set
        {
            state = value;
            OnGameStateChange?.Invoke(this, EventArgs.Empty);
        }
    }
    private float countdownToStartTimer = 3;
    private float gamePlayingTimer = 10;
    private float gamePlayingTimerMax = 10;

    private bool isGamePaused = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        State = GameState.WaitingToStart;
    }

    private void Start()
    {
        InputHandler.Instance.OnPause += InputHandler_OnPause;
        InputHandler.Instance.OnInteractAction += InputHandler_OnInteractAction;
    }

    private void InputHandler_OnInteractAction(object sender, EventArgs e)
    {
        if (State == GameState.WaitingToStart)
        {
            State = GameState.CountdownToStart;
        }
    }

    private void InputHandler_OnPause(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    private void Update()
    {
        switch (State)
        {
            case GameState.WaitingToStart:

            break;
            case GameState.CountdownToStart:
            countdownToStartTimer -= Time.deltaTime;
            if (countdownToStartTimer < 0)
            {
                State = GameState.GamePlaying;
            }
            break;
            case GameState.GamePlaying:
            gamePlayingTimer -= Time.deltaTime;
            if (gamePlayingTimer < 0)
            {
                State = GameState.GameOver;
            }
            break;
            case GameState.GameOver:
            break;
        }
    }

    public void TogglePauseGame()
    {
        isGamePaused = !isGamePaused;
        if (isGamePaused)
        {
            Time.timeScale = 0f;
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1f;
            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsGamePlaying() => State == GameState.GamePlaying;
    public bool IsCountdownToStartActive() => State == GameState.CountdownToStart;
    public float GetCountdownToStartTimer() => countdownToStartTimer;
    public bool IsGameOver() => State == GameState.GameOver;
    public float GetGamePlayingTimerNormalized() => gamePlayingTimer / gamePlayingTimerMax;
}

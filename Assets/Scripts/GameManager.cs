using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public event EventHandler OnGameStateChange;
    public static GameManager Instance { get; private set; }
    private enum GameState
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver
    }

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
    private float waitingToStartTimer = 1;
    private float countdownToStartTimer = 3;
    private float gamePlayingTimer = 10;
    private float gamePlayingTimerMax = 10;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        State = GameState.WaitingToStart;
    }

    private void Update()
    {
        switch (State)
        {
            case GameState.WaitingToStart:
            waitingToStartTimer -= Time.deltaTime;
            if (waitingToStartTimer < 0)
            {
                State = GameState.CountdownToStart;
            }
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

    public bool IsGamePlaying() => State == GameState.GamePlaying;
    public bool IsCountdownToStartActive() => State == GameState.CountdownToStart;
    public float GetCountdownToStartTimer() => countdownToStartTimer;
    public bool IsGameOver() => State == GameState.GameOver;
    public float GetGamePlayingTimerNormalized() => gamePlayingTimer / gamePlayingTimerMax;
}

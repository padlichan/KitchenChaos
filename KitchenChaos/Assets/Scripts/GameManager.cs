using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private float waitingToStartTimer = 1f;
    [SerializeField] private float countdownToStartTimer = 3f;
    [SerializeField] private float gamePlayingTimer;
    private float gamePlayingTimerMax = 10f;
    private bool isGamePaused = false;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;


    public event EventHandler OnStateChange;


    private enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        Gameover
    }
    private State gameState;
    private State GameState
    {
        get { return gameState; }
        set
        {
            gameState = value;
            OnStateChange?.Invoke(this, EventArgs.Empty);
        }
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Debug.Log("More than one GameManager instances in scene");
        GameState = State.WaitingToStart;
        gamePlayingTimer = gamePlayingTimerMax;
    }
    private void Start()
    {
        GameInput.Instance.OnPause += GameInput_OnPause;
    }

    private void GameInput_OnPause(object sender, EventArgs e)
    {
        ToggleGamePause();
    }

    public void ToggleGamePause()
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

    private void Update()
    {
        switch (GameState)
        {
            case State.WaitingToStart:
                waitingToStartTimer -= Time.deltaTime;
                if (waitingToStartTimer <= 0) GameState = State.CountdownToStart;
                break;
            case State.CountdownToStart:
                countdownToStartTimer -= Time.deltaTime;
                if (countdownToStartTimer <= 0) GameState = State.GamePlaying;
                break;
            case State.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                if (gamePlayingTimer <= 0) GameState = State.Gameover;
                break;
            case State.Gameover:
                break;
        }
    }

    public bool IsGamePlaying()
    {
        return GameState == State.GamePlaying;
    }
    public bool IsCountDownToStartActive()
    {
        return GameState == State.CountdownToStart;
    }

    public bool IsGameOver()
    {
        return GameState == State.Gameover;
    }
    public float GetCountdownToStartTimer()
    {
        return countdownToStartTimer;
    }

    public float GetGamePlayingTimerNormalised()
    {
        return gamePlayingTimer / gamePlayingTimerMax;
    }

}

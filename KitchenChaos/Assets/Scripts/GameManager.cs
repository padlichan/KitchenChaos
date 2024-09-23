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
        Instance = this;
        GameState = State.WaitingToStart;
        gamePlayingTimer = gamePlayingTimerMax;
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

    public bool isGamePlaying()
    {
        return GameState == State.GamePlaying;
    }
    public bool isCountDownToStartActive()
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

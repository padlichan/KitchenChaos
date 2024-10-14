using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlateCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;
    private float spawnPlateTimer;
    private float spawnPlateTimerMax = 4f;
    private int plateSpawnedAmount;
    private int plateSpawnedAmountMax = 4;

    private void Start()
    {
        //Populate counter with plates
        for (int i = 0; i < plateSpawnedAmountMax; i++)
        {
            plateSpawnedAmount++;
            OnPlateSpawned?.Invoke(this, EventArgs.Empty);
        }
    }
    public override void Interact(Player player)
    {
        {
            if (!player.HasKitchenObject())
            {
                //pick up the kithen object
                if (plateSpawnedAmount > 0)
                {
                    KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
                    plateSpawnedAmount--;
                    OnPlateRemoved?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }
    private void Update()
    {
        if (GameManager.Instance.IsGamePlaying() && plateSpawnedAmount < plateSpawnedAmountMax) spawnPlateTimer += Time.deltaTime;
        if (spawnPlateTimer > spawnPlateTimerMax && !HasKitchenObject())
        {
            spawnPlateTimer = 0;
            if (plateSpawnedAmount < plateSpawnedAmountMax)
            {
                plateSpawnedAmount++;
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}

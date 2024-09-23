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
        if (plateSpawnedAmount < plateSpawnedAmountMax) spawnPlateTimer += Time.deltaTime;
        if (spawnPlateTimer > spawnPlateTimerMax && !HasKitchenObject())
        {
            //KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, this);
            spawnPlateTimer = 0;
            if (plateSpawnedAmount < plateSpawnedAmountMax)
            {
                plateSpawnedAmount++;
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}

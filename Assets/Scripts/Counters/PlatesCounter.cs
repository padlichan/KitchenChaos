using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;

    [SerializeField] private KitchenObjectSO plateKitchenObjectSO;
    private float plateSpawnTimer = 0;
    private float plateSpawnTimerMax = 2;
    private int platesCount = 0;
    private int platesCountMax = 4;

    private void Update()
    {
        if (platesCount < platesCountMax)
        {
            plateSpawnTimer += Time.deltaTime;
            if (plateSpawnTimer > plateSpawnTimerMax)
            {
                plateSpawnTimer = 0;
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
                platesCount++;
            }
        }

    }
    public override void Interact(Player player)
    {
        if (platesCount > 0)
        {
            if (!player.HasKitchenObject())
            {
                platesCount--;
                OnPlateRemoved?.Invoke(this, EventArgs.Empty);
                KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
            }
            else
            {
                PlateKitchenObject plate = KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, this) as PlateKitchenObject;
                if (plate.TryAddingredient(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    platesCount--;
                    OnPlateRemoved?.Invoke(this, EventArgs.Empty);
                    player.GetKitchenObject().DestroySelf();
                    plate.SetKitchenObjectParent(player);
                }
                else plate.DestroySelf();
            }
        }
    }
}

using System;
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

    private void Start()
    {
        for (int i = 0; i < platesCountMax; i++)
        {
            SpawnPlate();
        }
    }

    private void Update()
    {
        if (platesCount < platesCountMax)
        {
            plateSpawnTimer += Time.deltaTime;
            if (plateSpawnTimer > plateSpawnTimerMax)
            {
                plateSpawnTimer = 0;
                SpawnPlate();
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
        }
    }

    private void SpawnPlate()
    {
        OnPlateSpawned?.Invoke(this, EventArgs.Empty);
        platesCount++;
    }
}

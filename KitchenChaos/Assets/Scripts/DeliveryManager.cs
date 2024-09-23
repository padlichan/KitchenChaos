using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnOrderSpawned;
    public event EventHandler OnOrderDelivered;
    public event EventHandler OnWrongOrderDelivered;
    public static DeliveryManager Instance { get; private set; }

    public int correctOrdersDeliveredAmount { get; private set; }
    public int wrongOrdersDeliveredAmount { get; private set; }
    [SerializeField] private RecipeListSO recipeListSO;

    private List<RecipeSO> orderList;

    private float spawnOrderTimer = 0f;
    private float spawnOrderTimerMax = 1f;
    private int maxOrder = 3;
    private void Awake()
    {
        Instance = this;
        orderList = new List<RecipeSO>();
        correctOrdersDeliveredAmount = 0;
        wrongOrdersDeliveredAmount = 0;
    }
    private void Update()
    {
        if (orderList.Count < maxOrder)
        {
            spawnOrderTimer += Time.deltaTime;
            if (spawnOrderTimer > spawnOrderTimerMax)
            {
                spawnOrderTimer = 0;
                SpawnNewOrder();
            }
        }
    }

    private void SpawnNewOrder()
    {
        RecipeSO neworder = recipeListSO.recipeListSOList[UnityEngine.Random.Range(0, recipeListSO.recipeListSOList.Count)];
        orderList.Add(neworder);
        OnOrderSpawned?.Invoke(this, EventArgs.Empty);
    }

    public bool OrderDelivery(PlateKitchenObject plate)
    {
        foreach (RecipeSO r in orderList)
        {
            bool isDeliveryValid = true;
            if (r.kitchenObjectSOList.Count == plate.kitchenObjectSOList.Count)
            {
                //Has same number of ingredients
                foreach (KitchenObjectSO k in r.kitchenObjectSOList)
                {
                    if (!plate.kitchenObjectSOList.Contains(k))
                    {
                        //Delivery doesn't contain this ingredient
                        isDeliveryValid = false;
                        break;
                    }
                }
                if (isDeliveryValid)
                {
                    orderList.Remove(r);
                    OnOrderDelivered?.Invoke(this, EventArgs.Empty);
                    correctOrdersDeliveredAmount++;
                    return true;
                }

            }
        }
        OnWrongOrderDelivered?.Invoke(this, EventArgs.Empty);
        wrongOrdersDeliveredAmount++;
        return false;
    }

    public List<RecipeSO> GetOrderList()
    {
        return orderList;
    }
}

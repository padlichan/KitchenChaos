using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnOrderSpawned;
    public event EventHandler OnOrderDelivered;
    public event EventHandler OnDeliverySuccess;
    public event EventHandler OnDeliveryFailed;


    [SerializeField] private RecipeListSO validOrderList;
    private List<RecipeSO> pendingOrderList;
    private int pendingOrderMax = 4;

    private int correctOrdersDelivered = 0;

    private float orderSpawnTimer = 0;
    private float orderSpawnTimerMax = 2;

    public static DeliveryManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Debug.LogError("Multiple instances of DeliveryManager in scene");
        pendingOrderList = new();
    }
    private void Update()
    {
        if (pendingOrderList.Count < pendingOrderMax)
        {
            orderSpawnTimer += Time.deltaTime;
            if (orderSpawnTimer > orderSpawnTimerMax)
            {
                orderSpawnTimer = 0;

                var newOrder = validOrderList.recipeSOList[UnityEngine.Random.Range(0, validOrderList.recipeSOList.Count)];
                pendingOrderList.Add(newOrder);
                OnOrderSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public bool DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        var thingsOnPlate = plateKitchenObject.GetKitchenObjectSOList();
        var fulfilledOrder = pendingOrderList.FirstOrDefault(p => AreListsEqual(p.kitchenObjectSOList, thingsOnPlate));

        if (fulfilledOrder != null)
        {
            pendingOrderList.Remove(fulfilledOrder);
            OnOrderDelivered?.Invoke(this, EventArgs.Empty);
            OnDeliverySuccess?.Invoke(this, EventArgs.Empty);
            correctOrdersDelivered++;
            return true;
        }
        OnDeliveryFailed?.Invoke(this, EventArgs.Empty);
        return false;

    }

    private bool AreListsEqual(List<KitchenObjectSO> a, List<KitchenObjectSO> b)
    {
        var setA = new HashSet<KitchenObjectSO>(a);
        var setB = new HashSet<KitchenObjectSO>(b);

        return setA.SetEquals(setB);
    }

    public List<RecipeSO> GetPendingOrderList() => pendingOrderList;

    public int getCorrectOrdersDelivered() => correctOrdersDelivered;

}

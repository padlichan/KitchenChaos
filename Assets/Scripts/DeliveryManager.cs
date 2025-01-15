using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    [SerializeField] private RecipeListSO validOrderList;
    private List<RecipeSO> pendingOrderList;
    private int pendingOrderMax = 4;

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

                var newOrder = validOrderList.recipeSOList[Random.Range(0, validOrderList.recipeSOList.Count)];
                pendingOrderList.Add(newOrder);
                Debug.Log($"New order added: {newOrder.RecipeName}");
            }
        }

    }

    public bool DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        var thingsOnPlate = plateKitchenObject.GetKitchenObjectSOList();
        var fulfilledOrder = pendingOrderList.FirstOrDefault(p => AreListsEqual(p.kitchenObjectSOList, thingsOnPlate));

        if (fulfilledOrder != null)
        {
            Debug.Log($"Correct order: {fulfilledOrder.RecipeName}");
            pendingOrderList.Remove(fulfilledOrder);
            return true;
        }
        Debug.Log("Wrong order!");
        return false;

    }

    private bool AreListsEqual(List<KitchenObjectSO> a, List<KitchenObjectSO> b)
    {
        var setA = new HashSet<KitchenObjectSO>(a);
        var setB = new HashSet<KitchenObjectSO>(b);

        return setA.SetEquals(setB);
    }
}

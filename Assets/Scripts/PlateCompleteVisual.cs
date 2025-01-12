using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]
    public struct KitchenObjectSO_GameObject
    {
        public KitchenObjectSO KitchenObjectSO;
        public GameObject GameObject;
    }

    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private List<KitchenObjectSO_GameObject> KitchenObjectSO_GameObjectList;

    private void Start()

    {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
    }


    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        KitchenObjectSO_GameObjectList.Where(kg => kg.KitchenObjectSO == e.kitchenObjectSO)
                                      .FirstOrDefault().GameObject
                                      .SetActive(true);
    }
}

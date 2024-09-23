using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateVisual : MonoBehaviour
{
    [SerializeField] PlateKitchenObject plateKitchenObject;
    [SerializeField] private List<KitchenObjectSO_GameObject> kitchenObjectSO_GameObjectList;
    [Serializable]
    public struct KitchenObjectSO_GameObject
    {
        public KitchenObjectSO kitchenObjectSO;
        public GameObject gameObject;
    }

    private void Start()
    {
        plateKitchenObject.OnIngredientAdded += UpdatePlateVisual;
    }

    private void UpdatePlateVisual(object sender, PlateKitchenObject.OnIngerdientAddedEventArgs e)
    {
        //Update visuals
        foreach (KitchenObjectSO_GameObject kitchenObjectSO_GameObject in kitchenObjectSO_GameObjectList)
        {
            if (kitchenObjectSO_GameObject.kitchenObjectSO == e.kitchenObjectSO)
            {
                kitchenObjectSO_GameObject.gameObject.SetActive(true);
            }
        }
    }
}

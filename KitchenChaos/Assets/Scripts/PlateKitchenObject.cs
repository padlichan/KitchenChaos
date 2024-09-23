using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public List<KitchenObjectSO> kitchenObjectSOList { get; private set; }
    [SerializeField] List<KitchenObjectSO> ValidKitchenObjectSOList;
    public event EventHandler<OnIngerdientAddedEventArgs> OnIngredientAdded;
    public static event EventHandler OnAnyIngredientAdded;

    public class OnIngerdientAddedEventArgs : EventArgs
    {
        public KitchenObjectSO kitchenObjectSO;
    }

    private void Start()
    {
        kitchenObjectSOList = new List<KitchenObjectSO>();
    }
    public bool TryAddIngredient(KitchenObjectSO inputKitchenObjectSO)
    {
        if (ValidKitchenObjectSOList.Contains(inputKitchenObjectSO))
        {
            //kitchenObjectSO is valid
            if (!kitchenObjectSOList.Contains(inputKitchenObjectSO))
            {
                //Does not yet have kitchenObjectSO
                kitchenObjectSOList.Add(inputKitchenObjectSO);
                OnIngredientAdded?.Invoke(this, new OnIngerdientAddedEventArgs { kitchenObjectSO = inputKitchenObjectSO });
                OnAnyIngredientAdded?.Invoke(this, EventArgs.Empty);
                return true;
            }
        }
        return false;
    }

    public List<KitchenObjectSO> GetkitchenObjectSOList()
    {
        return kitchenObjectSOList;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class ClearCounter : BaseCounter
{

    [SerializeField] private KitchenObjectSO kitchenObjectSO;


    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // Counter doesn't have kitchen object
            if (player.HasKitchenObject())
            {
                //Player has kitchen object
                player.GetKitchenObject().SetKitchenObjectParent(this);

            }
            else
            {
                //Player doesn't have kitchen object
            }
        }
        else
        {
            //Counter has kitchen object
            if (!player.HasKitchenObject())
            {
                //Player doesn't have kitchen object
                //Pick up the kithen object
                GetKitchenObject().SetKitchenObjectParent(player);
            }
            else
            //Counter has kitchenObject AND Player has kitchenObject
            if (GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {
                //Counter has plateKitchenObject
                if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    //KitcheObject can be added to plate
                    player.GetKitchenObject().DestroySelf();
                }
            }
            else if (player.GetKitchenObject().TryGetPlate(out plateKitchenObject))
            {
                //Player has plateKitchenObject
                if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                {
                    //KitchenObject can be added to plate
                    GetKitchenObject().DestroySelf();
                }
            }
        }
    }
}

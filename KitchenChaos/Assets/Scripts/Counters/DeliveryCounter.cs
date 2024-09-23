using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        if (player.GetKitchenObject() != null)
        {
            if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {
                DeliveryManager.Instance.OrderDelivery(plateKitchenObject);
                player.GetKitchenObject().DestroySelf();
            }
        }
    }
}

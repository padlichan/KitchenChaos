using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    public event EventHandler OnItemGrabbedFromContainer;
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player)
    {

        if (!player.HasKitchenObject())
        {
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);

            /*
            Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.itemPrefab);
            kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(player);
            */

            OnItemGrabbedFromContainer?.Invoke(this, EventArgs.Empty);
        }
        else Debug.Log("Player already has kitchen object");

    }
}

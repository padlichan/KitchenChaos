using System;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    public event EventHandler OnPlayerGrabObject;

    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if(player.HasKitchenObject())
            {
                KitchenObject playerKitchenObject = player.GetKitchenObject();
                if(playerKitchenObject.GetKitchenObjectSO() == kitchenObjectSO)
                {
                    playerKitchenObject.SetKitchenObjectParent(this);
                }
            }
            else 
            {
                KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);
                OnPlayerGrabObject?.Invoke(this, EventArgs.Empty);
            }
        }
        else
        {
            if(!player.HasKitchenObject())
            {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
}

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
                if(playerKitchenObject.GetKitchenObjectSO().Name == kitchenObjectSO.Name)
                {
                    playerKitchenObject.SetKitchenObjectParent(this);
                }
            }
            else 
            {
                Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.Prefab);
                kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(player);
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

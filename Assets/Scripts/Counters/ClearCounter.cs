using UnityEngine;

public class ClearCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            //Counter does not have KO
            if(player.HasKitchenObject())
            {
                //Player has KO
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
        }
        else
        {
            //Counter has KO
            if (!player.HasKitchenObject())
            {
                //Player does not have KO
                GetKitchenObject().SetKitchenObjectParent(player);
            }
            else
            {
                //Player has KO
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plate))
                {
                    //Player has plate
                    if(plate.TryAddingredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }
                else if(GetKitchenObject().TryGetPlate(out plate))
                {
                    //Counter has plate
                    if (plate.TryAddingredient(player.GetKitchenObject().GetKitchenObjectSO()))
                    {
                        player.GetKitchenObject().DestroySelf();
                    }
                }
            }
        }
    }
}

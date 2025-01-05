using System.Linq;
using UnityEngine;
using UnityEngine.Windows;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            //TO DO: If player has KO move KO to counter
            if (player.HasKitchenObject())
            {
                if(CanBePlaced(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                }
            }
        }
        else
        {
            //TO DO: If player doesn't have KO, give KO to player
            if (!player.HasKitchenObject())
            {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        if(HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
        {
            KitchenObjectSO output = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
            GetKitchenObject().DestroySelf();
            KitchenObject.SpawnKitchenObject(output, this);
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSO kitchenObjectSO)
    {
        return cuttingRecipeSOArray.Where(c => c.Input == kitchenObjectSO).FirstOrDefault() != null;
    }

    private bool CanBePlaced(KitchenObjectSO kitchenObjectSO)
    {
        return cuttingRecipeSOArray.Where(c => c.Input == kitchenObjectSO || c.Output == kitchenObjectSO).FirstOrDefault() != null;
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO input)
    {
        return cuttingRecipeSOArray.Where(c => c.Input == input).FirstOrDefault().Output;
    }
}

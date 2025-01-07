using System.Linq;
using UnityEngine;
using UnityEngine.Windows;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    private int cuttingProgress;
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                if(CanBePlaced(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProgress = 0;
                }
            }
        }
        else
        {
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
            cuttingProgress++;
            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
            if(cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
            {
                KitchenObjectSO output = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
                GetKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(output, this);
            }
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

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO input)
    {
        return cuttingRecipeSOArray.Where(c => c.Input == input).FirstOrDefault();
    }
}

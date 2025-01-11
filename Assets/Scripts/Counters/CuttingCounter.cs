using System;
using System.Linq;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    public event EventHandler OnCut;
    public event EventHandler<IHasProgress.OnProgresschangeEventArgs> OnProgressChange;

    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    private int cuttingProgress;
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                if (CanBePlaced(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    if (CanBeCut(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                        cuttingProgress = Mathf.RoundToInt(GetKitchenObject().CuttingProgressNormalized * cuttingRecipeSO.cuttingProgressMax);
                    }
                    OnProgressChange?.Invoke(this, new IHasProgress.OnProgresschangeEventArgs { progressNormalized = GetKitchenObject().CuttingProgressNormalized });
                }
            }
        }
        else
        {
            if (!player.HasKitchenObject())
            {
                GetKitchenObject().SetKitchenObjectParent(player);
                OnProgressChange?.Invoke(HasKitchenObject(), new IHasProgress.OnProgresschangeEventArgs {progressNormalized = 0 });  
            }
            else
            {
                //Player has KO
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plate))
                {
                    //Player has plate
                    if (plate.TryAddingredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                }
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && CanBeCut(GetKitchenObject().GetKitchenObjectSO()))
        {
            OnCut?.Invoke(this, EventArgs.Empty);
            cuttingProgress++;
            KitchenObject kitchenObject = GetKitchenObject();
            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(kitchenObject.GetKitchenObjectSO());
            kitchenObject.CuttingProgressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax;
            OnProgressChange?.Invoke(this, new IHasProgress.OnProgresschangeEventArgs { progressNormalized = kitchenObject.CuttingProgressNormalized });
            if (kitchenObject.CuttingProgressNormalized >= 1)
            {
                OnProgressChange?.Invoke(this, new IHasProgress.OnProgresschangeEventArgs { progressNormalized = 0 });
                KitchenObjectSO output = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
                GetKitchenObject().DestroySelf();
                KitchenObject.SpawnKitchenObject(output, this);
            }
        }
    }

    private bool CanBeCut(KitchenObjectSO kitchenObjectSO)
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

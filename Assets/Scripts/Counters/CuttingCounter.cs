using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows;

public class CuttingCounter : BaseCounter
{
    public event EventHandler OnCut;
    public event EventHandler<OnProgresschangeEventArgs> OnProgressChange;
    public class OnProgresschangeEventArgs : EventArgs
    {
        public float progressNormalized;
    }

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
                    if(CanBeCut(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
                        cuttingProgress = Mathf.RoundToInt(GetKitchenObject().cuttingProgressNormalized*cuttingRecipeSO.cuttingProgressMax);
                    }
                    OnProgressChange?.Invoke(this, new OnProgresschangeEventArgs{progressNormalized = GetKitchenObject().cuttingProgressNormalized});
                }
            }
        }
        else
        {
            if (!player.HasKitchenObject())
            {
                GetKitchenObject().SetKitchenObjectParent(player);
                OnProgressChange?.Invoke(HasKitchenObject(), new OnProgresschangeEventArgs{progressNormalized = 0 });  
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
            kitchenObject.cuttingProgressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax;
            OnProgressChange?.Invoke(this, new OnProgresschangeEventArgs { progressNormalized = kitchenObject.cuttingProgressNormalized });
            if (kitchenObject.cuttingProgressNormalized >= 1)
            {
                OnProgressChange?.Invoke(this, new OnProgresschangeEventArgs { progressNormalized = 0 });
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

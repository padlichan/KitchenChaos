using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress
{
    [SerializeField] private KitchenObjectSO cutKitchenObjectSO;
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;
    [SerializeField] private ProgressBarUI progressBarUI;

    public event EventHandler OnCut;
    public static event EventHandler OnAnyCut;
    new public static void ResetStaticData()
    {
        OnAnyCut = null;
    }
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;


    private int cuttingProgress = 0;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            //Counter has no kitchenObject
            if (player.HasKitchenObject())
            {
                //Player has kitchen object
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    //Player is carrying something that can be cut
                    player.GetKitchenObject().SetKitchenObjectParent(this);

                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSoFrominput(GetKitchenObject().GetKitchenObjectSO());
                    cuttingProgress = GetKitchenObject().GetCuttingProgress();
                    float cuttingProgressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax;
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressBarValueNormalized = cuttingProgressNormalized });
                }
            }
            //Player has no kitchenObject
        }
        else
        {
            if (!player.HasKitchenObject())
            {
                //Player has no kitchenObject
                GetKitchenObject().SetKitchenObjectParent(player);
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressBarValueNormalized = 0 });

            }
            else
            {
                //Counter has kitchenObject AND player has kitchenObject
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
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

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject() && !player.HasKitchenObject())
        {
            //Counter has kitchen object

            if (HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
            {
                OnCut?.Invoke(this, EventArgs.Empty);
                OnAnyCut?.Invoke(this, EventArgs.Empty);
                CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSoFrominput(GetKitchenObject().GetKitchenObjectSO());
                cuttingProgress++;
                GetKitchenObject().SetCuttingProgress(cuttingProgress);
                float progressBarValueNormalised = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax;
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressBarValueNormalized = progressBarValueNormalised });

                if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
                {
                    //Cutting is done
                    KitchenObjectSO outKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
                    GetKitchenObject().DestroySelf();
                    KitchenObject.SpawnKitchenObject(outKitchenObjectSO, this);
                    cuttingProgress = 0;


                }
            }
        }
    }

    private KitchenObjectSO GetOutputForInput(KitchenObjectSO kitchenObjectSO)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSoFrominput(kitchenObjectSO);
        if (cuttingRecipeSO != null) return cuttingRecipeSO.output;
        else return null;
    }

    private bool HasRecipeWithInput(KitchenObjectSO kitchenObjectSO)
    {
        return GetCuttingRecipeSoFrominput(kitchenObjectSO) != null;
    }

    private CuttingRecipeSO GetCuttingRecipeSoFrominput(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO c in cuttingRecipeSOArray)
        {
            if (c.input == inputKitchenObjectSO)
            {
                return c;
            }
        }
        return null;
    }
}

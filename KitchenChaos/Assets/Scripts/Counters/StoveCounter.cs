using UnityEngine;
using System;

public class StoveCounter : BaseCounter, IHasProgress
{
    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;
    [SerializeField] private ProgressBarUI progressBarUI;

    public event EventHandler<OnStateChangeEventArgs> OnStateChange;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public class OnStateChangeEventArgs : EventArgs
    {
        public State state;
    }


    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;

    private float fryingProgress = 0f;
    private float burningProgress = 0f;

    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burned,
    }

    public State state;



    private void Start()
    {
        state = State.Idle;
    }

    private void Update()
    {
        if (HasKitchenObject())
        {
            StoveState();
        }
    }

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            //Counter does not have kitchenObject
            if (player.HasKitchenObject())
            {
                //Player has kitchenObject
                if (HasFryingRecipeSOWithInputSO(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    //Player is carrying something that can be fried
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    fryingRecipeSO = GetFryingRecipeSOFromInputSO(GetKitchenObject().GetKitchenObjectSO());
                    fryingProgress = GetKitchenObject().GetFryingProgress();
                    state = State.Frying;
                    OnStateChange?.Invoke(this, new OnStateChangeEventArgs { state = state });
                }

            }
            //Player does not have kitchenObject
        }
        else
        {
            //Counter has kitchenObject
            if (!player.HasKitchenObject())
            {
                //Player does not have kitchenObject
                //Pick up kitchenObject 
                GetKitchenObject().SetFryingProgress(fryingProgress);
                GetKitchenObject().SetKitchenObjectParent(player);
                fryingRecipeSO = null;
                state = State.Idle;
                OnStateChange?.Invoke(this, new OnStateChangeEventArgs { state = state });
                fryingProgress = 0f;
                burningProgress = 0f;
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressBarValueNormalized = 0f });

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

                        fryingRecipeSO = null;
                        state = State.Idle;
                        OnStateChange?.Invoke(this, new OnStateChangeEventArgs { state = state });
                        fryingProgress = 0f;
                        burningProgress = 0f;
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressBarValueNormalized = 0f });
                    }

                }
            }
        }
    }

    private void StoveState()
    {
        switch (state)
        {
            case State.Idle:
                break;
            case State.Frying:
                fryingProgress += Time.deltaTime;
                float fryingProgressNormalized = fryingProgress / GetFryingRecipeSOFromInputSO(GetKitchenObject().GetKitchenObjectSO()).fryingProgressMax;
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressBarValueNormalized = fryingProgressNormalized });

                if (fryingProgress > fryingRecipeSO.fryingProgressMax)
                {
                    KitchenObjectSO outKitchenObjectSO = GetFryingOutputSOForInputSO(GetKitchenObject().GetKitchenObjectSO());
                    GetKitchenObject().DestroySelf();
                    KitchenObject.SpawnKitchenObject(outKitchenObjectSO, this);
                    state = State.Fried;
                    OnStateChange?.Invoke(this, new OnStateChangeEventArgs { state = state });
                    burningProgress = 0f;
                    burningRecipeSO = GetBurningRecipeSOFromInputSO(GetKitchenObject().GetKitchenObjectSO());



                }
                break;
            case State.Fried:
                burningProgress += Time.deltaTime;
                float burningProgressNormalized = burningProgress / GetBurningRecipeSOFromInputSO(GetKitchenObject().GetKitchenObjectSO()).burningProgressMax;
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs { progressBarValueNormalized = burningProgressNormalized });

                if (burningProgress > burningRecipeSO.burningProgressMax)
                {
                    KitchenObjectSO outKitchenObjectSO = GetBurningOutputSOForInputSO(GetKitchenObject().GetKitchenObjectSO());
                    GetKitchenObject().DestroySelf();
                    KitchenObject.SpawnKitchenObject(outKitchenObjectSO, this);
                    state = State.Burned;
                    burningProgress = 0f;
                    OnStateChange?.Invoke(this, new OnStateChangeEventArgs { state = state });
                }
                break;
            case State.Burned:
                break;
        }
    }

    private KitchenObjectSO GetFryingOutputSOForInputSO(KitchenObjectSO kitchenObjectSO)
    {
        FryingRecipeSO cuttingRecipeSO = GetFryingRecipeSOFromInputSO(kitchenObjectSO);
        if (cuttingRecipeSO != null) return cuttingRecipeSO.output;
        else return null;
    }

    private bool HasFryingRecipeSOWithInputSO(KitchenObjectSO kitchenObjectSO)
    {
        return GetFryingRecipeSOFromInputSO(kitchenObjectSO) != null;
    }

    private FryingRecipeSO GetFryingRecipeSOFromInputSO(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (FryingRecipeSO f in fryingRecipeSOArray)
        {
            if (f.input == inputKitchenObjectSO)
            {
                return f;
            }
        }
        return null;
    }

    private KitchenObjectSO GetBurningOutputSOForInputSO(KitchenObjectSO inputKitchenObjectSO)
    {
        KitchenObjectSO outputKitchenObjectSO = GetBurningRecipeSOFromInputSO(inputKitchenObjectSO).output;
        if (outputKitchenObjectSO != null) return outputKitchenObjectSO;
        else return null;
    }

    private BurningRecipeSO GetBurningRecipeSOFromInputSO(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (BurningRecipeSO b in burningRecipeSOArray)
        {
            if (b.input == inputKitchenObjectSO)
            {
                return b;
            }
        }
        return null;
    }

}
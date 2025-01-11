using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgresschangeEventArgs> OnProgressChange;

    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public StoveState state;
    }
    

    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    private float timer;
    private float Timer
    {
        get { return timer; }
        set
        {
            timer = value;
            float progressNormalised = 0;
            switch(state)
            {
                case StoveState.Idle:
                    progressNormalised = 0f;
                break;
                case StoveState.Frying:
                    progressNormalised = timer/fryingRecipeSO.FryingTimerMax;
                break;
                case StoveState.Fried:
                    progressNormalised = timer/burningRecipeSO.BurningTimerMax;
                break;
                case StoveState.Burned:
                progressNormalised = 0;
                break;
            }          
            
            OnProgressChange?.Invoke(this, new IHasProgress.OnProgresschangeEventArgs
            {
                progressNormalized = progressNormalised
            });
        }
    }
    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;
    private StoveState state;
    private StoveState State
    {
        get { return state; }
        set 
        {
            state = value;
            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = this.state });
        }
    }

    private void Start()
    {
        State = StoveState.Idle;
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = StoveState.Idle });
    }

    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (State)
            {
                case StoveState.Idle:
                break;
                case StoveState.Frying:
                Timer += Time.deltaTime;
                GetKitchenObject().FryingProgress = Timer;
                if (Timer > fryingRecipeSO.FryingTimerMax)
                {
                    Timer = 0;
                    GetKitchenObject().DestroySelf();
                    KitchenObject newKitchenObject = KitchenObject.SpawnKitchenObject(fryingRecipeSO.Output, this);
                    burningRecipeSO = GetBurningRecipeSOWithInput(fryingRecipeSO.Output);
                    fryingRecipeSO = null;
                    State = StoveState.Fried;
                }
                break;
                case StoveState.Fried:
                Timer += Time.deltaTime;
                GetKitchenObject().BurningProgress = Timer;
                if (Timer > burningRecipeSO.BurningTimerMax)
                {
                    Timer = 0;
                    GetKitchenObject().DestroySelf();
                    KitchenObject newKitchenObject = KitchenObject.SpawnKitchenObject(burningRecipeSO.Output, this);
                    burningRecipeSO = null;
                    State = StoveState.Burned;
                }
                break;
                case StoveState.Burned:
                break;
            }
        }
    }
    public override void Interact(Player player)
    {
        if (HasKitchenObject())
        {
            //Counter has kitchenObject
            if (!player.HasKitchenObject())
            {
                //Player does not have kitchenObject
                GetKitchenObject().SetKitchenObjectParent(player);
                Timer = 0;
                State = StoveState.Idle;
            }
            else if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plate))
            {
                //Player has plate
                if (plate.TryAddingredient(GetKitchenObject().GetKitchenObjectSO()))
                {
                    GetKitchenObject().DestroySelf();
                    Timer = 0;
                    State = StoveState.Idle;
                }
            }
        }
        else
        {
            //Counter does not have kitchenObject
            if (player.HasKitchenObject())
            {
                //Player has kitchenObject
                KitchenObject kitchenObject = player.GetKitchenObject();
                if (CanBePlaced(kitchenObject.GetKitchenObjectSO()))
                {
                    kitchenObject.SetKitchenObjectParent(this);
                    if (CanBeFried(kitchenObject.GetKitchenObjectSO()))
                    {
                        Timer = kitchenObject.FryingProgress;
                        fryingRecipeSO = GetFryingRecipeSOWithInput(kitchenObject.GetKitchenObjectSO());
                        State = StoveState.Frying;
                    }
                    else if (CanBeBurned(kitchenObject.GetKitchenObjectSO()))
                    {
                        Timer = kitchenObject.BurningProgress;
                        burningRecipeSO = GetBurningRecipeSOWithInput(kitchenObject.GetKitchenObjectSO());
                        State = StoveState.Fried;
                    }
                    else
                    {
                        State = StoveState.Burned;
                    }
                }
            }
        }
    }

    private bool CanBePlaced(KitchenObjectSO kitchenObjectSO)
    {
        return fryingRecipeSOArray.Any(f => f.Input == kitchenObjectSO || f.Output == kitchenObjectSO) || 
            burningRecipeSOArray.Any(b => b.Output == kitchenObjectSO);
    }

    private bool CanBeFried(KitchenObjectSO kitchenObjectSO)
    {
        return fryingRecipeSOArray.Any(f => f.Input == kitchenObjectSO);
    }

    private bool CanBeBurned(KitchenObjectSO kitchenObjectSO)
    {
        return burningRecipeSOArray.Any(b => b.Input == kitchenObjectSO);
    }

    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO input)
    {
        return fryingRecipeSOArray.Where(f => f.Input == input).FirstOrDefault();
    }

    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO input)
    {
        return burningRecipeSOArray.Where(b => b.Input == input).FirstOrDefault();
    }
}

public enum StoveState
{
    Idle,
    Frying,
    Fried,
    Burned
}

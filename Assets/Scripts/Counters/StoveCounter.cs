using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class StoveCounter : BaseCounter
{
    private enum State
    {
        Idle,
        Frying,
        Fried,
        Burned
    }

    [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
    [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

    private float fryingTimer;
    private float burningTimer;
    private FryingRecipeSO fryingRecipeSO;
    private BurningRecipeSO burningRecipeSO;
    private State state;

    private void Start()
    {
        state = State.Idle;
    }

    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (state)
            {
                case State.Idle:
                break;
                case State.Frying:
                fryingTimer += Time.deltaTime;
                GetKitchenObject().FryingProgress = fryingTimer;
                if (fryingTimer > fryingRecipeSO.FryingTimerMax)
                {
                    fryingTimer = 0;
                    GetKitchenObject().DestroySelf();
                    KitchenObject newKitchenObject = KitchenObject.SpawnKitchenObject(fryingRecipeSO.Output, this);
                    burningRecipeSO = GetBurningRecipeSOWithInput(fryingRecipeSO.Output);
                    fryingRecipeSO = null;
                    state = State.Fried;
                    burningTimer = 0;
                }
                break;
                case State.Fried:
                burningTimer += Time.deltaTime;
                GetKitchenObject().BurningProgress = burningTimer;
                if (burningTimer > burningRecipeSO.BurningTimerMax)
                {
                    burningTimer = 0;
                    GetKitchenObject().DestroySelf();
                    KitchenObject newKitchenObject = KitchenObject.SpawnKitchenObject(burningRecipeSO.Output, this);
                    burningRecipeSO = null;
                    state = State.Burned;
                }
                break;
                case State.Burned:
                break;
            }
        }
    }
    public override void Interact(Player player)
    {
        if (HasKitchenObject())
        {
            if (!player.HasKitchenObject())
            {
                GetKitchenObject().SetKitchenObjectParent(player);
                fryingTimer = 0;
                burningTimer = 0;
                state = State.Idle;
            }
        }
        else
        {
            if (player.HasKitchenObject())
            {
                KitchenObject kitchenObject = player.GetKitchenObject();
                if (CanBePlaced(kitchenObject.GetKitchenObjectSO()))
                {
                    kitchenObject.SetKitchenObjectParent(this);
                    if (CanBeFried(kitchenObject.GetKitchenObjectSO()))
                    {
                        fryingTimer = kitchenObject.FryingProgress;
                        fryingRecipeSO = GetFryingRecipeSOWithInput(kitchenObject.GetKitchenObjectSO());
                        state = State.Frying;
                    }
                    else if (CanBeBurned(kitchenObject.GetKitchenObjectSO()))
                    {
                        burningTimer = kitchenObject.BurningProgress;
                        burningRecipeSO = GetBurningRecipeSOWithInput(kitchenObject.GetKitchenObjectSO());
                        state = State.Fried;
                    }
                    else
                    {
                        state = State.Burned;
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

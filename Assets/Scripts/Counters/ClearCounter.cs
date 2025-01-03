using UnityEngine;

public class ClearCounter : BaseCounter
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            //TO DO: If player has KO move KO to counter
        }
        else
        {
            //TO DO: If player doesn't have KO, give KO to player
        }
    }
}

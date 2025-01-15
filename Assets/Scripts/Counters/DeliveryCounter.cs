public class DeliveryCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plate))
            {
                DeliveryManager.Instance.DeliverRecipe(plate);
                plate.DestroySelf();
            }
        }
    }
}

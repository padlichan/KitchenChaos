using System;

public class TrashCounter : BaseCounter
{
    public static event EventHandler OnAnyItemTrashed;
    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            OnAnyItemTrashed?.Invoke(this, EventArgs.Empty);
            player.GetKitchenObject().DestroySelf();
        }
    }
    new public static void ResetStaticData()
    {
        OnAnyItemTrashed = null;
    }
}

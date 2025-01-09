using UnityEngine;

[CreateAssetMenu(fileName = "BurningRecipeSO", menuName = "Scriptable Objects/BurningRecipeSO")]
public class BurningRecipeSO : ScriptableObject
{
    public KitchenObjectSO Input;
    public KitchenObjectSO Output;
    public float BurningTimerMax;
}

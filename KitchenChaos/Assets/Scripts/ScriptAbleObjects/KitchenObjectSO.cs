using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class KitchenObjectSO : ScriptableObject
{
    public Transform itemPrefab;
    public string itemName;
    public Sprite itemIcon;
}

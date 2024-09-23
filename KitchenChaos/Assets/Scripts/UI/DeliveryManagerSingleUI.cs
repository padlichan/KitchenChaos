using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryManagerSingleUI : MonoBehaviour
{
    [SerializeField] private TMP_Text orderName;
    [SerializeField] private Transform icons;
    [SerializeField] private Transform iconTemplate;


    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }
    public void SetOrderUISingleVisual(RecipeSO order)
    {
        orderName.text = order.recipeName;

        //Cleaning up all icons in UI element
        foreach (Transform child in icons)
        {
            if (child == iconTemplate) continue;
            Destroy(child.gameObject);
        }
        //Adding in correct icons
        foreach (KitchenObjectSO k in order.kitchenObjectSOList)
        {
            Transform newIcon = Instantiate(iconTemplate, icons);
            newIcon.gameObject.GetComponent<Image>().sprite = k.itemIcon;
            newIcon.gameObject.SetActive(true);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Transform orderTemplate;

    private void Awake()
    {
        orderTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        DeliveryManager.Instance.OnOrderSpawned += DeliveryManger_OnOrderListChange;
        DeliveryManager.Instance.OnOrderDelivered += DeliveryManger_OnOrderListChange;

        UpdateVisual();
    }


    private void DeliveryManger_OnOrderListChange(object sender, EventArgs e)
    {
        UpdateVisual();
    }
    private void UpdateVisual()
    {
        foreach (Transform child in container)
        {
            if (child == orderTemplate) continue;
            Destroy(child.gameObject);
        }


        List<RecipeSO> orderList = DeliveryManager.Instance.GetOrderList();
        foreach (RecipeSO order in orderList)
        {
            Transform orderTransform = Instantiate(orderTemplate, container);
            orderTransform.gameObject.SetActive(true);
            orderTransform.GetComponent<DeliveryManagerSingleUI>().SetOrderUISingleVisual(order);
            //Display order on UI
        }

    }

}

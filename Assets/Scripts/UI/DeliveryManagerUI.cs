using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Transform orderTemplate;

    private void Awake()
    {
        orderTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        DeliveryManager.Instance.OnOrderDelivered += DeliveryManager_OnOrderDelivered;
        DeliveryManager.Instance.OnOrderSpawned += DeliveryManager_OnOrderSpawned;
        UpdateVisual();
    }

    private void DeliveryManager_OnOrderSpawned(object sender, System.EventArgs e)
    {
        UpdateVisual();
    }

    private void DeliveryManager_OnOrderDelivered(object sender, System.EventArgs e)
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

        foreach (var order in DeliveryManager.Instance.GetPendingOrderList())
        {
            Transform orderTransform = Instantiate(orderTemplate, container);
            orderTransform.gameObject.SetActive(true);
            orderTransform.GetComponent<DeliveryManagerSingleUI>().SetRecipeSO(order);
        }
    }
}

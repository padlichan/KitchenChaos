using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCounterVisual : MonoBehaviour
{
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform plateVisualPrefab;
    [SerializeField] private PlateCounter plateCounter;
    private Transform plateVisualTransform;
    private List<GameObject> plateVisualGameObjectList = new List<GameObject>();

    private void Start()
    {
        plateCounter.OnPlateSpawned += SpawnPlateVisual;
        plateCounter.OnPlateRemoved += RemovePlateVisual;
    }

    private void SpawnPlateVisual(object sender, EventArgs e)
    {
        plateVisualTransform = Instantiate(plateVisualPrefab, counterTopPoint);
        float plateOffsetY = 0.1f;
        plateVisualTransform.localPosition = new Vector3(0, plateOffsetY * plateVisualGameObjectList.Count, 0);
        plateVisualGameObjectList.Add(plateVisualTransform.gameObject);
    }

    private void RemovePlateVisual(object sender, EventArgs e)
    {
        GameObject plateGameObject = plateVisualGameObjectList[plateVisualGameObjectList.Count - 1];
        plateVisualGameObjectList.Remove(plateGameObject);
        Destroy(plateGameObject);
    }
}

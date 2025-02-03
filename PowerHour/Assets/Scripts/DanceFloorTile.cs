using System.Collections.Generic;
using UnityEngine;

public class DanceFloorTile : MonoBehaviour
{
    [SerializeField] private List<Color> possibleColors;
    [SerializeField] private Light lightSource;
    [SerializeField] private Transform tileTransform;

    private float currentBPM = 122f;

    private Material materialInstance;
    private List<Color> availableColors;
    private float timer;

    private void Start()
    {
        materialInstance = tileTransform.GetComponent<Renderer>().material;
        tileTransform.GetComponent<Renderer>().material = materialInstance;

        availableColors = new List<Color>(possibleColors);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer >= 60f / currentBPM)
        {
            timer = 0f;
            ChangeColor();
        }
    }

    private void ChangeColor()
    {
        Color newColor = availableColors[Random.Range(0, availableColors.Count)];

        availableColors = new List<Color>(possibleColors);
        availableColors.Remove(newColor);

        materialInstance.SetColor("_BaseColor", newColor);
        materialInstance.SetColor("_EmissionColor", newColor * 2f);

        lightSource.color = newColor;
    }
}

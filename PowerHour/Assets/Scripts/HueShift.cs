using UnityEngine;

public class HueShift : MonoBehaviour
{
    private Light lightSource;

    [SerializeField] private float cycleTime = 10f;
    [SerializeField] private Material syncMaterial;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lightSource = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        lightSource.color = Color.HSVToRGB(Mathf.PingPong(Time.time / cycleTime, 1), 1, 1);

        if(syncMaterial != null)
        {
            syncMaterial.SetColor("_BaseColor", lightSource.color);
            syncMaterial.SetColor("_EmissionColor", lightSource.color * 2f);
        }
    }
}

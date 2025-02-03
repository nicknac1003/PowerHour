using UnityEngine;
using System.Collections;

public class LightFlicker : MonoBehaviour
{
    [SerializeField] private float minFlickerTime = 0.05f;
    [SerializeField] private float maxFlickerTime = 0.2f;
    [SerializeField] private float minBurstInterval = 1f;
    [SerializeField] private float maxBurstInterval = 3f;
    [SerializeField] private int minBurstCount = 3;
    [SerializeField] private int maxBurstCount = 6;

    private Light lightSource;

    private void Start()
    {
        lightSource = GetComponent<Light>();
        
        StartCoroutine(FlickerLight());
    }

    private IEnumerator FlickerLight()
    {
        while (true)
        {
            int burstCount = Random.Range(minBurstCount, maxBurstCount);
            for (int i = 0; i < burstCount; i++)
            {
                lightSource.enabled = !lightSource.enabled;
                yield return new WaitForSeconds(Random.Range(minFlickerTime, maxFlickerTime));
            }
            lightSource.enabled = true; // Ensure the light is on after the burst
            yield return new WaitForSeconds(Random.Range(minBurstInterval, maxBurstInterval));
        }
    }
}

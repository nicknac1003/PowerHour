using UnityEngine;

public class PlayerPowerupManager : MonoBehaviour
{
    [SerializeField] private bool hasAppletini; // Backing field
    [SerializeField] private Transform rightArm;


    private void OnValidate()
    {
        // This triggers when inspector values change during gameplay
        if (Application.isPlaying)
        {
            if (hasAppletini)
            {
                AppletiniPowerup();
            }
        }
    }


    void AppletiniPowerup()
    {
        rightArm.localScale = new Vector3(1.5f, 2.0f, 1.5f);
    }


}

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
            AppletiniPowerup();
        }
    }


    void AppletiniPowerup()
    {
        if (hasAppletini)
        {
            rightArm.localScale = new Vector3(1.5f, 2.0f, 1.5f);
        }
        else
        {
            rightArm.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
    }


}

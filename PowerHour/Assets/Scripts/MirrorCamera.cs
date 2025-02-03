using UnityEngine;

public class MirrorCamera : MonoBehaviour
{
    [SerializeField] private Transform mirror;

    // Update is called once per frame
    void Update()
    {
        // Get main camera position
        Vector3 mainCameraPosition = Camera.main.transform.position;
        transform.position = new Vector3(mainCameraPosition.x, mainCameraPosition.y, 2 * mirror.position.z - mainCameraPosition.z);
    }
}

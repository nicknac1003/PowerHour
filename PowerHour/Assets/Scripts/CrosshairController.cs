using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    private RectTransform crosshairRectTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        crosshairRectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get the mouse position in screen coordinates
        Vector2 mousePosition = Input.mousePosition;

        // Set the crosshair position to the mouse position
        crosshairRectTransform.position = mousePosition;
    }
}

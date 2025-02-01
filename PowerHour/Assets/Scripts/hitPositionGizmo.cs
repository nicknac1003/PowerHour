using UnityEngine;

public class hitPositionGizmo : MonoBehaviour
{
    public float radius = 1.5f;
    public Color color = Color.red;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, radius);
    }
}

using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DrawLine : MonoBehaviour
{
    public Transform startPoint;  // Assign your starting object's transform here
    public Transform endPoint;    // Assign your ending object's transform here

    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (startPoint != null && endPoint != null)
        {
            lineRenderer.SetPosition(0, startPoint.position);
            lineRenderer.SetPosition(1, endPoint.position);
        }
    }
}

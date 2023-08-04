using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    public LineRenderer lineRenderer;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lineRenderer.positionCount = 1;
            lineRenderer.SetPosition(0, Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, newPosition);
        }
    }
}

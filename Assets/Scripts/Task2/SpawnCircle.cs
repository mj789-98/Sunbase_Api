using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SpawnCircle : MonoBehaviour
{
    public GameObject circlePrefab;
    public int minCircles = 5;
    public int maxCircles = 10;
    public float minDistanceBetweenCircles = 1.5f;

    public List<GameObject> circles = new List<GameObject>();
    public List<GameObject> intersectedCircles = new List<GameObject>();

    private void Start()
    {
        SpawnCircles();
    }

    public void Restart()
    {
        SceneManager.LoadScene(2);
    }
    private void SpawnCircles()
    {
        int circleCount = Random.Range(minCircles, maxCircles + 1);

        for (int i = 0; i < circleCount; i++)
        {
            SpawnCircleAtRandomPosition();
        }
    }

    private void SpawnCircleAtRandomPosition()
    {
        Vector3 spawnPosition;
        bool validPosition;

        do
        {
            validPosition = true;
            spawnPosition = new Vector3(Random.Range(-8f, 8f), Random.Range(-4.5f, 4.5f), 0f);

            foreach (GameObject circle in circles)
            {
                if (Vector3.Distance(spawnPosition, circle.transform.position) < minDistanceBetweenCircles)
                {
                    validPosition = false;
                    break;
                }
            }
        } while (!validPosition);

        GameObject newCircle = Instantiate(circlePrefab, spawnPosition, Quaternion.identity);
        circles.Add(newCircle);
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            CheckLineIntersection();
        }
        if(Input.GetMouseButtonUp(0))
        {
            DestroyIntersectedCircles();
        }
    }

    private void CheckLineIntersection()
    {
        LineRenderer lineRenderer = FindObjectOfType<LineRenderer>();
        Vector3[] linePositions = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(linePositions);

        intersectedCircles.Clear();

        foreach (GameObject circle in circles)
        {
            CircleCollider2D circleCollider = circle.GetComponent<CircleCollider2D>();
            if (circleCollider != null && IsLineIntersectingCircle(linePositions, circleCollider))
            {
                intersectedCircles.Add(circle);
            }
        }

        //DestroyIntersectedCircles();
    }

    private void DestroyIntersectedCircles()
    {
        foreach (GameObject circle in intersectedCircles)
        {
            circles.Remove(circle);
            Destroy(circle);
        }
    }

    private bool IsLineIntersectingCircle(Vector3[] linePositions, CircleCollider2D circleCollider)
    {
        Vector2 circleCenter = circleCollider.transform.position;
        float circleRadius = circleCollider.radius;

        for (int i = 0; i < linePositions.Length - 1; i++)
        {
            Vector2 startPoint = linePositions[i];
            Vector2 endPoint = linePositions[i + 1];

            if (IsIntersectingCircleLine(circleCenter, circleRadius, startPoint, endPoint))
            {
                return true;
            }
        }

        return false;
    }

    private bool IsIntersectingCircleLine(Vector2 circleCenter, float circleRadius, Vector2 lineStart, Vector2 lineEnd)
    {
        Vector2 circleToLineStart = lineStart - circleCenter;
        Vector2 circleToLineEnd = lineEnd - circleCenter;

        return Vector2.Dot(circleToLineStart, circleToLineStart) < circleRadius * circleRadius !=
               Vector2.Dot(circleToLineEnd, circleToLineEnd) < circleRadius * circleRadius;
    }
}

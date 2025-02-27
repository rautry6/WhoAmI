using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(CanvasRenderer))]
public class UICurvedLine : MaskableGraphic
{
    [Header("Curve Settings")]
    public Vector2 startPoint;   // The beginning of the line (in local canvas space)
    public Vector2 endPoint;     // The end of the line (in local canvas space)
    public Vector2 controlPoint; // Determines the curvature (a good default is the midpoint plus an offset)

    [Header("Appearance")]
    public float thickness = 5f; // Line thickness
    public int segmentCount = 20; // How many segments to split the curve into (more = smoother)

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        // Generate points along a quadratic Bézier curve
        List<Vector2> curvePoints = new List<Vector2>();
        for (int i = 0; i <= segmentCount; i++)
        {
            float t = i / (float)segmentCount;
            Vector2 point = CalculateQuadraticBezierPoint(t, startPoint, controlPoint, endPoint);
            curvePoints.Add(point);
        }

        // For each segment, create a quad so the line has thickness
        for (int i = 0; i < curvePoints.Count - 1; i++)
        {
            Vector2 p1 = curvePoints[i];
            Vector2 p2 = curvePoints[i + 1];
            Vector2 direction = (p2 - p1).normalized;
            Vector2 normal = new Vector2(-direction.y, direction.x);
            Vector2 offset = normal * (thickness / 2f);

            // Create four vertices for a quad
            UIVertex v0 = UIVertex.simpleVert;
            v0.color = color;
            v0.position = p1 - offset;
            UIVertex v1 = UIVertex.simpleVert;
            v1.color = color;
            v1.position = p1 + offset;
            UIVertex v2 = UIVertex.simpleVert;
            v2.color = color;
            v2.position = p2 - offset;
            UIVertex v3 = UIVertex.simpleVert;
            v3.color = color;
            v3.position = p2 + offset;

            int startIndex = vh.currentVertCount;
            vh.AddVert(v0);
            vh.AddVert(v1);
            vh.AddVert(v2);
            vh.AddVert(v3);

            // Create two triangles for this quad
            vh.AddTriangle(startIndex, startIndex + 1, startIndex + 2);
            vh.AddTriangle(startIndex + 2, startIndex + 1, startIndex + 3);
        }
    }

    // Computes a point on a quadratic Bézier curve.
    private Vector2 CalculateQuadraticBezierPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2)
    {
        float u = 1 - t;
        return (u * u * p0) + (2 * u * t * p1) + (t * t * p2);
    }
}

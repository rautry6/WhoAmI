using System.Net;
using UnityEngine;
using UnityEngine.UI;

public class UICurvedLineController : MonoBehaviour
{
    [Header("References")]
    public UICurvedLine uiLine;      // Your custom UI curved line component
    public RectTransform startUI;    // The UI element (child) from which the line should start
    public Canvas canvas;            // The Canvas (Screen Space – Overlay)

    [Header("Control Settings")]
    public Vector2 controlOffset = new Vector2(0, 50f); // Offset for the curve's midpoint

    // We'll use the UI line's own RectTransform as the conversion reference.
    private RectTransform lineRect;
    public bool connectionMade = false;

    private void Awake()
    {
        if (canvas == null)
            canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        // Get the RectTransform of the UI curved line (this component should be on the same GameObject)
        lineRect = uiLine.GetComponent<RectTransform>();
    }

    void Update()
    {
        if (startUI == null)
            return;

        Vector2 startLocalPos;
        Vector3 worldPos = startUI.transform.TransformPoint(Vector3.zero);  // Get correct world pos
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            lineRect,
            worldPos,
            null,
            out startLocalPos);
        uiLine.startPoint = startLocalPos;

        Vector2 end;

        if (!connectionMade)
        {
            // Convert the mouse position (screen space) into the local space of the UI line.
            Vector2 mouseLocalPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                lineRect,
                Input.mousePosition,
                null,
                out mouseLocalPos);

            end = mouseLocalPos;
            uiLine.endPoint = mouseLocalPos;
        }
        else
        {
            Vector3 worldPosi = uiLine.EndPointTransform.TransformPoint(Vector3.zero);  // Get correct world pos
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                lineRect,
                worldPosi,
                null,
                out end);
            uiLine.endPoint = end;
        }

        // Set the control point as the midpoint plus an offset.
        uiLine.controlPoint = (startLocalPos + end) * 0.5f + controlOffset;

        // Force the curved line to update its mesh.
        uiLine.SetVerticesDirty();
    }
}

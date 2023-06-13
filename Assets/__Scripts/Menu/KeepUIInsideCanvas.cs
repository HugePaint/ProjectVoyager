using UnityEngine;
using UnityEngine.UI;

public class KeepUIInsideCanvas : MonoBehaviour
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector3[] canvasCorners;
    private Vector3[] rectCorners;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasCorners = new Vector3[4];
        rectCorners = new Vector3[4];
    }

    public void KeepInsideCanvas()
    {
        if (rectTransform == null || canvas == null)
            return;

        RectTransform canvasRectTransform = canvas.GetComponent<RectTransform>();
        canvasRectTransform.GetWorldCorners(canvasCorners);
        rectTransform.GetWorldCorners(rectCorners);

        Vector3 minCanvas = canvasCorners[0];
        Vector3 maxCanvas = canvasCorners[2];

        Vector3 minRect = rectCorners[0];
        Vector3 maxRect = rectCorners[2];

        float rectWidth = maxRect.x - minRect.x;
        float rectHeight = maxRect.y - minRect.y;

        float clampedX = Mathf.Clamp(rectTransform.position.x, minCanvas.x + rectWidth * rectTransform.pivot.x, maxCanvas.x - rectWidth * (1 - rectTransform.pivot.x));
        float clampedY = Mathf.Clamp(rectTransform.position.y, minCanvas.y + rectHeight * rectTransform.pivot.y, maxCanvas.y - rectHeight * (1 - rectTransform.pivot.y));

        rectTransform.position = new Vector3(clampedX, clampedY, rectTransform.position.z);
    }
}
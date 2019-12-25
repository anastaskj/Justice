using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Camera cam;
    public float maxZoomOut;
    public float maxZoomIn;

    public float moveSpeed;

    void Update()
    {
        float zoomDelta = Input.GetAxis("Mouse ScrollWheel");
        if (zoomDelta != 0f)
        {
            AdjustZoom(zoomDelta);
        }
        float xDelta = Input.GetAxis("Horizontal");
        float yDelta = Input.GetAxis("Vertical");
        if (xDelta != 0f || yDelta != 0f)
        {
            AdjustPosition(xDelta, yDelta);
        }
    }

    void AdjustZoom(float delta)
    {
        if (cam.orthographicSize + delta < maxZoomOut && cam.orthographicSize + delta > maxZoomIn)
        {
            cam.orthographicSize += delta;
        }
    }

    void AdjustPosition(float xDelta, float yDelta)
    {
        Vector3 direction = new Vector3(xDelta, yDelta, 0f).normalized;
        float damping = Mathf.Max(Mathf.Abs(xDelta), Mathf.Abs(yDelta));
        float distance = moveSpeed * damping * Time.deltaTime;
        Vector3 position = transform.localPosition;
        position += direction * distance;
        transform.localPosition = position;
    }

}

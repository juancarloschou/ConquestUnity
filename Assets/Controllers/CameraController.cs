using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class CameraController : MonoBehaviour
{
    //panSpeed: Controls the speed of the camera movement.
    public float panSpeed = 20f;
    //panBorderThickness: Determines the distance from the screen borders where the camera starts moving.
    public float panBorderThickness = 20f;
    //panLimit: Limits the camera's movement along the x and z axes.
    public Vector2 panLimit = new Vector2(7f, 13.2f);
    //scrollSpeed: Controls the speed of the zoom.
    public float zoomSpeed = 2f;
    //minZoom and maxZoom: Set the minimum and maximum zoom of the camera.
    public float minZoom = 5f;
    public float maxZoom = 20f;

    // Update is called once per frame
    void Update()
    {
        HandlePan();
        HandleZoom();
    }

    // put mouse in borders of screen to do movement (for mouse, not tactile device)
    void HandlePan()
    {
        Vector3 pos = transform.position;
        //Debug.Log($"current pos: {pos}");

        if (Input.GetKey(KeyCode.UpArrow) || Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            pos.y += panSpeed * Time.deltaTime;
            //Debug.Log($"up: {pos}");
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.mousePosition.y <= panBorderThickness)
        {
            pos.y -= panSpeed * Time.deltaTime;
            //Debug.Log($"down: {pos}");
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.mousePosition.x <= panBorderThickness)
        {
            pos.x -= panSpeed * Time.deltaTime;
            //Debug.Log($"left: {pos}");
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            pos.x += panSpeed * Time.deltaTime;
            //Debug.Log($"right: {pos}");
        }

        // Calculate pan limits based on the current orthographic size
        float currentOrthoSize = Camera.main.orthographicSize;
        float adaptedPanLimitX = panLimit.x + (currentOrthoSize - minZoom);
        float adaptedPanLimitY = panLimit.y + (currentOrthoSize - minZoom);

        // Clamp the camera position to adapted pan limits
        pos.x = Mathf.Clamp(pos.x, -adaptedPanLimitX, adaptedPanLimitX);
        pos.y = Mathf.Clamp(pos.y, -adaptedPanLimitY, adaptedPanLimitY);

        // Handle touch-based camera movement
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            Debug.Log($"touch: {touch}");

            if (touch.phase == TouchPhase.Moved)
            {
                pos.x -= touch.deltaPosition.x * panSpeed * Time.deltaTime;
                pos.y -= touch.deltaPosition.y * panSpeed * Time.deltaTime;
            }
        }

        //Debug.Log($"new pos: {pos}");
        transform.position = pos;
    }

    // zoom with mouse scroll wheel (for mouse, not tactile device)
    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        // Check if scroll input is not zero before adjusting zoom
        if (scroll != 0)
        {
            Camera mainCamera = Camera.main; // Assuming this script is attached to the main camera

            // Calculate the new orthographic size
            float newOrthoSize = mainCamera.orthographicSize - scroll * zoomSpeed * Time.deltaTime * 1000f;

            // Clamp the orthographic size to minY and maxY values
            newOrthoSize = Mathf.Clamp(newOrthoSize, minZoom, maxZoom);

            // Apply the new orthographic size to the camera
            //Debug.Log($"zoom scroll: {scroll}, newOrthoSize: {newOrthoSize}");
            mainCamera.orthographicSize = newOrthoSize;
        }
    }
}


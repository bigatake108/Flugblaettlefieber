using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnifyGlass : MonoBehaviour
{
    private Camera magnifyCamera;
    //private GameObject magnifyBorders;
    private float magnifyOriginX, magnifyOriginY; // Magnify Glass Origin X and Y position
    private float magnifyWidth = Screen.width / 5f, magnifyHeight = Screen.width / 5f; // Magnify glass width and height
    private Vector3 mousePos;

    

    void Start()
    {
        createMagnifyGlass();
    }
    void Update()
    {
        // Following lines set the camera's pixelRect and camera position at mouse position
        magnifyCamera.pixelRect = new Rect(Input.mousePosition.x - magnifyWidth / 2.0f, Input.mousePosition.y - magnifyHeight / 2.0f, magnifyWidth, magnifyHeight);
        mousePos = getWorldPosition(Input.mousePosition);
        magnifyCamera.transform.position = mousePos;
        mousePos.z = 0;
        //magnifyBorders.transform.position = mousePos;
    }

    // Following method creates MagnifyGlass
    private void createMagnifyGlass()
    {
        GameObject camera = new GameObject("MagnifyCamera");
        magnifyOriginX = Screen.width / 2f - magnifyWidth / 2f;
        magnifyOriginY = Screen.height / 2f - magnifyHeight / 2f;
        magnifyCamera = camera.AddComponent<Camera>();
        magnifyCamera.pixelRect = new Rect(magnifyOriginX, magnifyOriginY, magnifyWidth, magnifyHeight);
        magnifyCamera.transform.position = new Vector3(0, 0, 0);
        if (Camera.main.orthographic)
        {
            magnifyCamera.orthographic = true;
            magnifyCamera.orthographicSize = Camera.main.orthographicSize / 5.0f;//+ 1.0f;
        }
        else
        {
            magnifyCamera.orthographic = false;
            magnifyCamera.fieldOfView = Camera.main.fieldOfView / 10.0f;//3.0f;
        }

    }

    // Following method calculates world's point from screen point as per camera's projection type
    public Vector3 getWorldPosition(Vector3 screenPos)
    {
        Vector3 worldPos;
        if (Camera.main.orthographic)
        {
            worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            worldPos.z = Camera.main.transform.position.z;
        }
        else
        {
            worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, Camera.main.transform.position.z));
            worldPos.x *= -1;
            worldPos.y *= -1;
        }
        return worldPos;
    }
}

using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float startPanSpeed;
    float panSpeed; // this will change with zooming
    public float panBorderThickness = 10f; // how close we need to get to edge of screen to move camera
    public Vector2 panLimit;
    // Input.mousePosition is (0,0) at the bottom left of the screen
    public float minOrthographicSize;
    public float maxOrthographicSize;
    public float scrollSpeed;

    private void Start()
    {
        startPanSpeed = 12f;
        panSpeed = startPanSpeed;

        minOrthographicSize = 4f;
        maxOrthographicSize = 10f;
        scrollSpeed = 7f;

        // Getting the size of the map, because the camera should not go beyond it
        GameObject map = GameObject.FindWithTag("Ground");
        if(map != null)
        {
            Renderer planeRenderer = map.GetComponent<Renderer>(); 
            if(planeRenderer != null)
            {
                Bounds bounds = planeRenderer.bounds;

                Vector2 mapExtents = bounds.extents; // half of the size of the map
                panLimit = mapExtents;
            } else {
                Debug.Log("Error: the plane (map) doesn't have a renderer component that indicates its size");
            }
        } else {
            Debug.Log("Error: No object with tag Ground (the map) was found to initialize camera bounds");
        }
    }

    void LateUpdate()
    {
        Vector3 cameraPos = transform.position;

        // Scrolling in and out in 2D uses ortographic size
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        float newOrthographicSize = Camera.main.orthographicSize - scrollInput * scrollSpeed;
        // Scale panSpeed with how zoomed out you are. To avoid the screen panning slow when zoomed out.
        panSpeed = startPanSpeed + startPanSpeed * Camera.main.orthographicSize / minOrthographicSize;

        if (Input.GetKey("w") || Input.mousePosition.y > Screen.height - panBorderThickness)
        {
            cameraPos.y += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("s") || Input.mousePosition.y < panBorderThickness)
        {
            cameraPos.y -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("a") || Input.mousePosition.x < panBorderThickness)
        {
            cameraPos.x -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d") || Input.mousePosition.x > Screen.width - panBorderThickness)
        {
            cameraPos.x += panSpeed * Time.deltaTime;
        }

        // To prevent the user from going outside the map
        cameraPos.x = Mathf.Clamp(cameraPos.x, -panLimit.x, panLimit.x);
        cameraPos.y = Mathf.Clamp(cameraPos.y, -panLimit.y, panLimit.y);

        //Debug.Log("cameraPos.y: " + cameraPos.y + ", panLimit.y: " + panLimit.y);

        transform.position = cameraPos;


        newOrthographicSize = Mathf.Clamp(newOrthographicSize, minOrthographicSize, maxOrthographicSize);

        Camera.main.orthographicSize = newOrthographicSize;
    }
}

using UnityEngine;

/// <summary>
/// Used in a test scene. 
/// 
/// Please duplicate this instead of writing in it.
/// 
/// Has basic functionality:
/// 
///     - Camera Panning
///     - Left mouse click
/// 
/// </summary>

public class NewClickTest : MonoBehaviour
{
    //Cache
    private Vector2 axis;
    private Camera cam;
    private Ray mouseRay;


    public float cameraSpeed = 5.0f;

    private const float rayDistance = 500.0f;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        MouseClick();
    }

    void MouseClick()
    {
        mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        Debug.DrawLine(mouseRay.origin, mouseRay.direction * 500.0f, Color.red);

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            RaycastHit2D hit2D = Physics2D.Raycast(mouseRay.origin, Camera.main.transform.forward, rayDistance);
            if (hit2D)
            {
                Debug.Log($"{hit2D.point} { hit2D.transform.name}"); // Example
            }
            else if (Physics.Raycast(mouseRay, out RaycastHit hit, rayDistance))
            {
                var gameObject = hit.collider.gameObject;

                if (gameObject.CompareTag("Enemy")) // Example 
                {
                    gameObject.GetComponent<TestEnemy>().Click();
                }
                else
                {
                    Debug.Log($"{hit.point}" + gameObject.name); // Example
                }
            }
        }
    }

    private void LateUpdate()
    {
        axis.x = Input.GetAxis("Horizontal");
        axis.y = Input.GetAxis("Vertical");

        cam.transform.Translate(axis * cameraSpeed * Time.deltaTime);
    }
}

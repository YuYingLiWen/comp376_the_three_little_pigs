using UnityEngine;
using System.Collections;
using UnityEngine.Timeline;
using System.Runtime.CompilerServices;

public class MouseControl : MonoBehaviour
{
    //[SerializeField]
    //float mouseSensitivity;

    [SerializeField]
    Marker markerPrefab;

    private GameObject selectedObject;
    private Color originalColor;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // left click (to select object)
        {
            // Send ray
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // If ray hit something
            if (Physics.Raycast(ray, out hit))
            {
                // Deselect any previously selected object
                if (selectedObject != null)
                {
                    DeselectObject();
                }

                // Check if the object is a player (pig)
                PlayerUnit pig = hit.collider.GetComponent<PlayerUnit>();

                if (pig != null)
                {
                    selectedObject = hit.collider.gameObject;

                    // Store the original color (if the object has a renderer)
                    Renderer renderer = selectedObject.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        originalColor = renderer.material.color;
                    }

                    // Change the object's color to indicate selection
                    ChangeObjectColor(selectedObject, Color.red);
                }
            }
        }
        else if (Input.GetMouseButtonDown(1)) // right click (to do actions with selected object)
        {
            // Send ray
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // If ray hit something & and we have a selected object
            if (Physics.Raycast(ray, out hit) && selectedObject != null)
            {
                PlayerUnit selectedPig = selectedObject.GetComponent<PlayerUnit>();
                
                if (selectedPig != null) // if selected object is a pig
                {
                    if (hit.collider.tag == "Ground") // move
                    {
                        selectedPig.SetDestination(hit.point);
                        Instantiate(markerPrefab, hit.point + new Vector3(0.0f, 0.1f, 0.0f), markerPrefab.transform.rotation);
                    } else if (hit.collider.tag == "Tree")// else if tag is tree then go fetch tree
                    {
                        CollectTree(selectedPig, hit);
                    }
                    
                    // else if tag is tower then man tower...
                }

                // if the selected object is tower, do tower related stuff etc.
            }
        }
    }

    // Pig goes get the tree and brings back wood to the house
    private void CollectTree(PlayerUnit selectedPig, RaycastHit hit)
    {
        GameObject tree = hit.collider.gameObject;
        if (tree != null)
        {
            // tree will now be stored in the PlayerUnit object and used in OnCollisionEnter to collect
            selectedPig.targetTree = tree;
            selectedPig.SetDestination(tree.transform.position);
        }
    }

    // Deselect the selected object
    private void DeselectObject()
    {
        if (selectedObject != null)
        {
            // Restore the original look
            Renderer renderer = selectedObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = originalColor;
            }

            selectedObject = null;
        }
    }

    // Change the color of a selected object
    private void ChangeObjectColor(GameObject obj, Color color)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = color;
        }
    }

}

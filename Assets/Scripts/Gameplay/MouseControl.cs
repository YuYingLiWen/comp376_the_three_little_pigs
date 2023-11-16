using UnityEngine;
using System.Collections;

public class MouseControl : MonoBehaviour
{
    //[SerializeField]
    //float mouseSensitivity;

    [SerializeField]
    Marker markerPrefab;

    private GameObject selectedObject;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // left click (to select object)
        {
            // Send ray
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            Debug.DrawLine(ray.origin, ray.direction * 1000.0f, Color.red, 10.0f);
            
            // If ray hit something
            if (Physics.Raycast(ray, out hit, 500.0f, LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore))
            {
                // Deselect any previously selected object
                if (selectedObject != null) // We have a selected object
                {
                    var selectInteract = selectedObject.GetComponent<IInteractable>();
                    if(selectedObject != null) selectInteract.Deselect();

                    selectedObject = null;
                }


                var interactable = hit.collider.GetComponent<IInteractable>();
                if(interactable != null)
                {
                    selectedObject = hit.collider.gameObject;
                    interactable.OnClick();
                }
            }
        }
        else if (Input.GetMouseButtonDown(1)) // right click (to do actions with selected object)
        {
            // Send ray
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Debug.DrawLine(ray.origin, ray.direction * 1000.0f, Color.blue, 10.0f);

            Debug.Log("1");

            // If ray hit something & and we have a selected object
            if (Physics.Raycast(ray, out hit,500.0f, LayerMask.GetMask("Default"), QueryTriggerInteraction.Ignore) && selectedObject != null)
            {
                Debug.Log("2");

                PlayerUnit selectedPig = selectedObject.GetComponent<PlayerUnit>();
                
                if (selectedPig != null) // if selected object is a pig
                {
                    Debug.Log("null test" + hit.collider.name + hit.collider.tag);

                    if (hit.collider.tag == "Ground") // move
                    {
                        Debug.Log("Groind");
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
}

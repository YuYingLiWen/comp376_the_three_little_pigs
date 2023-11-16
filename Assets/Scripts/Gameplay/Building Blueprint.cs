using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBlueprint : MonoBehaviour
{

    RaycastHit hit;
    Vector3 movePoint;
    public GameObject prefab; // the actual building


    void Start()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit))
        {
            transform.position = hit.point;
        }
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            transform.position = hit.point; // move the blueprint with the mouse
        }

        // When left clicked, destroy blueprint and instantiate real one 
        if (Input.GetMouseButtonDown(0))
        {
            Destroy(gameObject); // destroy the blueprint of the building
            Instantiate(prefab, transform.position, transform.rotation); // build the building
            // substract from resources here
        }
    }
}

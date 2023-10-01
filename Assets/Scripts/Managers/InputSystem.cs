using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSystem : MonoBehaviour
{
    public Action<bool> OnPause;

    bool isPaused = false;

    Ray mouseRay;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            isPaused = !isPaused;
            OnPause?.Invoke(isPaused);
        }


        if(Input.GetKeyUp(KeyCode.Mouse0)) MouseClick();
    }


    private void MouseClick()
    {
        mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        Debug.DrawLine(mouseRay.origin, mouseRay.direction * 1000f, Color.red); 

        RaycastHit hit;
        
        if(Physics.Raycast(mouseRay,out hit, float.MaxValue))
        {
            var gameObject = hit.collider.gameObject;

            if (gameObject.CompareTag("Enemy")) // Example 
            {
                gameObject.GetComponent<TestEnemy>().Click();
            }
            else
            {
                Debug.Log(gameObject.name); // Example
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private GameObject aButtonMenu;
    [SerializeField] private GameObject bButtonMenu;
    [SerializeField] private GameObject rightHandAnchore;

    private InteractableObject currentSelectedObject;

    private bool onGrab;
    // Start is called before the first frame update
    void Start()
    {
        currentSelectedObject = null;
        onGrab = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        if (InputSystem.IsLandscapeMode())
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 10f,
                LayerMask.NameToLayer("InteractableObject")))
            {
                Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * hit.distance, Color.blue);
                InteractableObject tempObject = hit.transform.GetComponent<InteractableObject>();
                if (tempObject != currentSelectedObject)
                {
                    tempObject.OnSelected();
                    currentSelectedObject.OffSelected();
                    currentSelectedObject = tempObject;
                }

            }
            else
            {
                currentSelectedObject.OffSelected();
                
            }
        }
        else
        {
            
        }

        if (currentSelectedObject != null)
        {
            if (InputSystem.GetButton(ButtonType.Grip))
            {
                if (!onGrab)
                {
                    currentSelectedObject.OnGrap(transform);
                    onGrab = true;
                }
                
            }
            else
            {
                if (onGrab)
                {
                    currentSelectedObject.OffGrap();
                    onGrab = false;
                }
            }
            
            InputSystem.GetButton(ButtonType.Trigger);
        }


    }
}

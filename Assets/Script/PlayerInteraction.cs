using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private GameObject aButtonMenu;
    [SerializeField] private GameObject bButtonMenu;
    [SerializeField] private GameObject rightHandAnchore;
    [SerializeField] private GameObject rightHandAnchoreRay;
    [SerializeField] private GameObject cameraAnchore;
    [SerializeField] private GameObject cameraRayMarker;
    

    private InteractableObject currentSelectedObject;

    private bool previousButtonAState = false;
    private bool previousButtonBState = false;
    private bool previousButtonTriggerState = false;
    private bool previousButtonGrapState = false;

    private bool onGrab;
    // Start is called before the first frame update
    void Start()
    {
        currentSelectedObject = null;
        onGrab = false;
        previousButtonAState = false;
        previousButtonBState = false;
        previousButtonTriggerState = false;
        previousButtonGrapState = false;
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
            cameraRayMarker.SetActive(true);
            rightHandAnchoreRay.SetActive(false);
            if (onGrab)
            {
                
            }
            else
            {
                Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 10f,
                    Color.blue);
                if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 10f,
                    1 << LayerMask.NameToLayer("InteractableObject")))
                {
                    InteractableObject tempObject = hit.transform.GetComponent<InteractableObject>();
                    if (tempObject != currentSelectedObject)
                    {
                        tempObject.OnSelected();
                        if(currentSelectedObject != null)
                            currentSelectedObject.OffSelected();
                        currentSelectedObject = tempObject;
                    }
                }
                else
                {
                    if (currentSelectedObject != null)
                    {
                        currentSelectedObject.OffSelected();
                        currentSelectedObject = null;
                    }
                }
            }
        }
        else
        {
            rightHandAnchoreRay.SetActive(true);
            cameraRayMarker.SetActive(false);
            if (onGrab)
            {
                
            }
            else
            {
                Debug.DrawRay(rightHandAnchore.transform.position, -rightHandAnchore.transform.right * 10f,
                    Color.red);
                if (Physics.Raycast(rightHandAnchore.transform.position, -rightHandAnchore.transform.right, out hit, 10f,
                    1 << LayerMask.NameToLayer("InteractableObject")))
                {
                    InteractableObject tempObject = hit.transform.GetComponent<InteractableObject>();
                    if (tempObject != currentSelectedObject)
                    {
                        tempObject.OnSelected();
                        if(currentSelectedObject != null)
                            currentSelectedObject.OffSelected();
                        currentSelectedObject = tempObject;
                    }
                }
                else
                {
                    if (currentSelectedObject != null)
                    {
                        currentSelectedObject.OffSelected();
                        currentSelectedObject = null;
                    }
                }
            }
            
        }

        if (currentSelectedObject != null)
        {
            if (InputSystem.GetButton(ButtonType.Grip))
            {
                if (!onGrab)
                {
                    if (InputSystem.IsLandscapeMode())
                    {
                        currentSelectedObject.OnGrap(cameraAnchore.transform);
                    }
                    else
                    {
                        currentSelectedObject.OnGrap(rightHandAnchore.transform);
                    }

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

            if (InputSystem.GetButton(ButtonType.Trigger) && !previousButtonTriggerState)
            {
                currentSelectedObject.OnTrigger();
            }
            
        }
            
            
        if (InputSystem.GetButton(ButtonType.ButtonA) && !previousButtonAState)
        {
            aButtonMenu.SetActive(!aButtonMenu.activeSelf);
        }
            
        if (InputSystem.GetButton(ButtonType.ButtonB) && !previousButtonBState)
        {
            bButtonMenu.SetActive(!bButtonMenu.activeSelf);
        }

        ButtonStateUpdate();
    }

    private void ButtonStateUpdate()
    {
        previousButtonAState = InputSystem.GetButton(ButtonType.ButtonA);
        previousButtonBState = InputSystem.GetButton(ButtonType.ButtonB);
        previousButtonGrapState = InputSystem.GetButton(ButtonType.Grip);
        previousButtonTriggerState = InputSystem.GetButton(ButtonType.Trigger);
    }
    

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardCursor : MonoBehaviour
{
    private DataStructs.VRKeyboardStruct cursorKeyboardData;
    private KeyboardButton currentButton;
    
    void Start()
    {
        currentButton = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentButton != null)
            currentButton.ButtonPress();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("VRKeyboardButton"))
        {
            if (currentButton == null)
                currentButton = other.GetComponent<KeyboardButton>();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("VRKeyboardButton"))
        {
            if (currentButton == null)
                currentButton = other.GetComponent<KeyboardButton>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("VRKeyboardButton"))
        {
            if (currentButton.transform == other.transform)
            {
                currentButton.ButtonExit();
                currentButton = null;
            }
                
        }
        
    }

    public void PointerUp()
    {
        if (currentButton != null)
        {
            currentButton.ButtonUp();
            currentButton = null;
        }

        gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRControllerTest : MonoBehaviour
{
    public Text debugText;
    

    // Update is called once per frame
    void Update()
    {
        debugText.text = "Trigger Pressed : " + InputSystem.GetButton(ButtonType.Trigger, ControllerType.LeftController).ToString() +
                         "\nGrip Pressed : " + InputSystem.GetButton(ButtonType.Grip, ControllerType.LeftController).ToString() +
                         "\nButton A Pressed : " + InputSystem.GetButton(ButtonType.ButtonA, ControllerType.RightController).ToString() +
                         "\nButton B Pressed : " + InputSystem.GetButton(ButtonType.ButtonB, ControllerType.RightController).ToString() +
                         "\nright Joystick Axis : " + InputSystem.GetAxis2D(ControllerType.RightController).ToString() +
                         "\nleft Joystick Axis : " + InputSystem.GetAxis2D(ControllerType.LeftController).ToString();
    }
}

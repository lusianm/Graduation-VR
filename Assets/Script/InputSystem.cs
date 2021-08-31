using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControllerType
{
    RightController = 1, LeftController = 2
}

public enum ButtonType
{
    Trigger = 1, Grip = 2, ButtonA = 3, ButtonB = 4
}

public class InputSystem : MonoBehaviour
{
    static private DataStructs.VRControllerStruct rightHandControllerData, leftHandControllerData;
    private static bool isLandscapeMode;
    
    // Start is called before the first frame update
    void Start()
    {
        rightHandControllerData.controllerType = ControllerType.RightController;
        InitControllerData(rightHandControllerData);
        leftHandControllerData.controllerType = ControllerType.LeftController; 
        InitControllerData(leftHandControllerData);
        
        isLandscapeMode = true;
    }

    void InitControllerData(DataStructs.VRControllerStruct data)
    {
        data.isButton1Pressed = false;
        data.isButton2Pressed = false;
        data.isJoysticPressed = false;
        data.joysticAxis = Vector2.zero;
        data.deviceOrientation = DeviceOrientation.Unknown;
    }

    public static void SetControllerData(DataStructs.VRControllerStruct data)
    {
        if (data.controllerType == ControllerType.LeftController)
        {
            leftHandControllerData = data;
        }
        else
        {
            rightHandControllerData = data;
        }

        isLandscapeMode = (data.deviceOrientation == DeviceOrientation.LandscapeLeft ||
                           data.deviceOrientation == DeviceOrientation.LandscapeRight);
    }

    public static bool GetButton(ButtonType buttonType, ControllerType controllerType = ControllerType.RightController)
    {
        switch (buttonType)
        {
            case ButtonType.Trigger:
                if (isLandscapeMode)
                    return leftHandControllerData.isButton1Pressed;
                else
                    return rightHandControllerData.isButton1Pressed;
                break;
            case ButtonType.Grip:
                if (isLandscapeMode)
                    return leftHandControllerData.isButton2Pressed;
                else
                    return rightHandControllerData.isButton2Pressed;
                break;
            case ButtonType.ButtonA:
                if (isLandscapeMode)
                    return rightHandControllerData.isButton2Pressed;
                else
                    return false;
                break;
            case ButtonType.ButtonB:
                if (isLandscapeMode)
                    return rightHandControllerData.isButton1Pressed;
                else
                    return false;
                break;
        }

        return false;
    }

    public static Vector2 GetAxis2D(ControllerType controllerType = ControllerType.RightController)
    {
        switch (controllerType)
        {
            case ControllerType.LeftController:
                if (leftHandControllerData.isJoysticPressed)
                    return leftHandControllerData.joysticAxis;
                else
                    return Vector2.zero;
                break;
            case ControllerType.RightController:
                if (rightHandControllerData.isJoysticPressed)
                    return rightHandControllerData.joysticAxis;
                else
                    return Vector2.zero;
                break;
        }
        
        return Vector2.zero;
    }
    
}

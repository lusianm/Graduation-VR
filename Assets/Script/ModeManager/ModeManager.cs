using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeManager : MonoBehaviour
{
    private int currentFunctionType;
    [SerializeField] private ByteToRawTexture byteToRawTexture;
    [SerializeField] private PartialVideoSeeThroughManager partialVideoSeeThroughManager;
    [SerializeField] private MirroringManager mirroringManager;
    [SerializeField] private KeyboardManager keyboardManager;
    private bool isAndroidModeActivated;

    public int GetCurrentModeMangerFunctionType() => currentFunctionType;

    private void Start()
    {
        currentFunctionType = -1;
        isAndroidModeActivated = false;
        partialVideoSeeThroughManager.closePartialVideoSeeThrough();
        mirroringManager.closeMirroringMode();
        keyboardManager.CloseKeyboard();
    }

    public void MobileModeChangeRequest(int changeFunctionType)
    {
        isAndroidModeActivated = true;
        if (changeFunctionType == currentFunctionType)
            return;
        TCP_SERVER.GetInstance().ChangeMobileMode(changeFunctionType);
    }
    
    public void MobileModeChangeRequest(FunctionTypes changeFunctionType)
    {
        isAndroidModeActivated = true;
        if ((int)changeFunctionType == currentFunctionType)
            return;
        TCP_SERVER.GetInstance().ChangeMobileMode(changeFunctionType);
    }

    public void ModeChange(int changeMode)
    {
        if (!isAndroidModeActivated)
            return;
        //turn off current mode
        switch (currentFunctionType)
        {
            case 1:
                partialVideoSeeThroughManager.closePartialVideoSeeThrough();
                break;
            case 2:
                mirroringManager.closeMirroringMode();
                break;
            case 3:
                break;
            case 4:
                keyboardManager.CloseKeyboard();
                break;
                
        }
        
        
        //turn on new mode
        switch (changeMode)
        {
            case 1:
                partialVideoSeeThroughManager.SetMode(0, byteToRawTexture.GetMobileImageTexture());
                break;
            case 2:
                mirroringManager.SetMirroringMode(byteToRawTexture.GetMobileImageTexture());
                break;
            case 3:
                break;
            case 4:
                keyboardManager.SetupKeyboard();
                break;
                
        }

        currentFunctionType = changeMode;
    }

    public void TurnOffAndroidMode()
    {
        partialVideoSeeThroughManager.closePartialVideoSeeThrough();
        mirroringManager.closeMirroringMode();
        keyboardManager.CloseKeyboard();
        
        currentFunctionType = -1;
    }

    public void SetPartialVideoSeeThroughMode(int modeType = 0)
    {
        partialVideoSeeThroughManager.SetMode(modeType, byteToRawTexture.GetMobileImageTexture());
    }

    public void ClosePartialVideoSeeThrough()
    {
        partialVideoSeeThroughManager.closePartialVideoSeeThrough();
    }
    
}

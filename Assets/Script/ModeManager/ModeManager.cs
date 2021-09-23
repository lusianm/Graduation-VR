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

    private void Start()
    {
        partialVideoSeeThroughManager.closePartialVideoSeeThrough();
        keyboardManager.CloseKeyboard();
    }

    public void ChangeMobileMode(int changeFunctionType)
    {
        if (changeFunctionType == currentFunctionType)
            return;
        TCP_SERVER.GetInstance().ChangeMobileMode(changeFunctionType);
        ModeChange(changeFunctionType);
    }
    
    public void ChangeMobileMode(FunctionTypes changeFunctionType)
    {
        if ((int)changeFunctionType == currentFunctionType)
            return;
        TCP_SERVER.GetInstance().ChangeMobileMode(changeFunctionType);
        ModeChange((int)changeFunctionType);
    }

    private void ModeChange(int changeMode)
    {
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

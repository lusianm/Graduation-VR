using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FingerTipUIPanel : MonoBehaviour
{
    public FingerTipUIPanel parentPanel = null;
    [SerializeField] private List<FingerTipUIButton> buttonList;

    private int currentBaseButtonNum = 0;

    private void Start()
    {
        currentBaseButtonNum = 0;
    }

    public void SetParentPanel(FingerTipUIPanel _parentPanel)
    {
        parentPanel = _parentPanel;
    }

    public FingerTipUIPanel ButtonAction(int buttonNum)
    {
        if (buttonNum > 4)
        {
            Debug.LogError("Out of FingerTipUI Button Num Boundary");
            return null;
        }
        
        if (buttonNum + currentBaseButtonNum > buttonList.Count)
        {
            Debug.Log("It is Empty FingerTipUI Button");
            return null;
        }

        return buttonList[buttonNum + currentBaseButtonNum].ButtonAction();
    }
}

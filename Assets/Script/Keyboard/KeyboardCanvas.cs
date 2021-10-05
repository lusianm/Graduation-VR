using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;


public enum KeyboardButtonType
{
    a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,comma,period,questionMark,num1,num2,num3,num4,num5,num6,num7,num8,num9,num0,enter,delete,backSpace,shift,space
}


public class KeyboardCanvas : MonoBehaviour
{
    [SerializeField] private Transform keyboardPlane;
    [SerializeField] private Transform keyboardTempPointer;
    [SerializeField] private Transform pointersParent;
    [SerializeField] private List<Transform> adjustPointsTransforms;
    [SerializeField] private OVRSkeleton rightHand;
    private Transform rightHandIndexTip;
    
    private bool isCanvasAdjusting = true;
    private int currentAdjustPoint;
    private Vector3[] adjustPosition;
    

    private string inputText = "";
    public TextMeshPro _textMeshPro;
    private bool isShiftPressed = false;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform point in adjustPointsTransforms)
        {
            point.gameObject.SetActive(false);
        }
        
        currentAdjustPoint = -1;
        adjustPosition = new Vector3[3];
        isCanvasAdjusting = false;
        
        
        for (int i = 0; i < rightHand.Bones.Count; i++)
        {
            if (rightHand.Bones[i].Id == OVRSkeleton.BoneId.Hand_IndexTip)
            {
                rightHandIndexTip = rightHand.Bones[i].Transform;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        _textMeshPro.text = inputText;
        
        if (DataStructs.vrKeyboardBuffor.Count == 0)
        {
            return;
        }

        DataStructs.VRKeyboardStruct[] tempDataList = new DataStructs.VRKeyboardStruct[DataStructs.vrKeyboardBuffor.Count];
        DataStructs.vrKeyboardBuffor.CopyTo(tempDataList);
        DataStructs.vrKeyboardBuffor.Clear();
        foreach (DataStructs.VRKeyboardStruct touchInfo in tempDataList)
        {
            MovePointer(touchInfo);
        }
    }

    public void MovePointer(DataStructs.VRKeyboardStruct keyBoardData)
    {
        Transform tempPointer = pointersParent.GetChild(keyBoardData.touchID);
        if (keyBoardData.isPressed)
        {
            if (!tempPointer.gameObject.activeSelf)
            {
                tempPointer.gameObject.SetActive(true);
            }
            
            Vector2 movePosition = new Vector2(keyBoardData.touchPosition.x / keyBoardData.screenResolution.x,
                keyBoardData.touchPosition.y / keyBoardData.screenResolution.y);

            movePosition.x = movePosition.x - 0.5f;
            movePosition.y = movePosition.y - 0.5f;

            keyboardTempPointer.localPosition = new Vector3(movePosition.x, 5f, movePosition.y);
            tempPointer.position = keyboardTempPointer.position;
        }
        else
        {
            if (isCanvasAdjusting)
            {
                adjustPosition[currentAdjustPoint] = rightHandIndexTip.position;
                adjustPointsTransforms[currentAdjustPoint].gameObject.SetActive(false);
                currentAdjustPoint++;
                if (currentAdjustPoint < 3)
                {
                    adjustPointsTransforms[currentAdjustPoint].gameObject.SetActive(true);
                }
                else
                {
                    ApplyKeyboardAdjustData();
                }

            }
            
            
            if (tempPointer.gameObject.activeSelf)
            {
                tempPointer.GetComponent<KeyboardCursor>().PointerUp();
            }
        }
    }

    public void SetKeyboardAdjusting()
    {
        isCanvasAdjusting = true;
        currentAdjustPoint = 0;
        adjustPointsTransforms[currentAdjustPoint].gameObject.SetActive(true);
    }

    private void ApplyKeyboardAdjustData()
    {
        inputText = "";
        Vector3 line12 = adjustPosition[0] - adjustPosition[1];
        Vector3 line23 = adjustPosition[2] - adjustPosition[1];
        transform.localScale = new Vector3(line12.magnitude, (line12.magnitude+line23.magnitude)/2, line23.magnitude * 2);
        transform.up = Vector3.Cross(line23, line12);
        transform.right = -line12;
        transform.position = (adjustPosition[0] + adjustPosition[2]) / 2;

        isCanvasAdjusting = false;
    }


    public void ButtonPressed(KeyboardButtonType buttonType)
    {
        switch (buttonType)
        {
            case KeyboardButtonType.a:
                break;
            case KeyboardButtonType.b:
                break;
            case KeyboardButtonType.c:
                break;
            case KeyboardButtonType.d:
                break;
            case KeyboardButtonType.e:
                break;
            case KeyboardButtonType.f:
                break;
            case KeyboardButtonType.g:
                break;
            case KeyboardButtonType.h:
                break;
            case KeyboardButtonType.i:
                break;
            case KeyboardButtonType.j:
                break;
            case KeyboardButtonType.k:
                break;
            case KeyboardButtonType.l:
                break;
            case KeyboardButtonType.m:
                break;
            case KeyboardButtonType.n:
                break;
            case KeyboardButtonType.o:
                break;
            case KeyboardButtonType.p:
                break;
            case KeyboardButtonType.q:
                break;
            case KeyboardButtonType.r:
                break;
            case KeyboardButtonType.s:
                break;
            case KeyboardButtonType.t:
                break;
            case KeyboardButtonType.u:
                break;
            case KeyboardButtonType.v:
                break;
            case KeyboardButtonType.w:
                break;
            case KeyboardButtonType.x:
                break;
            case KeyboardButtonType.y:
                break;
            case KeyboardButtonType.z:
                break;
            case KeyboardButtonType.comma:
                break;
            case KeyboardButtonType.period:
                break;
            case KeyboardButtonType.questionMark:
                break;
            case KeyboardButtonType.num1:
                break;
            case KeyboardButtonType.num2:
                break;
            case KeyboardButtonType.num3:
                break;
            case KeyboardButtonType.num4:
                break;
            case KeyboardButtonType.num5:
                break;
            case KeyboardButtonType.num6:
                break;
            case KeyboardButtonType.num7:
                break;
            case KeyboardButtonType.num8:
                break;
            case KeyboardButtonType.num9:
                break;
            case KeyboardButtonType.num0:
                break;
            case KeyboardButtonType.enter:
                break;
            case KeyboardButtonType.delete:
                break;
            case KeyboardButtonType.backSpace:
                break;
            case KeyboardButtonType.space:
                break;
            case KeyboardButtonType.shift:
                isShiftPressed = true;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(buttonType), buttonType, null);
        }
    }

    public void ButtonExit(KeyboardButtonType buttonType)
    {
        switch (buttonType)
        {
            case KeyboardButtonType.a:
                break;
            case KeyboardButtonType.b:
                break;
            case KeyboardButtonType.c:
                break;
            case KeyboardButtonType.d:
                break;
            case KeyboardButtonType.e:
                break;
            case KeyboardButtonType.f:
                break;
            case KeyboardButtonType.g:
                break;
            case KeyboardButtonType.h:
                break;
            case KeyboardButtonType.i:
                break;
            case KeyboardButtonType.j:
                break;
            case KeyboardButtonType.k:
                break;
            case KeyboardButtonType.l:
                break;
            case KeyboardButtonType.m:
                break;
            case KeyboardButtonType.n:
                break;
            case KeyboardButtonType.o:
                break;
            case KeyboardButtonType.p:
                break;
            case KeyboardButtonType.q:
                break;
            case KeyboardButtonType.r:
                break;
            case KeyboardButtonType.s:
                break;
            case KeyboardButtonType.t:
                break;
            case KeyboardButtonType.u:
                break;
            case KeyboardButtonType.v:
                break;
            case KeyboardButtonType.w:
                break;
            case KeyboardButtonType.x:
                break;
            case KeyboardButtonType.y:
                break;
            case KeyboardButtonType.z:
                break;
            case KeyboardButtonType.comma:
                break;
            case KeyboardButtonType.period:
                break;
            case KeyboardButtonType.questionMark:
                break;
            case KeyboardButtonType.num1:
                break;
            case KeyboardButtonType.num2:
                break;
            case KeyboardButtonType.num3:
                break;
            case KeyboardButtonType.num4:
                break;
            case KeyboardButtonType.num5:
                break;
            case KeyboardButtonType.num6:
                break;
            case KeyboardButtonType.num7:
                break;
            case KeyboardButtonType.num8:
                break;
            case KeyboardButtonType.num9:
                break;
            case KeyboardButtonType.num0:
                break;
            case KeyboardButtonType.enter:
                break;
            case KeyboardButtonType.delete:
                break;
            case KeyboardButtonType.backSpace:
                break;
            case KeyboardButtonType.space:
                break;
            case KeyboardButtonType.shift:
                isShiftPressed = false;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(buttonType), buttonType, null);
        }
    }

    public void ButtonUp(KeyboardButtonType buttonType)
    {
        switch (buttonType)
        {
            case KeyboardButtonType.a:
                if (!isShiftPressed)
                {
                    inputText += 'a';
                }
                else
                {
                    inputText += 'A';
                }
                break;
            case KeyboardButtonType.b:
                if (!isShiftPressed)
                {
                    inputText += 'b';
                }
                else
                {
                    inputText += 'B';
                }
                break;
            case KeyboardButtonType.c:
                if (!isShiftPressed)
                {
                    inputText += 'c';
                }
                else
                {
                    inputText += 'C';
                }
                break;
            case KeyboardButtonType.d:
                if (!isShiftPressed)
                {
                    inputText += 'd';
                }
                else
                {
                    inputText += 'D';
                }
                break;
            case KeyboardButtonType.e:
                if (!isShiftPressed)
                {
                    inputText += 'e';
                }
                else
                {
                    inputText += 'E';
                }
                break;
            case KeyboardButtonType.f:
                if (!isShiftPressed)
                {
                    inputText += 'f';
                }
                else
                {
                    inputText += 'F';
                }
                break;
            case KeyboardButtonType.g:
                if (!isShiftPressed)
                {
                    inputText += 'g';
                }
                else
                {
                    inputText += 'G';
                }
                break;
            case KeyboardButtonType.h:
                if (!isShiftPressed)
                {
                    inputText += 'h';
                }
                else
                {
                    inputText += 'H';
                }
                break;
            case KeyboardButtonType.i:
                if (!isShiftPressed)
                {
                    inputText += 'i';
                }
                else
                {
                    inputText += 'I';
                }
                break;
            case KeyboardButtonType.j:
                if (!isShiftPressed)
                {
                    inputText += 'j';
                }
                else
                {
                    inputText += 'J';
                }
                break;
            case KeyboardButtonType.k:
                if (!isShiftPressed)
                {
                    inputText += 'k';
                }
                else
                {
                    inputText += 'K';
                }
                break;
            case KeyboardButtonType.l:
                if (!isShiftPressed)
                {
                    inputText += 'l';
                }
                else
                {
                    inputText += 'L';
                }
                break;
            case KeyboardButtonType.m:
                if (!isShiftPressed)
                {
                    inputText += 'm';
                }
                else
                {
                    inputText += 'M';
                }
                break;
            case KeyboardButtonType.n:
                if (!isShiftPressed)
                {
                    inputText += 'n';
                }
                else
                {
                    inputText += 'N';
                }
                break;
            case KeyboardButtonType.o:
                if (!isShiftPressed)
                {
                    inputText += 'o';
                }
                else
                {
                    inputText += 'O';
                }
                break;
            case KeyboardButtonType.p:
                if (!isShiftPressed)
                {
                    inputText += 'p';
                }
                else
                {
                    inputText += 'P';
                }
                break;
            case KeyboardButtonType.q:
                if (!isShiftPressed)
                {
                    inputText += 'q';
                }
                else
                {
                    inputText += 'Q';
                }
                break;
            case KeyboardButtonType.r:
                if (!isShiftPressed)
                {
                    inputText += 'r';
                }
                else
                {
                    inputText += 'R';
                }
                break;
            case KeyboardButtonType.s:
                if (!isShiftPressed)
                {
                    inputText += 's';
                }
                else
                {
                    inputText += 'S';
                }
                break;
            case KeyboardButtonType.t:
                if (!isShiftPressed)
                {
                    inputText += 't';
                }
                else
                {
                    inputText += 'T';
                }
                break;
            case KeyboardButtonType.u:
                if (!isShiftPressed)
                {
                    inputText += 'u';
                }
                else
                {
                    inputText += 'U';
                }
                break;
            case KeyboardButtonType.v:
                if (!isShiftPressed)
                {
                    inputText += 'v';
                }
                else
                {
                    inputText += 'V';
                }
                break;
            case KeyboardButtonType.w:
                if (!isShiftPressed)
                {
                    inputText += 'w';
                }
                else
                {
                    inputText += 'W';
                }
                break;
            case KeyboardButtonType.x:
                if (!isShiftPressed)
                {
                    inputText += 'x';
                }
                else
                {
                    inputText += 'X';
                }
                break;
            case KeyboardButtonType.y:
                if (!isShiftPressed)
                {
                    inputText += 'y';
                }
                else
                {
                    inputText += 'Y';
                }
                break;
            case KeyboardButtonType.z:
                if (!isShiftPressed)
                {
                    inputText += 'z';
                }
                else
                {
                    inputText += 'Z';
                }
                break;
            case KeyboardButtonType.comma:
                if (!isShiftPressed)
                {
                    inputText += ',';
                }
                else
                {
                    inputText += '!';
                }
                break;
            case KeyboardButtonType.period:
                if (!isShiftPressed)
                {
                    inputText += '.';
                }
                else
                {
                    inputText += '?';
                }
                break;
            case KeyboardButtonType.num1:
                if (!isShiftPressed)
                {
                    inputText += '1';
                }
                else
                {
                    inputText += '-';
                }
                break;
            case KeyboardButtonType.num2:
                if (!isShiftPressed)
                {
                    inputText += '2';
                }
                else
                {
                    inputText += '@';
                }
                break;
            case KeyboardButtonType.num3:
                if (!isShiftPressed)
                {
                    inputText += '3';
                }
                else
                {
                    inputText += '#';
                }
                break;
            case KeyboardButtonType.num4:
                if (!isShiftPressed)
                {
                    inputText += '4';
                }
                else
                {
                    inputText += '/';
                }
                break;
            case KeyboardButtonType.num5:
                if (!isShiftPressed)
                {
                    inputText += '5';
                }
                else
                {
                    inputText += '%';
                }
                break;
            case KeyboardButtonType.num6:
                if (!isShiftPressed)
                {
                    inputText += '6';
                }
                else
                {
                    inputText += '^';
                }
                break;
            case KeyboardButtonType.num7:
                if (!isShiftPressed)
                {
                    inputText += '7';
                }
                else
                {
                    inputText += '&';
                }
                break;
            case KeyboardButtonType.num8:
                if (!isShiftPressed)
                {
                    inputText += '8';
                }
                else
                {
                    inputText += '*';
                }
                break;
            case KeyboardButtonType.num9:
                if (!isShiftPressed)
                {
                    inputText += '9';
                }
                else
                {
                    inputText += '(';
                }
                break;
            case KeyboardButtonType.num0:
                if (!isShiftPressed)
                {
                    inputText += '0';
                }
                else
                {
                    inputText += ')';
                }
                break;
            case KeyboardButtonType.enter:
                inputText += '\n';
                break;
            case KeyboardButtonType.delete:
                break;
            case KeyboardButtonType.backSpace:
                inputText = inputText.Substring(0, inputText.Length - 1);
                break;
            case KeyboardButtonType.space:
                inputText += ' ';
                break;
            case KeyboardButtonType.shift:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(buttonType), buttonType, null);
        }
    }

}

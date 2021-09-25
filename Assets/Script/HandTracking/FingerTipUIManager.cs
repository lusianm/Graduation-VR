using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerTipUIManager : MonoBehaviour
{

    [SerializeField] private GameObject fingerTipCanvas;
    private RectTransform fingerTipCanvasRectTransform;
    [SerializeField] private Transform fingerTipHandAnchor;

    [SerializeField] private RectTransform mainPanelPosition, subPanelPosition, vanishingPanelPosition;
    [SerializeField] private FingerTipUIPanel rootPanel;

    [SerializeField] private OVRHand fingerTipUIHand;

    [SerializeField] private Camera mainCamera;

    private FingerTipUIPanel mainPanel, subPanel;

    private float fingerTipUIActivateLimit;
    private float indexFingerUIActivateLimit = 0f;
    private float middleFingerUIActivateLimit = 0f;
    private float pinkyFingerUIActivateLimit = 0f;
    private float ringFingerUIActivateLimit = 0f;
    
    
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        fingerTipUIActivateLimit = 0f;
        fingerTipCanvasRectTransform = fingerTipCanvas.GetComponent<RectTransform>();
        DeactivateCanvas();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (fingerTipUIHand.IsDataValid)
        {
            if (!fingerTipCanvas.activeSelf)
            {
                
                if ((Time.time < fingerTipUIActivateLimit) &&
                    !fingerTipUIHand.GetFingerIsPinching(OVRHand.HandFinger.Index))
                {
                    ActivateCanvas();
                    fingerTipUIActivateLimit = 0f;
                }

                if (fingerTipUIHand.GetFingerIsPinching(OVRHand.HandFinger.Index) && fingerTipUIHand.IsSystemGestureInProgress)
                    fingerTipUIActivateLimit = Time.time + 0.5f;

            }
            else
            {
                SetCanvasPosition();
                CheckFingerDownAction();
                CheckFingerUpAction();
            }
        }
        else
        {
            Debug.Log("Deactivate1");
            DeactivateCanvas();
        }

    }

    #region Canvas De/Activate
    // ReSharper disable Unity.PerformanceAnalysis
    void ActivateCanvas()
    {
        if (!fingerTipCanvas.activeSelf)
        {
            fingerTipCanvas.SetActive(true);
            SetPanelToMain(rootPanel);
        }
        else
        {
            DeactivateCanvas();
        }
    }

    

    void DeactivateCanvas()
    {
        fingerTipCanvas.SetActive(false);
    }

    #endregion

    #region Set Canvas/Panel Position
    
    void SetCanvasPosition()
    {
        //Canvas Position & Rotation
        fingerTipCanvas.transform.position = fingerTipHandAnchor.position;
        fingerTipCanvas.transform.LookAt(mainCamera.transform);
        fingerTipCanvas.transform.Translate(-1 * fingerTipCanvas.transform.right * fingerTipCanvasRectTransform.sizeDelta.x, Space.World);
        fingerTipCanvas.transform.LookAt(mainCamera.transform);
        fingerTipCanvas.transform.Rotate(0f, 180f, 0f, Space.Self);
    }

    void SetPanelToMain(FingerTipUIPanel newMainPanel)
    {
        if(mainPanel != null)
            VanishingPanel(mainPanel);

        mainPanel = newMainPanel;
        mainPanel.gameObject.SetActive(true);
        mainPanel.GetComponent<RectTransform>().position = mainPanelPosition.position;
        for (int i = 0; i < 4; i++)
        {
            FingerTipUIPanel tempPanel = mainPanel.GetChildPanel(i);
            if (tempPanel != null)
                ResetCanvasPosition(tempPanel);
        }
        
    }
    
    void ResetCanvasPosition(FingerTipUIPanel vanishingPanel)
    {
        VanishingPanel(vanishingPanel);
        for (int i = 0; i < 4; i++)
        {
            FingerTipUIPanel tempPanel = vanishingPanel.GetChildPanel(i);
            if (tempPanel != null)
                ResetCanvasPosition(tempPanel);
        }
    }

    void SetPanelToSub(FingerTipUIPanel newSubPanel)
    {
        if(subPanel != null)
            VanishingPanel(subPanel);

        subPanel = newSubPanel;
        subPanel.gameObject.SetActive(true);
        subPanel.GetComponent<RectTransform>().position = subPanelPosition.position;
    }

    void VanishingPanel(FingerTipUIPanel tempPanel)
    {
        tempPanel.GetComponent<RectTransform>().position = vanishingPanelPosition.position;
    }
    #endregion

    #region Check Finger

    // ReSharper disable Unity.PerformanceAnalysis
    void CheckFingerUpAction()
    {
        if (!fingerTipCanvas.activeSelf)
            return;
        
        if ((Time.time < fingerTipUIActivateLimit) &&
            !fingerTipUIHand.GetFingerIsPinching(OVRHand.HandFinger.Index) &&
            fingerTipUIHand.IsSystemGestureInProgress)
        {
            Debug.Log("Deactivate2");
            DeactivateCanvas();
            fingerTipUIActivateLimit = 0;
            return;
        }

        FingerTipUIPanel tempPanel = null;
        //Button Action Check
        if (indexFingerUIActivateLimit != 0)
        {
            if ((Time.time < indexFingerUIActivateLimit) &&
                !fingerTipUIHand.GetFingerIsPinching(OVRHand.HandFinger.Index))
            {
                tempPanel = mainPanel.ButtonAction(0);
                indexFingerUIActivateLimit = 0f;

                if (tempPanel != null)
                {
                    SetPanelToMain(tempPanel);
                }
                else
                {
                    Debug.Log("Deactivate3 : " + tempPanel);
                    DeactivateCanvas();
                }
            }
        }
        else if (middleFingerUIActivateLimit != 0)
        {
            if ((Time.time < middleFingerUIActivateLimit) &&
                !fingerTipUIHand.GetFingerIsPinching(OVRHand.HandFinger.Middle))
            {
                tempPanel = mainPanel.ButtonAction(1);
                middleFingerUIActivateLimit = 0f;

                if (tempPanel != null)
                {
                    SetPanelToMain(tempPanel);
                }
                else
                {
                    Debug.Log("Deactivate4");
                    DeactivateCanvas();
                }
            }
        }
        else if (ringFingerUIActivateLimit != 0)
        {
            if ((Time.time < ringFingerUIActivateLimit) &&
                !fingerTipUIHand.GetFingerIsPinching(OVRHand.HandFinger.Ring))
            {
                tempPanel = mainPanel.ButtonAction(2);
                ringFingerUIActivateLimit = 0f;

                if (tempPanel != null)
                {
                    SetPanelToMain(tempPanel);
                }
                else
                {
                    Debug.Log("Deactivate6");
                    DeactivateCanvas();
                }
            }
        }
        else if (pinkyFingerUIActivateLimit != 0)
        {
            if ((Time.time < pinkyFingerUIActivateLimit) &&
                !fingerTipUIHand.GetFingerIsPinching(OVRHand.HandFinger.Pinky))
            {
                tempPanel = mainPanel.ButtonAction(3);
                middleFingerUIActivateLimit = 0f;

                if (tempPanel != null)
                {
                    SetPanelToMain(tempPanel);
                }
                else
                {
                    Debug.Log("Deactivate5");
                    DeactivateCanvas();
                }
            }
        }
        
    }

    void CheckFingerDownAction()
    {
        if (!fingerTipCanvas.activeSelf)
            return;

        if (fingerTipUIHand.GetFingerIsPinching(OVRHand.HandFinger.Index) && fingerTipUIHand.IsSystemGestureInProgress)
        {
            fingerTipUIActivateLimit = Time.time + 0.2f;
        }

        if (fingerTipUIHand.GetFingerIsPinching(OVRHand.HandFinger.Index))
        {
            indexFingerUIActivateLimit = Time.time + 0.5f;
        }
        else if (fingerTipUIHand.GetFingerIsPinching(OVRHand.HandFinger.Middle))
        {
            middleFingerUIActivateLimit = Time.time + 0.5f;
        }
        else if (fingerTipUIHand.GetFingerIsPinching(OVRHand.HandFinger.Ring))
        {
            ringFingerUIActivateLimit = Time.time + 0.5f;
        }
        else if (fingerTipUIHand.GetFingerIsPinching(OVRHand.HandFinger.Pinky))
        {
            pinkyFingerUIActivateLimit = Time.time + 0.5f;
        }
                
                
        //UI 미리보기
        if (fingerTipUIHand.GetFingerPinchStrength(OVRHand.HandFinger.Index) > 0.8f)
        {
        }
        else if (fingerTipUIHand.GetFingerPinchStrength(OVRHand.HandFinger.Middle) > 0.8f)
        {
        }
        else if (fingerTipUIHand.GetFingerPinchStrength(OVRHand.HandFinger.Ring) > 0.8f)
        {
        }
        else if (fingerTipUIHand.GetFingerPinchStrength(OVRHand.HandFinger.Pinky) > 0.8f)
        {
        }
    }

    #endregion
    
    
}

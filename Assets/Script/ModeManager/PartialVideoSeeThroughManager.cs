using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartialVideoSeeThroughManager : MonoBehaviour
{
    [SerializeField] OVRSkeleton rightHand, leftHand;
    Transform rightHandIndexTip, rightHandThumbTip, rightHandThumb3, leftHandThumb3;
    [SerializeField] private Canvas userHandCanvas;
    private RectTransform userHandCanvasRectTransform;
    private float trackCoverTime;
    [SerializeField] private float trackCoverTimeDelay;
    private bool isPhoneTracking;
    private Texture currentSetTexture;
    private bool isPartialVideoSeeThroughActivated;
    [SerializeField] private Transform baseRotation;
    [SerializeField] private Transform forwardTransform;
    
    
    [SerializeField] private AndroidDisplayer uiAndroidDisplayer, handAndroidDisplayer, handPhoneAndroidDisplayer;

    Quaternion ToUnityQuaternion(Quaternion q) => new Quaternion(q.x, q.y, -q.z, -q.w);

    private int currentMode;
    void Start()
    {
        for (int i = 0; i < rightHand.Bones.Count; i++)
        {
            if (rightHand.Bones[i].Id == OVRSkeleton.BoneId.Hand_IndexTip)
            {
                rightHandIndexTip = rightHand.Bones[i].Transform;
            }

            if (rightHand.Bones[i].Id == OVRSkeleton.BoneId.Hand_ThumbTip)
            {
                rightHandThumbTip = rightHand.Bones[i].Transform;
            }

            if (rightHand.Bones[i].Id == OVRSkeleton.BoneId.Hand_Thumb3)
            {
                rightHandThumb3 = rightHand.Bones[i].Transform;
            }
        }

        for (int i = 0; i < leftHand.Bones.Count; i++)
        {
            if (leftHand.Bones[i].Id == OVRSkeleton.BoneId.Hand_Thumb3)
            {
                leftHandThumb3 = leftHand.Bones[i].Transform;
            }
        }
        
        userHandCanvasRectTransform = userHandCanvas.GetComponent<RectTransform>();
        trackCoverTime = Time.time;
        isPhoneTracking = false;
        currentMode = 0;
        currentSetTexture = null;
        isPartialVideoSeeThroughActivated = false;
    }

    private void FixedUpdate()
    {
        if (!isPartialVideoSeeThroughActivated)
            return;
        
        if(currentMode == 1)
            CheckIsHandTracked();

        switch (currentMode)
        {
            case 0:

                break;
            case 1:
                if (!rightHand.isActiveAndEnabled)
                {
                    
                    SetMode(0, handAndroidDisplayer.GetDisplayTexture());
                    break;
                }
                    
                UpdateHandCavasPosition();

                break;
        }
        
    }

    private void CheckIsHandTracked()
    {
        if (DataStructs.partialTrackingData.isTrack)
        {
            isPhoneTracking = true;
            trackCoverTime = Time.time + trackCoverTimeDelay;
        }
        else if(Time.time > trackCoverTime)
        {
            isPhoneTracking = false;
        }
    }

    private void UpdateHandCavasPosition()
    {
        if(!userHandCanvas.gameObject.activeSelf)
            userHandCanvas.gameObject.SetActive(true);
        
        if (isPhoneTracking)
        {
            handAndroidDisplayer.gameObject.SetActive(false);
            handPhoneAndroidDisplayer.gameObject.SetActive(true);
            handPhoneAndroidDisplayer.SetDisplayTexture(currentSetTexture);


            SetHandPhoneCanvasPosition();
        }
        else
        {
            handPhoneAndroidDisplayer.gameObject.SetActive(false);
            handAndroidDisplayer.gameObject.SetActive(true);
            handAndroidDisplayer.SetDisplayTexture(currentSetTexture);
            
            SetHandCanvasePosition();
        }

        return;

    }

    private void SetHandCanvasePosition()
    {
        userHandCanvasRectTransform.pivot = new Vector2(0.5f, 0f);
        Vector3 canvasVector = rightHandThumbTip.position - rightHandIndexTip.position;
        userHandCanvasRectTransform.position = rightHandIndexTip.position + (canvasVector / 2);
            
        userHandCanvasRectTransform.LookAt(Camera.main.transform);
        userHandCanvasRectTransform.sizeDelta = new Vector2(canvasVector.magnitude, canvasVector.magnitude * 1 / 1);
        
    }

    private void SetHandPhoneCanvasPosition()
    {
        userHandCanvasRectTransform.pivot = new Vector2(0.5f, 0.5f);
        baseRotation.parent.eulerAngles = new Vector3(90f, 137.5f, 0f);
        baseRotation.localEulerAngles = DataStructs.partialTrackingData.trackedRotation;
        userHandCanvasRectTransform.eulerAngles = baseRotation.eulerAngles;


        if (DataStructs.partialTrackingData.isRight)
        {
            userHandCanvasRectTransform.pivot = new Vector2(1f, 0f);
            userHandCanvasRectTransform.position = rightHandThumb3.position;
        }
        else
        {
            userHandCanvasRectTransform.pivot = new Vector2(0f, 0f);
            userHandCanvasRectTransform.position = leftHandThumb3.position;
        }
    }


    public void SetMode(int mode, Texture setTexture)
    {
        isPartialVideoSeeThroughActivated = true;
        currentSetTexture = setTexture;
        switch (mode)
        {
            case 0:
                if (currentMode == mode)
                {
                    uiAndroidDisplayer.SetDisplayTexture(setTexture);
                    uiAndroidDisplayer.ResetDispalyerSize(DataStructs.partialTrackingData.cameraResolution.x / 10f,
                        DataStructs.partialTrackingData.cameraResolution.y / 10f, 3, 3);
                    break;
                }

                currentMode = mode;

                uiAndroidDisplayer.gameObject.SetActive(true);
                userHandCanvas.gameObject.SetActive(false);
                handAndroidDisplayer.gameObject.SetActive(false);
                uiAndroidDisplayer.SetDisplayTexture(setTexture);
                break;
            case 1:
                if (currentMode == mode)
                {
                    handAndroidDisplayer.SetDisplayTexture(setTexture);
                    break;
                }
                currentMode = mode;
                
                uiAndroidDisplayer.gameObject.SetActive(false);
                userHandCanvas.gameObject.SetActive(true);
                handAndroidDisplayer.gameObject.SetActive(true);
                handAndroidDisplayer.SetDisplayTexture(setTexture);                
                break;
        }
    }

    public void closePartialVideoSeeThrough()
    {
        isPartialVideoSeeThroughActivated = false;
        currentMode = -1;
        uiAndroidDisplayer.gameObject.SetActive(false);
        handAndroidDisplayer.gameObject.SetActive(false);
        userHandCanvas.gameObject.SetActive(false);
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandTrackingTest : MonoBehaviour
{
    [SerializeField] private OVRHand rightHand, leftHand;

    [SerializeField] private Text rightText, leftText;
    [SerializeField] private GameObject rightPose, leftPose;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (rightHand.IsDataValid)
        {
            rightText.text = "Index : " + rightHand.GetFingerIsPinching(OVRHand.HandFinger.Index).ToString()
                                        + "\nIndex Strength : " + rightHand
                                            .GetFingerPinchStrength(OVRHand.HandFinger.Index).ToString()
                                        + "\nMiddle : " + rightHand.GetFingerIsPinching(OVRHand.HandFinger.Middle)
                                            .ToString()
                                        + "\nMiddle Strength : " + rightHand
                                            .GetFingerPinchStrength(OVRHand.HandFinger.Middle).ToString()
                                        + "\nPinky : " + rightHand.GetFingerIsPinching(OVRHand.HandFinger.Pinky)
                                            .ToString()
                                        + "\nPinky Strength : " + rightHand
                                            .GetFingerPinchStrength(OVRHand.HandFinger.Pinky).ToString()
                                        + "\nRing : " + rightHand.GetFingerIsPinching(OVRHand.HandFinger.Ring)
                                            .ToString()
                                        + "\nRing Strength : " + rightHand
                                            .GetFingerPinchStrength(OVRHand.HandFinger.Ring).ToString()
                                        + "\nThumb : " + rightHand.GetFingerIsPinching(OVRHand.HandFinger.Thumb)
                                            .ToString()
                                        + "\nThumb Strength : " + rightHand
                                            .GetFingerPinchStrength(OVRHand.HandFinger.Thumb).ToString()
                                        + "\nPointer Pose position : " + rightHand.PointerPose.position.ToString()
                                        + "\nPointer Pose Foward : " + rightHand.PointerPose.forward.ToString();
            
            rightPose.SetActive(true);
            rightPose.transform.position = rightHand.PointerPose.position;
            rightPose.transform.forward = rightHand.PointerPose.forward;
        }
        else
        {
            rightPose.SetActive(false);
        }
        if (leftHand.IsDataValid)
        {
            leftText.text = "Index : " + leftHand.GetFingerIsPinching(OVRHand.HandFinger.Index).ToString()
                                       + "\nIndex Strength : " + leftHand
                                           .GetFingerPinchStrength(OVRHand.HandFinger.Index).ToString()
                                       + "\nMiddle : " + leftHand.GetFingerIsPinching(OVRHand.HandFinger.Middle)
                                           .ToString()
                                       + "\nMiddle Strength : " + leftHand
                                           .GetFingerPinchStrength(OVRHand.HandFinger.Middle).ToString()
                                       + "\nPinky : " + leftHand.GetFingerIsPinching(OVRHand.HandFinger.Pinky)
                                           .ToString()
                                       + "\nPinky Strength : " + leftHand
                                           .GetFingerPinchStrength(OVRHand.HandFinger.Pinky).ToString()
                                       + "\nRing : " + leftHand.GetFingerIsPinching(OVRHand.HandFinger.Ring).ToString()
                                       + "\nRing Strength : " + leftHand
                                           .GetFingerPinchStrength(OVRHand.HandFinger.Ring).ToString()
                                       + "\nThumb : " + leftHand.GetFingerIsPinching(OVRHand.HandFinger.Thumb)
                                           .ToString()
                                       + "\nThumb Strength : " + leftHand
                                           .GetFingerPinchStrength(OVRHand.HandFinger.Thumb).ToString()
                                       + "\nPointer Pose position : " + leftHand.PointerPose.position.ToString()
                                       + "\nPointer Pose Foward : " + leftHand.PointerPose.forward.ToString();
            leftPose.SetActive(true);
            leftPose.transform.position = leftHand.PointerPose.position;
            leftPose.transform.forward = leftHand.PointerPose.forward;

        }
        else
        {
            leftPose.SetActive(false);
        }
    }
}

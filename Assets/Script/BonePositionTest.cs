using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonePositionTest : MonoBehaviour
{

    [SerializeField] OVRSkeleton rightHand, leftHand;
    Transform rightHandIndexTip, rightHandThumbTip, rightHandThumb3, leftHandThumb3;
    public Text text;
    public Canvas userCanvas;
    RectTransform canvasTransform;
    Vector3 canvasBaseRotation = new Vector3(10, 0, 0);
    private float trackCoverTime;
    private bool isPhoneTracking;
    private void Start()
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
        
        canvasTransform = userCanvas.GetComponent<RectTransform>();
        trackCoverTime = Time.time;
        isPhoneTracking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!rightHand.isActiveAndEnabled)
            return;
        
        if (isPhoneTracking && (Time.time > trackCoverTime))
        {
            isPhoneTracking = false;
        }

        if (isPhoneTracking)
        {
            text.text = "Right : " + rightHandThumb3.position.ToString();
            text.text += "\nLeft : " + leftHandThumb3.position.ToString();


            
            Vector3 canvasVector = rightHandThumbTip.position - rightHandIndexTip.position;
            //canvasTransform.position = rightHandIndexTip.position + (canvasVector / 2);
            //canvasTransform.LookAt(Camera.main.transform);
            //canvasTransform.Rotate(canvasBaseRotation);
            canvasTransform.sizeDelta = new Vector2(DataStructs.partialTrackingData.cameraResolution.y / 4000,
                DataStructs.partialTrackingData.cameraResolution.x / 4000);
            
            if (DataStructs.partialTrackingData.isRight)
            {
                canvasTransform.pivot = new Vector2(0f, 0f);
                canvasTransform.position = rightHandThumb3.position;
            }
            else
            {
                canvasTransform.pivot = new Vector2(1f, 0f);
                canvasTransform.position = leftHandThumb3.position;
            }
            
            canvasTransform.rotation = DataStructs.partialTrackingData.trackedRotation;
            canvasTransform.Rotate(0f, -195f, 180f, Space.Self);
            canvasTransform.Rotate(90f, 180f, -90f, Space.World);
        }
        else
        {
            canvasTransform.pivot = new Vector2(0.5f, 0f);
            text.text = "Right 검지 : " + rightHandIndexTip.position.ToString();
            text.text += "\nRight 엄지 : " + rightHandThumbTip.position.ToString();

            Vector3 canvasVector = rightHandThumbTip.position - rightHandIndexTip.position;
            canvasTransform.position = rightHandIndexTip.position + (canvasVector / 2);
            
            canvasTransform.LookAt(Camera.main.transform);
            //canvasTransform.Rotate(0f, 0f, -90f, Space.Self);
            //canvasTransform.Rotate(canvasBaseRotation);
            canvasTransform.sizeDelta = new Vector2(canvasVector.magnitude, canvasVector.magnitude * 1 / 1);
            
            
        }
        
    }

    public void SetTrackingData()
    {
        if (DataStructs.partialTrackingData.isTrack)
        {
            isPhoneTracking = true;
            trackCoverTime = Time.time + 0.5f;
        }
    }

}

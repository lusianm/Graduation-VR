using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerMove : MonoBehaviour
{
    private Vector2[] joystickAxis;
    private CharacterController character;
    public float moveSpeed = 0.1f;
    private Transform centerEyeAnchor;
    private float gravity = -9.8f;
    private float fallingSpeed;
    public float additionalHeight = 1f;

    public LayerMask groundLayer;
    
    // Start is called before the first frame update
    void Start()
    {
        character = transform.GetComponent<CharacterController>();
        joystickAxis = new Vector2[2];
        centerEyeAnchor = transform.GetComponent<OVRCameraRig>().centerEyeAnchor;
        //character.height = centerEyeAnchor.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        //joystickAxis[0] = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);
        joystickAxis[0] = InputSystem.GetAxis2D(ControllerType.RightController);
    }

    private void FixedUpdate()
    {
        CharacterCapsuleHeightAddjust();
        
        Quaternion headYaw = Quaternion.Euler(0, centerEyeAnchor.eulerAngles.y, 0);
        Vector3 direction = headYaw * new Vector3(joystickAxis[0].x, 0f, joystickAxis[0].y);
        character.Move(direction * (Time.fixedDeltaTime * moveSpeed));
        
        //gravity
        if (isGround())
            fallingSpeed = 0;
        else
        {
            fallingSpeed += gravity * Time.fixedDeltaTime;
            character.Move(Vector3.up * (fallingSpeed * Time.fixedDeltaTime));
        }
    }

    public void CharacterCapsuleHeightAddjust()
    {
        character.height = centerEyeAnchor.localPosition.y + additionalHeight;
        Vector3 capsuleCenter = transform.InverseTransformPoint(centerEyeAnchor.position);
        capsuleCenter.y = -(character.height/2 + character.skinWidth);
        character.center = capsuleCenter;
    }
    bool isGround()
    {
        Vector3 rayStart = transform.TransformPoint(character.center);
        float rayLength = character.center.y + 0.01f;
        bool hasHit = Physics.SphereCast(rayStart, character.radius, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer);
        return hasHit;
    }

}

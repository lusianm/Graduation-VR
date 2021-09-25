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

    private int leftControllerState = 0;
    
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
        
    }

    private void FixedUpdate()
    {
        joystickAxis[0] = InputSystem.GetAxis2D(ControllerType.RightController);
        joystickAxis[1] = InputSystem.GetAxis2D(ControllerType.LeftController);
        CharacterCapsuleHeightAddjust();
        CharacterMove();
    }

    void CharacterMove()
    {
        
        Quaternion headYaw = Quaternion.Euler(0, centerEyeAnchor.eulerAngles.y, 0);
        //right joystick input
        Vector3 rightMoveDirection = new Vector3(joystickAxis[0].x, 0f, joystickAxis[0].y);
        Vector3 leftMoveDirection;
        //left joystick input
        if (Vector2.Dot(joystickAxis[1], Vector2.left) > 0.7f)
        {
            if (leftControllerState != 1)
            {
                transform.Rotate(0f, -30f, 0f);
                leftControllerState = 1;
            }

            leftMoveDirection = Vector3.zero;
        }
        else if (Vector2.Dot(joystickAxis[1], Vector2.right) > 0.7f)
        {
            if (leftControllerState != 2)
            {
                transform.Rotate(0f, 30f, 0f);
                leftControllerState = 2;
            }

            leftMoveDirection = Vector3.zero;
        }
        else
        {
            leftControllerState = 0;
            Vector2 leftMoveVector;
            if (joystickAxis[1].y > 0)
            {
                leftMoveVector = Vector2.up * Vector2.Dot(Vector2.up, joystickAxis[1]);
            }
            else
            {
                leftMoveVector = Vector2.down * Vector2.Dot(Vector2.down, joystickAxis[1]);
            }
            leftMoveDirection = new Vector3(leftMoveVector.x, 0f, leftMoveVector.y);
        }

        Vector3 moveDirection = headYaw * ( rightMoveDirection + leftMoveDirection );
        
        
        
        character.Move(moveDirection * (Time.fixedDeltaTime * moveSpeed));
        
        //gravity
        if (isGround())
            fallingSpeed = 0;
        else
        {
            fallingSpeed += gravity * Time.fixedDeltaTime;
            character.Move(Vector3.up * (fallingSpeed * Time.fixedDeltaTime));
        }
        
    }

    void CharacterCapsuleHeightAddjust()
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

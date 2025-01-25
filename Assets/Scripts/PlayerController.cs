using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Bubble Variables")]
    public float blowSpeed;
    public float bubbleSpeed;
    public float bubbleSpeedMultiplier;
    public float lungCapacity;
    public float maxLungCapacity;
    public float airRemovalSpeed;
    public float maxBubbleCapacity;
    public float deflationRate;

    [Header("Movement Variables")]
    public float moveSpeed;
    public float gravity;
    public float moveMultiplier;


    [Header("References")]
    public InputAction moveAction;
    public InputAction blowAction;
    public Rigidbody rb;
    public AnimationCurve bubblePower;

    // Private state variables
    private Vector2 moveDirection;
    private float bubbleFullness = 0;

    private void OnEnable()
    {
        moveAction.Enable();
        blowAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        blowAction.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        moveDirection = new Vector2(0, 0);
        moveAction.performed += Move;
    }

    // Update is called once per frame
    void Update()
    {

        bool isBlowing = blowAction.ReadValue<float>() > 0.0f;
        if (isBlowing)
        {
            bubbleFullness = Math.Min(blowSpeed + bubbleFullness, maxBubbleCapacity);
        }
        // TODO: Make this done in a breath instead
        else
        {
            bubbleFullness = Math.Max(bubbleFullness - deflationRate, 0);
        }
        float normalizedPos = bubbleFullness / maxBubbleCapacity;
        bubbleSpeed = bubblePower.Evaluate(normalizedPos) * bubbleSpeedMultiplier;
    }

    private void FixedUpdate()
    {
        var deltaTime = Time.fixedDeltaTime;
        var moveDirDelta = moveDirection * deltaTime;

        // rb.velocity = new Vector3(moveDirDelta.x, 0, moveDirDelta.y);
    }

    private void Move(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            moveDirection = moveAction.ReadValue<Vector2>() * moveSpeed;
            rb.AddForce(new Vector3(moveDirection.x, 0, moveDirection.y), ForceMode.Impulse);
        }
    }
}

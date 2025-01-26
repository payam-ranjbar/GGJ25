using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    [Header("Bubble Variables")]
    public float maxBubbleSize;
    public float blowRate;
    public float startLungFullness;
    public float riseRate;
    public float bubbleScaleFactor;
    public float bubbleStartSize;

    [Header("Movement Variables")]
    public float airMoveSpeed;
    public float groundMoveSpeed;
    public float gravity;
    public float groundDrag;
    public float airDrag;

    [Header("References")]
    public Rigidbody rb;
    public PlayerBubble bubble;
    
    // Private state variables
    public Vector2 moveDirection;
    public bool blow;
    //private float currentBubbleFullness = 0;
    private float currentLungFullness;
    public bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        moveDirection = new Vector2(0, 0);
        currentLungFullness = startLungFullness;
        rb.drag = groundDrag;
    }

    // Update is called once per frame
    void Update()
    {
        var dt = Time.deltaTime;
    }

    private void FixedUpdate()
    {
        var dt = Time.fixedDeltaTime;

        if (!isGrounded)
        {
            rb.drag = airDrag;
        }
        else
        {
            rb.drag = groundDrag;
        }
        var force = new Vector3(moveDirection.x, 0, moveDirection.y);
        if (!isGrounded)
        {
            force *= airMoveSpeed;
        }
        else
        {
            force *= groundMoveSpeed;
        }
        rb.AddForce(force);

        if (bubble.poped == false)
        {
            var heightT = (bubble.balloonScale - 1) / (bubble.maxSize - 1);
            var targetY = Mathf.Lerp(0.0f, 15.0f, heightT) + 1;
            var heightDiff = (targetY - rb.position.y);

            var velocity = rb.velocity;
            var mag = Mathf.Abs(heightDiff);
            var dir = heightDiff > 0 ? 1.0f : -1.0f;
            velocity.y = (Mathf.Min(mag, gravity)) * dir;
            rb.velocity = velocity;
        }
        else
        {
            rb.AddForce(Vector3.down * gravity);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        if (collision.gameObject.CompareTag("Pop"))
        {

        }

    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
    }

    public void OnBlow(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            blow = true;
            currentLungFullness -= blowRate;
            if (currentLungFullness < 0.0f)
            {
                // todo: Out of breath!
                currentLungFullness = 0.0f;
            }
        }
        else if (context.canceled)
        {
            blow = false;
        }
    }
}

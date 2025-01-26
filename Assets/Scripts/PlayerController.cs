using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

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
            rb.AddForce(Vector3.down * gravity);
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

        if (blow == true && bubble.poped == false)
        {
            rb.AddForce(Vector3.up * riseRate, ForceMode.Impulse);
            blow = false;
            //velocity.y += riseRate * dt;
        }
        //if (isGrounded == false)
        //{
        //    velocity.y -= gravity * dt;
        //}
        //rb.velocity = velocity;
        //var t = (bubble.balloonScale - 1) / (bubble.maxSize - 1);
        //var targetY = Mathf.Lerp(0.0f, 5.0f, t);
        //var velocity = rb.velocity;
        //velocity.y = (targetY - rb.position.y) / dt;
        //rb.velocity = velocity;
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

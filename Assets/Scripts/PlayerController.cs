using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Bubble Variables")]
    public float maxBubbleSize;
    public float blowRate;
    public float deflateRate;
    public float startLungFullness;
    public float riseRate;
    public float bubbleScale;

    [Header("Movement Variables")]
    public float moveSpeed;
    public float gravity;
    public float moveMultiplier;
    public float groundDrag;
    public float airDrag;

    [Header("References")]
    public Rigidbody rb;
    public GameObject bubble;
    public AnimationCurve bubblePower;

    // Private state variables
    private Vector2 moveDirection;
    private float currentBubbleFullness = 0;
    private float currentLungFullness;
    private bool isGrounded;

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

    }

    private void FixedUpdate()
    {
        if (!isGrounded)
        {
            rb.drag = airDrag;
            rb.AddForce(Vector3.down * gravity);
        }
        else
        {
            rb.drag = groundDrag;
        }
        moveDirection *= Time.fixedDeltaTime;
        rb.AddForce(new Vector3(moveDirection.x, 0, moveDirection.y), ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
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
        if (context.performed)
        {
            moveDirection = context.ReadValue<Vector2>() * moveSpeed;
        }
    }

    public void OnBlow(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            rb.AddForce(Vector3.up * riseRate, ForceMode.Impulse);
            currentBubbleFullness += blowRate;
            currentLungFullness -= blowRate;
            if (currentBubbleFullness > 1.0)
            {
                // TODO: Pop!
                currentBubbleFullness = 1.0f;
            }
            if (currentLungFullness < 0.0f)
            {
                // todo: Out of breath!
                currentLungFullness = 0.0f;
            }
            var scale = Vector3.one * currentBubbleFullness * bubbleScale;
            bubble.transform.localScale = scale;
        }
        else if (context.canceled)
        {
        }
    }
}

using System.Xml;
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
    public float bubbleScaleFactor;
    public float bubbleStartSize;

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
    private bool recentlyHit = false;

    // Start is called before the first frame update
    void Start()
    {
        moveDirection = new Vector2(0, 0);
        currentLungFullness = startLungFullness;
        rb.drag = groundDrag;
        bubble.transform.localScale = new Vector3(bubbleStartSize, bubbleStartSize, bubbleStartSize);
    }

    // Update is called once per frame
    void Update()
    {
        var dt = Time.deltaTime;
        var scale = bubble.transform.localScale - Vector3.one * deflateRate * dt;
        if (scale.x <= bubbleStartSize)
        {
            scale = new Vector3(bubbleStartSize, bubbleStartSize, bubbleStartSize);
        }
        bubble.transform.localScale = scale;
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
        rb.AddForce(moveDirection.x, 0, moveDirection.y);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            if (recentlyHit)
            {
                recentlyHit = false;
            }
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Pop"))
        {
            // Play SFX for the balloon popping
            // Play animation of the balloon popping (explode object?)
            var controller = collider.GetComponentInParent<PlayerController>();
            controller.recentlyHit = true;
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
        moveDirection = context.ReadValue<Vector2>() * moveSpeed;
    }

    public void OnBlow(InputAction.CallbackContext context)
    {
        if (context.performed && !recentlyHit)
        {
            rb.AddForce(Vector3.up * riseRate, ForceMode.Impulse);
            currentBubbleFullness += blowRate;
            currentLungFullness -= blowRate;
            if (currentBubbleFullness > maxBubbleSize)
            {
                // TODO: Pop!
                currentBubbleFullness = 1.0f;
                bubble.transform.localScale = new Vector3(bubbleStartSize, bubbleStartSize, bubbleStartSize);
            }
            if (currentLungFullness < 0.0f)
            {
                // todo: Out of breath!
                currentLungFullness = 0.0f;
            }
            var scale = bubble.transform.localScale + Vector3.one * currentBubbleFullness * bubbleScaleFactor;
            bubble.transform.localScale = scale;
        }
    }
}

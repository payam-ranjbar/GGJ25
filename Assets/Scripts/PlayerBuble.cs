using UnityEngine;

public class PlayerBubble : MonoBehaviour
{
    public float deflateRate;
    public float inflateRate;
    public float maxSize;
    public float groundLoopDuration;

    public PlayerController player;

    public Vector3 iniScale = Vector3.zero;
    private Vector3 iniPos = Vector3.zero;
    private Vector3 targetScale = Vector3.zero;

    public bool popped = false;

    public float balloonScale => transform.localScale.x / iniScale.x;

    public void OnValidate()
    {
        if (player == null)
        {
            player = GetComponentInParent<PlayerController>();
        }
    }

    public void Awake()
    {
        OnValidate();
        iniPos = transform.localPosition;
        iniScale = transform.localScale;
        targetScale = transform.localScale;
    }

    private float groundTime = 0.0f;


    public void Update()
    {
        var deltaTime = Time.deltaTime;
        if (player.isGrounded == true)
        {
            popped = false;
            groundTime += deltaTime;
            float t = Mathf.PingPong(groundTime / groundLoopDuration, 1.0f);
            targetScale = Vector3.Lerp(iniScale, iniScale * 2.0f, t);
        }
        else if (popped == false)
        {
            if (player.blow == true)
            {
                targetScale += inflateRate * Vector3.one * deltaTime;
            }
            else if (targetScale.x > iniScale.x)
            {
                targetScale -= deflateRate * Vector3.one * deltaTime;
            }
        }

        if (popped == false)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, targetScale, 0.25f * deltaTime);
            transform.localPosition = iniPos + transform.forward * (iniScale.x - transform.localScale.x) * 0.5f;

            if (targetScale.x > maxSize * iniScale.x)
            {
                Pop();
            }
        }
    }

    public void Pop()
    {
        popped = true;
        transform.localScale = Vector3.zero;
    }
}
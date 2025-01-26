using UnityEngine;

public class BirdControls : MonoBehaviour
{
    [Header("State variables")]
    public float speed;
    public bool isFlying;
    public Vector3 moveDirection;

    void Start()
    {
        moveDirection = moveDirection.normalized;
        transform.rotation = Quaternion.LookRotation(-moveDirection);
    }

    void Update()
    {
        if (isFlying)
        {
            transform.position += moveDirection * speed * Time.deltaTime;
        }
    }
}

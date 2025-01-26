using UnityEngine;

public class PlayerAniamtion : MonoBehaviour
{

    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _visuals;
    [SerializeField] private float _rotationSpeed;

    private bool _isGrounded = false;

    private void OnValidate()
    {
        if (_animator == null)
        {
            _animator = GetComponentInChildren<Animator>();
        }

        if (_rigidbody == null)
        {
            _rigidbody = GetComponentInChildren<Rigidbody>();
        }

        if (_visuals == null)
        {
            _visuals = _rigidbody.transform.GetChild(0);
        }
    }

    private void Awake()
    {
        OnValidate();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGrounded = false;
        }
    }

    private void Update()
    {
        var deltaTime = Time.deltaTime;

        _animator.SetBool("grounded", _isGrounded);
        
        var hVel = _rigidbody.velocity;
        hVel.y = 0.0f;
        var hSpeed = hVel.magnitude;

        _animator.SetFloat("speed", hSpeed);

        if (hSpeed > 1e-3f)
        {
            var velDir = hSpeed > 0 ? hVel / hSpeed : Vector3.zero;
            var dot = Vector3.Dot(velDir, _visuals.forward);
            var newLookDir = -Vector3.forward;
            if (dot > -0.25f)
            {
                newLookDir = velDir;
            }
            _visuals.rotation = Quaternion.RotateTowards(
                _visuals.rotation, 
                Quaternion.LookRotation(newLookDir), 
                deltaTime * _rotationSpeed
            );
        }
    }

}
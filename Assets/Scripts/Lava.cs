using System.Collections;
using UnityEngine;

public class Lava : MonoBehaviour
{
    [SerializeField] private Vector2 _minMaxHeight;
    [SerializeField] private float _duration = 10.0f;
    [SerializeField] private PlayerSpawnManager _spawnManager;
    [SerializeField] private GameObject _playerDeathParticle;
    
    private float _time = 0.0f;

    private void Awake()
    {
        enabled = false;
    }


    public bool rise;

    private void OnValidate()
    {
        Reset();
        if (_spawnManager == null)
        {
            _spawnManager = FindObjectOfType<PlayerSpawnManager>();
        }
    }

    private void Reset()
    {
        var position = transform.position;
        position.y = _minMaxHeight.x;
        transform.position = position;
    }

    public void Activate()
    {

        StartCoroutine(Rise());
        enabled = true;
        AudioManager.Instance.PlayLava();
        GameManager.Instance.RiseLava();
        Reset();
    }

    IEnumerator Rise()
    {
        yield return new WaitForSeconds(2f);
        
        rise = true;
        enabled = true;
    }

    public void Update()
    {
        if(!rise) return;
        var dt = Time.deltaTime;
        _time += dt;
        var t = Mathf.PingPong(_time / _duration, 1.0f);
        var height = Mathf.Lerp(_minMaxHeight.x, _minMaxHeight.y, t);
        var position = transform.position;
        position.y = height;
        transform.position = position;
    }

    private void OnTriggerEnter(Collider collider)
    {
        var player = collider.GetComponentInParent<PlayerController>();
        if (player != null)
        {
            player.Die();
            _spawnManager.OnPlayerDeath(player.index);
            var go = GameObject.Instantiate(_playerDeathParticle, player.GetComponent<Rigidbody>().position, Quaternion.identity, null);
            var particle = go.GetComponent<ParticleSystem>();
            particle.Stop();
            particle.Play();
        }
    }
}
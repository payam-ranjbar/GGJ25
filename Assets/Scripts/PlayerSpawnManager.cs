using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawnManager : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private float _gameDelayStart = 3.0f;
    [SerializeField] private Lava _lava = null;
    //[SerializeField] private Vector2 _eventDelayMinMax;
    [SerializeField] private BirdHazardManager _birdHazard;

    public GameObject birdHazard;

    private int _index = 0;
    private int _playerCount = 0;
    private bool _gameStarted = false;
    private bool _gameEnded = false;
    //private float _time = 0.0f;

    private void OnValidate()
    {
        if (_lava == null)
        {
            _lava = FindObjectOfType<Lava>();
        }

        if (_birdHazard == null)
        {
            _birdHazard = FindObjectOfType<BirdHazardManager>();
        }
    }

    public void OnPlayerJoined(PlayerInput player)
    {
        var rigidbody = player.GetComponentInChildren<Rigidbody>();
        var interpolation = rigidbody.interpolation;
        rigidbody.position = _spawnPoints[_index].position;
        rigidbody.transform.position = _spawnPoints[_index].position;
        rigidbody.interpolation = interpolation;

        var playerController = player.GetComponentInChildren<PlayerController>();
        playerController.SetIndex(_playerCount);
        var cameraTarget = GameManager.Instance.GetPlayerCameraTarget(_playerCount);
        if(cameraTarget != null)
            cameraTarget.transform.parent = playerController.transform;

        ++_playerCount;
        _index = (_index + 1) % _spawnPoints.Length;

        if (_playerCount >= 2 && _gameStarted == false)
        {
            GetComponent<PlayerInputManager>().DisableJoining();
            _gameStarted = true;
            PlayerEventHandler.Instance.TriggerPlayersJoin();
            // 2 players are joined -> 5 second
            Invoke("OnGameBegin", _gameDelayStart);
        }
    }

    private void OnGameBegin()
    {
        _lava.Activate();
        //_time = Random.Range(_eventDelayMinMax.x, _eventDelayMinMax.y);
        _birdHazard.started = true;
    }



    public void OnPlayerDeath(int index)
    {
        if (_gameEnded == true)
        {
            return;
        }
        _gameEnded = true;
        Debug.Log($"Player died {index}");
        PlayerEventHandler.Instance.TriggerDeath();
        _birdHazard.started = false;
        if (index == 1)
        {
            GameManager.Instance.PlayerOneWin();
        }
        else
        {
            GameManager.Instance.PlayerTwoWin();
        }
    }

    //private void Update()
    //{
    //    if (_time <= 0.0f)
    //    {
    //        _time = Random.Range(_eventDelayMinMax.x, _eventDelayMinMax.y);
    //    }
    //    _time -= Time.deltaTime;
    //}

}
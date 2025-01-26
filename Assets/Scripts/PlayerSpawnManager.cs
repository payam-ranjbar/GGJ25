using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawnManager : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private float _gameDelayStart = 3.0f;
    [SerializeField] private Lava _lava = null;

    private int _index = 0;
    private int _playerCount = 0;
    private bool _gameStarted = false;
    private bool _gameEnded = false;

    private void OnValidate()
    {
        if (_lava == null)
        {
            _lava = FindObjectOfType<Lava>();
        }
    }

    public void OnPlayerJoined(PlayerInput player) {
        var rigidbody = player.GetComponentInChildren<Rigidbody>();
        var interpolation = rigidbody.interpolation;
        rigidbody.position = _spawnPoints[_index].position;
        rigidbody.transform.position = _spawnPoints[_index].position;
        rigidbody.interpolation = interpolation;

        var playerController = player.GetComponentInChildren<PlayerController>();
        playerController.SetIndex(_playerCount);
        //var cameraTarget = GameManager.Instance.GetPlayerCameraTarget(_playerCount);
        //cameraTarget.transform.parent = playerController.transform;

        ++_playerCount;
        _index = (_index + 1) % _spawnPoints.Length;
        
        if (_playerCount >= 2 && _gameStarted == false)
        {
            _gameStarted = true;
            PlayerEventHandler.Instance.TriggerPlayersJoin();
            // 2 players are joined -> 5 second
            Invoke("OnGameBegin", _gameDelayStart);
        }
    }

    private void OnGameBegin()
    {
        _lava.Activate();
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
        if (index == 1)
        {
            GameManager.Instance.PlayerOneWin();
        }
        else
        {
            GameManager.Instance.PlayerTwoWin();
        }
    }

}
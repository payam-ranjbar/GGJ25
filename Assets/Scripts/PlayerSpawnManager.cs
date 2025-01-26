using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawnManager : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private float _gameDelayStart = 3.0f;
    [SerializeField] private Lava _lava = null;

    private int _index = 0;
    private int _playerCount = 0;

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

        ++_playerCount;
        _index = (_index + 1) % _spawnPoints.Length;

        // 2 players are joined -> 5 second
        Invoke("OnGameBegin", _gameDelayStart);
    }

    private void OnGameBegin()
    {
        _lava.Activate();
    }

}
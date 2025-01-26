using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnPoints;

    private int _index = 0;
    private int _playerCount = 0;

    public void OnPlayerJoined(PlayerInput player) {
        var rigidbody = player.GetComponentInChildren<Rigidbody>();
        var interpolation = rigidbody.interpolation;
        rigidbody.position = _spawnPoints[_index].position;
        rigidbody.transform.position = _spawnPoints[_index].position;
        rigidbody.interpolation = interpolation;

        ++_playerCount;
        _index = (_index + 1) % _spawnPoints.Length;

        // 2 players are joined -> 5 second
    }

}
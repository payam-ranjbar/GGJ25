using System;
using UnityEngine;

public class PlayerEventHandler : MonoBehaviour
{
    public static PlayerEventHandler Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public event Action OnJump;
    public event Action OnAttack;
    public event Action OnInteract;

    public void TriggerJump()
    {
        OnJump?.Invoke();
    }

    public void TriggerAttack()
    {
        OnAttack?.Invoke();
    }

    public void TriggerInteract()
    {
        OnInteract?.Invoke();
    }
}
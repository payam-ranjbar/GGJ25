using System;
using UnityEngine;
using UnityEngine.Events;

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

    public event Action OnDeath;
    public event Action OnBubblePop;
    public event Action OnBirdCollide;
    public event Action OnThunderCollide;
    public event Action OnStormCollide;
    public event Action OnPlayerBlow;
    public event Action OnPlayerLand;
    public event Action OnBubbleReachMax;
    public event Action OnBabbleDeflate;
    public event Action OnNotification;
    public event Action OnBirdsSpawn;
    public event Action OnThunderSpawn;
    public event Action OnStormSpawn;
    public event Action OnPlayersCollide;
    public event Action OnPlayerJoin;

    [SerializeField] private UnityEvent deathEvent;
    [SerializeField] private UnityEvent bubblePopEvent;
    [SerializeField] private UnityEvent birdCollideEvent;
    [SerializeField] private UnityEvent thunderCollideEvent;
    [SerializeField] private UnityEvent stormCollideEvent;
    [SerializeField] private UnityEvent playerBlowEvent;
    [SerializeField] private UnityEvent playerLandEvent;
    [SerializeField] private UnityEvent bubbleReachMaxEvent;
    [SerializeField] private UnityEvent bubbleDeflateEvent;
    [SerializeField] private UnityEvent notificationEvent;
    [SerializeField] private UnityEvent birdsSpawnEvent;
    [SerializeField] private UnityEvent thunderSpawnEvent;
    [SerializeField] private UnityEvent stormSpawnEvent;
    [SerializeField] private UnityEvent playersCollideEvent;

    public void TriggerDeath() { OnDeath?.Invoke(); deathEvent?.Invoke(); }
    public void TriggerBubblePop() { OnBubblePop?.Invoke(); bubblePopEvent?.Invoke(); }
    public void TriggerBirdCollide() { OnBirdCollide?.Invoke(); birdCollideEvent?.Invoke(); }
    public void TriggerThunderCollide() { OnThunderCollide?.Invoke(); thunderCollideEvent?.Invoke(); }
    public void TriggerStormCollide() { OnStormCollide?.Invoke(); stormCollideEvent?.Invoke(); }
    public void TriggerPlayerBlow() { OnPlayerBlow?.Invoke(); playerBlowEvent?.Invoke(); }
    public void TriggerPlayerLand() { OnPlayerLand?.Invoke(); playerLandEvent?.Invoke(); }
    public void TriggerBubbleReachMax() { OnBubbleReachMax?.Invoke(); bubbleReachMaxEvent?.Invoke(); }
    public void TriggerBubbleDeflate() { OnBabbleDeflate?.Invoke(); bubbleDeflateEvent?.Invoke(); }
    public void TriggerNotification() { OnNotification?.Invoke(); notificationEvent?.Invoke(); }
    public void TriggerBirdsSpawn() { OnBirdsSpawn?.Invoke(); birdsSpawnEvent?.Invoke(); }
    public void TriggerThunderSpawn() { OnThunderSpawn?.Invoke(); thunderSpawnEvent?.Invoke(); }
    public void TriggerStormSpawn() { OnStormSpawn?.Invoke(); stormSpawnEvent?.Invoke(); }
    public void TriggerPlayersCollide() { OnPlayersCollide?.Invoke(); playersCollideEvent?.Invoke(); }
    public void TriggerPlayersJoin() { OnPlayerJoin?.Invoke(); }
}

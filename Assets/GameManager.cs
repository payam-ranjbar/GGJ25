using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject playerOneCamTarget;
    [SerializeField] private GameObject playerTwoCamTarget;
    [SerializeField] private CinemachineImpulseSource impulseSource;

    private int _activePlayers;

    private bool _gameStarted;
    
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

    private void OnEnable()
    {
    }

    //private void OnDisable()
    //{
    //    PlayerEventHandler.Instance.OnPlayerJoin -= AddPlayer;
    //}

    private void Start()
    {
        PlayerEventHandler.Instance.OnPlayerJoin += AddPlayer;
        UIManager.Instance.ShowStartScreen();
    }

    public GameObject GetPlayerCameraTarget(int i)
    {
        return i <= 1 ? playerOneCamTarget : playerTwoCamTarget;
    }

    public void PlayerOneWin()
    {
        GameOver("Player One");
        AudioManager.Instance.PlayWinPlayerOne();
    }
    public void PlayerTwoWin()
    {
        GameOver("Player Two");
        AudioManager.Instance.PlayWinPlayerTwo();
    }


    public void ShowThunderNotification()
    {
        AudioManager.Instance.PlayNarratorThunder();
        UIManager.Instance.ShowNotif();
    }
    public void ShowStormNotification()
    {
        AudioManager.Instance.PlayNarratorStorm();
        UIManager.Instance.ShowNotif();
    }
    public void ShowBirdsNotification()
    {
        AudioManager.Instance.PlayNarratorBirds();
        UIManager.Instance.ShowNotif();
    }

    private void GameOver(string playerName)
    {
        AudioManager.Instance.PlayGameEnd();
        UIManager.Instance.ShowEndGame(playerName);
    }


    public void AddPlayer()
    {
        //_activePlayers++;

        UIManager.Instance.HideStartScreen();
        
        //if (_activePlayers >= 2)
        {
            StartTimerToStartTheGame();
        }
    }

    private void StartTimerToStartTheGame()
    {
        _gameStarted = true;

        StartCoroutine(Counter());
        
    }


    private bool _counterStarted;


    private IEnumerator Counter()
    {
        if(_counterStarted) yield break;
        _counterStarted = true;
            AudioManager.Instance.PlayCounter();
        UIManager.Instance.ShowCounter();
        yield return new WaitForSeconds(1f);
        UIManager.Instance.SetCounter("2");

        yield return new WaitForSeconds(1f);
        UIManager.Instance.SetCounter("1");
        
        yield return new WaitForSeconds(1f);
        UIManager.Instance.SetCounter("Go!");
        yield return new WaitForSeconds(0.5f);
        UIManager.Instance.HideCounter();
        _counterStarted = false;
        

        RiseLava();
        yield return new WaitForSeconds(0.3f);
        AudioManager.Instance.PlayDing();
        UIManager.Instance.ShowPlayerHUD();
    }

    private void RiseLava()
    {
        impulseSource.GenerateImpulse(0.1f);
        AudioManager.Instance.PlayBGMSequentially();
        
        
    }
}
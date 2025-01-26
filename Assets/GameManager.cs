using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

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
}
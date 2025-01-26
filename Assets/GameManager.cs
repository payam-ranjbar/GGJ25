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


    private void GameOver(string playerName)
    {
        AudioManager.Instance.PlayGameEnd();
        UIManager.Instance.ShowEndGame(playerName);
    }
}
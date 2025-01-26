using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager: MonoBehaviour
{
    [SerializeField] private Animator playerHUDAnimator;
    [SerializeField] private Animator startScreenAnimator;
    [SerializeField] private Animator endGameAnimator;
    [SerializeField] private Animator notifAnimtor;
    [SerializeField] private TMP_Text endGameText;


    public static UIManager Instance { get; private set; }

    
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
    public void ShowPlayerHUD()
    {
        
        playerHUDAnimator.SetTrigger("Show");
    }

    public void HidePlayerHUD()
    {
        playerHUDAnimator.SetTrigger("Hide");
    }

    public void HideStartScreen()
    {
        startScreenAnimator.SetTrigger("Hide");
    }

    public void ShowStartScreen()
    {
        startScreenAnimator.SetTrigger("Show");
    }


    public void ShowEndGame(string winnerName)
    {
        endGameText.text = $"{winnerName} has won!";
        endGameAnimator.SetTrigger("Show");
    }
    public void HideEndGame()
    {
        endGameAnimator.SetTrigger("Hide");
    }
    
    public void ShowNotif()
    {
        if (_notifDelay != null)
        {
            Debug.Log("notification show rejected");
            HideNotif();
            return;
        }
        AudioManager.Instance.PlayNotifDing();
        notifAnimtor.SetTrigger("Show");
        _notifDelay = StartCoroutine(Delay());


    }
    public void HideNotif()
    {
        notifAnimtor.SetTrigger("Hide");
        if (_notifDelay != null)
        {
            StopCoroutine(_notifDelay);
            _notifDelay = null;
        }
    }


    private Coroutine _notifDelay;
    private IEnumerator Delay()
    {
        
        yield return new WaitForSeconds(2f);
        HideNotif();
        _notifDelay = null;

    }

}
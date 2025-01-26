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
    [SerializeField] private TMP_Text counterText;
    [SerializeField] private Animator counterAnimator;

    private bool _startScreenOn;

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
        if(!_startScreenOn) return;
        _startScreenOn = false;
        startScreenAnimator.SetTrigger("Hide");
    }

    public void ShowStartScreen()
    {
        if(_startScreenOn) return;
        startScreenAnimator.SetTrigger("Show");
        _startScreenOn = true;
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

    private bool notifCalled;
    public void ShowNotif()
    {
        if(notifCalled) return;

        notifCalled = true;
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
        notifCalled = false;

    }

    public void ShowCounter()
    {
        counterAnimator.SetTrigger("Show");
    }
    public void HideCounter()
    {
        counterAnimator.SetTrigger("Hide");
    }    
    
    public void SetCounter(string text)
    {
        counterText.text = text;
        counterAnimator.SetTrigger("Wiggle");
    }


    private Coroutine _notifDelay;
    private IEnumerator Delay()
    {
        
        yield return new WaitForSeconds(2f);
        HideNotif();
        _notifDelay = null;

    }

}
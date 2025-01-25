using UnityEngine;

public class UIManager: MonoBehaviour
{
    [SerializeField] private Animator playerHUDAnimator;

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
}
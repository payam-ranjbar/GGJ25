using UnityEngine;

public class UIManager: MonoBehaviour
{
    [SerializeField] private Animator playerHUDAnimator;



    public void ShowPlayerHUD()
    {
        
        playerHUDAnimator.SetTrigger("Show");
    }

    public void HidePlayerHUD()
    {
        playerHUDAnimator.SetTrigger("Hide");
    }
}
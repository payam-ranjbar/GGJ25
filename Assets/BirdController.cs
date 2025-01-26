using UnityEngine;

public class BirdController : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Pop"))
        {
            // Play SFX for the balloon popping
            // Play animation of the balloon popping (explode object?)
            Debug.Log("The bird hit a player");
            var controller = collider.GetComponentInParent<PlayerController>();
            controller.recentlyHit = true;
            controller.bubble.Pop();
        }
    }
}

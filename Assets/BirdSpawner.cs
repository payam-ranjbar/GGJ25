using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    [Header("State Variables")]
    public int numBirds;
    public float minSpeed;
    public float maxSpeed;
    public float deletionTime;
    public float timeFromNotificationToBirds;
    public bool startEvent = false;

    public float[] xBounds;
    public float[] yBounds;
    public float zPlane;
    public Vector3 normal; // The direction that the birds should move in (normal)
    public Vector2 center;
    public float width;
    public float height;
    public float minWidth;
    public float maxWidth;
    public float minHeight;
    public float maxHeight;

    [Header("References")]
    public GameObject birdReference;

    private bool birdsFlying = false;
    private float currentTime = 0.0f;
    private List<GameObject> birds = new List<GameObject>();
    private bool audioPlayed = false;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (startEvent)
        {
            // If we started another event while the birds are still flying the timers can get messed up
            // This fixes the issue
            if (birdsFlying)
            {
                birdsFlying = false;
                currentTime = 0.0f;
                foreach (var bird in birds)
                {
                    GameObject.Destroy(bird);
                }
                birds.Clear();
            }
            if (!audioPlayed)
            {
                GameManager.Instance.ShowBirdsNotification();
                audioPlayed = true;
                currentTime = 0.0f;
            }
            currentTime += Time.deltaTime;
            if (currentTime >= timeFromNotificationToBirds)
            {
                currentTime = 0.0f;
                ChooseSpawnLocation();
                SpawnBirds();
                birdsFlying = true;
                startEvent = false;
                audioPlayed = false;
            }
        }
        if (birdsFlying)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= deletionTime)
            {
                if (birds != null)
                {
                    foreach (var bird in birds)
                    {
                        GameObject.Destroy(bird);
                    }

                    birds.Clear();
                    currentTime = 0.0f;
                    birdsFlying = false;
                }
            }
        }
    }

    private void ChooseSpawnLocation()
    {
        center = new Vector2(Random.Range(xBounds[0], xBounds[1]), Random.Range(yBounds[0], yBounds[1]));
        width = Random.Range(minWidth, maxWidth);
        height = Random.Range(minHeight, maxHeight);
    }

    private void SpawnBirds()
    {
        for (int i = 0; i < numBirds; i++)
        {
            Debug.Log("Spawning a bird");
            Vector3 position = new Vector3(Random.Range(center.x - width / 2, center.y + width / 2), Random.Range(center.y - height / 2, center.y + height / 2), zPlane);
            var bird = Instantiate(birdReference, position, Quaternion.identity);
            var birdControls = bird.GetComponent<BirdControls>();
            birdControls.speed = Random.Range(minSpeed, maxSpeed);
            birdControls.isFlying = true;
            birdControls.moveDirection = normal;
        }
    }
}

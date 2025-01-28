using System;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BirdSpawner : MonoBehaviour
{
    [Header("State Variables")]
    public float minSpawnInterval =  5;
    public float maxSpawnInterval = 7;
    public float maxSpeed;
    public float spawnInterval;
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

            if ((currentTime + timeFromNotificationToBirds >= spawnInterval))
            {
                if (!audioPlayed)
                {
                    if (GameManager.Instance != null)
                        GameManager.Instance.ShowBirdsNotification();
                    if (AudioManager.Instance != null)
                        AudioManager.Instance.PlayBirdsSpawn();
                    audioPlayed = true;
                    Debug.Log("bird notif played");
                }
            }

            if (currentTime  >= spawnInterval)
            {
                currentTime = 0.0f;
                spawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
                ChooseSpawnLocation();
                SpawnBirds();
                birdsFlying = true;
                audioPlayed = false;
                return;
            }
            currentTime += Time.deltaTime;

        }
        
    }

    private void ChooseSpawnLocation()
    {
        center = new Vector2(Random.Range(xBounds[0], xBounds[1]), Random.Range(yBounds[0], yBounds[1]));
        width = Random.Range(minWidth, maxWidth);
        height = Random.Range(minHeight, maxHeight);
    }

    [SerializeField] private FlockManager flock;
    private void SpawnBirds()
    {
        // for (int i = 0; i < numBirds; i++)
        // {
        //     Debug.Log("Spawning a bird");
        //     Vector3 position = new Vector3(Random.Range(center.x - width / 2, center.y + width / 2), Random.Range(center.y - height / 2, center.y + height / 2), zPlane);
        //     var bird = Instantiate(birdReference, position, Quaternion.identity);
        //     var birdControls = bird.GetComponent<BirdControls>();
        //     birdControls.speed = Random.Range(minSpeed, maxSpeed);
        //     birdControls.isFlying = true;
        //     birdControls.moveDirection = normal;
        // }
        //
        Vector3 position = new Vector3(transform.position.x + width, transform.position.y + height, transform.position.z);
         var newFlock = Instantiate(flock, position, Quaternion.identity);
         newFlock.isFlaying = true;
         newFlock.flyDirection = normal.normalized;

         Destroy(newFlock.gameObject, 10f);

         for (int i = 0; i < newFlock.transform.childCount; i++)
         {
             Destroy(newFlock.transform.GetChild(i).gameObject, 9.8f);
         }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, normal.normalized * maxSpeed);
        Gizmos.DrawWireCube(transform.position, new Vector3(maxWidth, maxHeight, 0));
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 0));
    }
}

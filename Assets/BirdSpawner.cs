using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    [Header("State Variables")]
    public int numBirds;
    public float minSpeed;
    public float maxSpeed;
    public float[] xBounds;
    public float[] yBounds;
    public float[] zBounds;
    public Vector3 normal;
    public float deletionTime;
    public float timeFromNotificationToBirds;
    public bool startEvent = false;

    [Header("References")]
    public GameObject birdReference;

    private bool birdsFlying = false;
    private float currentTime = 0.0f;
    private List<GameObject> birds;
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

    private void SpawnBirds()
    {
        for (int i = 0; i < numBirds; i++)
        {
            Debug.Log("Spawning a bird");
            Vector3 direction = Random.onUnitSphere;

            var cos = Vector3.Dot(normal, direction);
            if (cos < 0.0f)
            {
                direction = -direction;
            }
            Vector3 position = new Vector3(Random.Range(xBounds[0], xBounds[1]), Random.Range(yBounds[0], yBounds[1]), Random.Range(zBounds[0], zBounds[1]));
            var bird = Instantiate(birdReference, position, Quaternion.identity);
            var birdControls = bird.GetComponent<BirdControls>();
            birdControls.speed = Random.Range(minSpeed, maxSpeed);
            birdControls.isFlying = true;
            birdControls.moveDirection = direction;
        }
    }
}

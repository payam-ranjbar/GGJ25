using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdHazardManager : MonoBehaviour
{
    [Header("References")]
    public BirdSpawner[] spawners;

    [Header("Variables")]
    public float minSpeed;
    public float maxSpeed;
    public float minWaveTime;
    public float maxWaveTime;
    public bool started = false;

    private float currentTime = 0.0f;
    private float timeExpiresTime;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var spawner in spawners)
        {
            spawner.minSpeed = minSpeed;
            spawner.maxSpeed = maxSpeed;
        }

        timeExpiresTime = Random.Range(minWaveTime, maxWaveTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (started)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= timeExpiresTime)
            {
                timeExpiresTime = Random.Range(minWaveTime, maxWaveTime);
                currentTime = 0.0f;
                foreach (var spawner in spawners)
                {
                    spawner.startEvent = true;
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdHazardManager : MonoBehaviour
{
    [Header("References")]
    public BirdSpawner[] spawners;

    [Header("Variables")]
    public bool spawnBirds = false;
    public float minSpeed;
    public float maxSpeed;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var spawner in spawners)
        {
            spawner.minSpeed = minSpeed;
            spawner.maxSpeed = maxSpeed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnBirds)
        {
            spawnBirds = false;
            foreach (var spawner in spawners)
            {
                spawner.startEvent = true;
            }
        }
    }
}

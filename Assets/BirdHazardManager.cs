using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdHazardManager : MonoBehaviour
{
    [Header("References")]
    public BirdSpawner[] spawners;
    public bool spawnBirds = false;

    // Start is called before the first frame update
    void Start()
    {
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

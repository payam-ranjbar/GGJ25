using UnityEngine;

public static class RandomUtils
{
    public static int GetRandomIntInRange(int min, int max)
    {
        return Random.Range(min, max);
    }
        
}
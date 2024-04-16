using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperUtils : MonoBehaviour
{
    public static int GetRandomNumber(int limit)
    {
        return UnityEngine.Random.Range(0, int.MaxValue) % (limit + 1);
    }

    public static int GetRandomNumber(int start, int limit)
    {
        return UnityEngine.Random.Range(0, int.MaxValue) % (limit - start + 1) + start;
    }
}

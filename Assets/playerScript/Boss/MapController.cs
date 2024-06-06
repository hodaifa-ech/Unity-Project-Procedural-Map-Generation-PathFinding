using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [SerializeField]
    public int corridorLength1 = 1, corridorCount1 = 25;
    [SerializeField]
    [Range(0.1f, 1)]
    public float roomPercent1 = 1f;
    void Awake()
    {
        CorridorFirstDundeonGenerator.corridorLength = corridorLength1;
        CorridorFirstDundeonGenerator.corridorCount = corridorCount1;
        CorridorFirstDundeonGenerator.roomPercent = roomPercent1; 
    }

    
}

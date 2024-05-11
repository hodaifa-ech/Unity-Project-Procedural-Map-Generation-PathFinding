using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractDungeonGenerator : MonoBehaviour
{
    [SerializeField]
    protected Tailmapvisualizer tilemapbvisualizer = null;
    [SerializeField]
    protected Vector2Int startPosition = Vector2Int.zero;

    public void GenerateDunguon()
    {
        tilemapbvisualizer.Clear();
        RunProceduralGeneration();

    }

    public abstract void RunProceduralGeneration();
}

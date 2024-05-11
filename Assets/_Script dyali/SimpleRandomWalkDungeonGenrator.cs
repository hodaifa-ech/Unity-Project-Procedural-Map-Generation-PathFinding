using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class SimpleRandomWalkDungeonGenrator : AbstractDungeonGenerator
{

    [SerializeField]
    protected SimpleRandomWalkSO randomWalkParameters;



    public override void RunProceduralGeneration()
    {
        HashSet<Vector2Int> floorPositions = RunRandomWalk(randomWalkParameters, startPosition);
        /*foreach(var position in floorPositions)
        {
            Debug.Log(position); 
        }*/
        tilemapbvisualizer.Clear();
        tilemapbvisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tilemapbvisualizer);
    }

    protected HashSet<Vector2Int> RunRandomWalk(SimpleRandomWalkSO parameters, Vector2Int position)
    {
        var currentPosition = position;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        for (int i = 0; i < parameters.iteration; i++)
        {
            var path = ProcedurakGenerationAlgorithme.SimpleRandomWalk(currentPosition, parameters.walklength);
            floorPositions.UnionWith(path);
            if (parameters.startRandomlyEachIteration)
            {
                currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
            }
        }
        return floorPositions;
    }


}

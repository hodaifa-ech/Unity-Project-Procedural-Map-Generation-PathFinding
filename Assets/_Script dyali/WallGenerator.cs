using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator
{
   public static void CreateWalls(HashSet<Vector2Int> floorPositions, Tailmapvisualizer tilemapvisualizer)
    {
        var basicWallPositions = FindWallsInDirections(floorPositions, Direction2d.cardinalDirectionsList);
        var cornerWallPositions = FindWallsInDirections(floorPositions, Direction2d.diagonalDirectionsList);
        CreateBasicWall(tilemapvisualizer, basicWallPositions, floorPositions);
        CreateCornerWalls(tilemapvisualizer, cornerWallPositions, floorPositions);
    }

    private static void CreateCornerWalls(Tailmapvisualizer tilemapvisualizer, HashSet<Vector2Int> cornerWallPositions, HashSet<Vector2Int> floorPositions)
    {
        foreach (var position in cornerWallPositions) {
            string neighboursBinaryType = "";
            foreach (var direction in Direction2d.eightDirectionList)
            {
                var neighbourPosition = position + direction;
                if(floorPositions.Contains(neighbourPosition))
                {
                    neighboursBinaryType += "1";
                }
                else
                {
                    neighboursBinaryType += "0";
                }
            }
            tilemapvisualizer.PaintSingleCornerWall(position, neighboursBinaryType);


            tilemapvisualizer.Item2(position, neighboursBinaryType, 2);


        }
    }

    private static void CreateBasicWall(Tailmapvisualizer tilemapvisualizer, HashSet<Vector2Int> basicWallPositions, HashSet<Vector2Int> floorPositions)
    {

        tilemapvisualizer.DestroyAllItems(); // supprimer old items for non duplicate it 

        foreach (var position in basicWallPositions)
        {
            string neighboursBinaryType = "";
            foreach (var direction in Direction2d.cardinalDirectionsList)
            {
                var neighbourPosition = position + direction;
                if(floorPositions.Contains(neighbourPosition))
                {
                    neighboursBinaryType += "1";
                }
                else
                {
                    neighboursBinaryType += "0";
                }
            }
            tilemapvisualizer.PaintSingleBasicWall(position,neighboursBinaryType);

            //here

            int SpawnerProbability = UnityEngine.Random.Range(1,10 ); // probability to set the Item | example : int rand = UnityEngine.Random.Range(1, 10); 
            int ItemProbability = UnityEngine.Random.Range(1, 20);
            if (ItemProbability == 1)
            {
                tilemapvisualizer.Item(position, neighboursBinaryType, ItemProbability);
            }
            if (SpawnerProbability == 2)
            {
                tilemapvisualizer.Item(position, neighboursBinaryType, SpawnerProbability);
            }

        }
    }

    private static  HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorPositions, List<Vector2Int> directionsLists)
    {
       HashSet<Vector2Int> wallPositions=new HashSet<Vector2Int>();
        foreach (var position in floorPositions)
        {
            foreach (var direction in directionsLists) { 
                var neighbourPositions = position+direction;
                if(floorPositions.Contains(neighbourPositions)==false) {
                  wallPositions.Add(neighbourPositions);
                }
            
            }
        }
        return wallPositions;  
    }
}

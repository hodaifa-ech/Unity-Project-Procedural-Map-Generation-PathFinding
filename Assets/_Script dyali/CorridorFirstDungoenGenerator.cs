using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public class CorridorFirstDundeonGenerator : SimpleRandomWalkDungeonGenrator
{
    public static List<Vector2Int> roomspos;

    [SerializeField]
    private int corridorLength = 14, corridorCount = 5;
    [SerializeField]
    [Range(0.1f, 1)]
    private float roomPercent = 0.8f;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject boss;
    [SerializeField]
    private GameObject enemies;

    public List<GameObject> instantiatedEnemies = new List<GameObject>();






    public void Start()
    {
    }




    public override void RunProceduralGeneration()
    {
        CorriderFirstGeneration();
    }




    private void CorriderFirstGeneration()
    {
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        HashSet<Vector2Int> potentialRoomPositions = new HashSet<Vector2Int>();

        List<List<Vector2Int>> corridors = CreateCorridors(floorPositions, potentialRoomPositions);

        HashSet<Vector2Int> roomPostions = CreateRooms(potentialRoomPositions);

        List<Vector2Int> deadEnds = FindAllDeadEnds(floorPositions); // dead ends finder 

        CreateRoomAtDeadEnd(deadEnds, roomPostions); //add room at the dead ends position

        floorPositions.UnionWith(roomPostions);


        for (int i = 0; i < corridors.Count; i++)
        {
            //corridors[i] = IncreaseCorridorSizeByOne(corridors[i]);
            corridors[i] = IncreaseCorridorBrush3by3(corridors[i]);
            floorPositions.UnionWith(corridors[i]);
        }
        tilemapbvisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tilemapbvisualizer);
    }

    private List<Vector2Int> IncreaseCorridorBrush3by3(List<Vector2Int> corridor)
    {
        List<Vector2Int> newCorridor = new List<Vector2Int>();
        for (int i = 1; i < corridor.Count; i++)
        {
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    newCorridor.Add(corridor[i - 1] + new Vector2Int(x, y));
                }
            }
        }
        return newCorridor;
    }

    private List<Vector2Int> IncreaseCorridorSizeByOne(List<Vector2Int> corridor)
    {
        List<Vector2Int> newCorridor = new List<Vector2Int>();
        Vector2Int previousDirection = Vector2Int.zero;
        for (int i = 1; i < corridor.Count; i++)
        {
            Vector2Int directionFrameCell = corridor[i] - corridor[i - 1];
            if (previousDirection != Vector2Int.zero &&
                directionFrameCell != previousDirection)
            {
                //handle corner
                for (int x = -1; x < 2; x++)
                {
                    for (int y = -1; y < 2; y++)
                    {
                        newCorridor.Add(corridor[i - 1] + new Vector2Int(x, y));
                    }
                }
                previousDirection = directionFrameCell;
            }
            else
            {
                Vector2Int newCorridorTileOffset =
                    GetDirection90From(directionFrameCell);
                newCorridor.Add(corridor[i - 1]);
                newCorridor.Add(corridor[i - 1] + newCorridorTileOffset);
            }

        }
        return newCorridor;

    }

    private Vector2Int GetDirection90From(Vector2Int directionFrameCell)
    {
        if (directionFrameCell == Vector2Int.up)
            return Vector2Int.right;
        if (directionFrameCell == Vector2Int.right)
            return Vector2Int.down;
        if (directionFrameCell != Vector2Int.down)
            return Vector2Int.left;
        if (directionFrameCell != Vector2Int.left)
            return Vector2Int.up;
        return Vector2Int.zero;
    }

    private void CreateRoomAtDeadEnd(List<Vector2Int> deadEnds, HashSet<Vector2Int> roomFloors)
    {
        foreach (var position in deadEnds)
        {
            if (roomFloors.Contains(position) == false)
            {
                var room = RunRandomWalk(randomWalkParameters, position);
                roomFloors.UnionWith(room);

                //spawner other room (except player room )
                int rand = UnityEngine.Random.Range(0, 4);
                if ((int)(player.transform.position.magnitude) != (int)position.magnitude)
                {

                    enemies.transform.position = position + new Vector2(rand, rand);

                }

            }
        }

    }

    private List<Vector2Int> FindAllDeadEnds(HashSet<Vector2Int> floorPositions)
    {
        List<Vector2Int> deadEnds = new List<Vector2Int>();
        foreach (var position in floorPositions)
        {
            int neighboursCount = 0;
            foreach (var direction in Direction2d.cardinalDirectionsList)
            {
                if (floorPositions.Contains(position + direction))
                    neighboursCount++;
            }
            if (neighboursCount == 1)
            {
                deadEnds.Add(position);
            }
        }
        return deadEnds;
    }
    //destroy enemies list when click on generate dunguon
    public void DestroyAllEnemies()
    {
        foreach (GameObject enemy in instantiatedEnemies)
        {
            DestroyImmediate(enemy);
        }
        instantiatedEnemies.Clear(); // Clear the list after destroying enemies
    }

    private HashSet<Vector2Int> CreateRooms(HashSet<Vector2Int> potentialRoomPositions)
    {
        HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();

        int roomsToCreateCount = Mathf.RoundToInt(potentialRoomPositions.Count * roomPercent);

        List<Vector2Int> roomsToCreate = potentialRoomPositions.OrderBy(x => Guid.NewGuid()).Take(roomsToCreateCount).ToList();

        foreach (var roomPostion in roomsToCreate)
        {
            var roomFloor = RunRandomWalk(randomWalkParameters, roomPostion);
            roomPositions.UnionWith(roomFloor);
            //player.transform.position = new Vector2(roomPostion.x , roomPostion.y); // ana zedtha 

        }



        // ana zdtha 
        List<Vector2Int> deadend = FindAllDeadEnds(roomPositions);
        player.transform.position = (Vector2)roomsToCreate[roomsToCreate.Count - 1];
        DestroyAllEnemies(); // destroy last enemies when you click on generate dunguon 

        foreach (Vector2 position in deadend)
        {
            foreach (Vector2 index in roomsToCreate)
            {
                if (Vector2.Distance(position, player.transform.position) < Vector2.Distance(position, index))
                {

                    player.transform.position = index;

                }
            }
        }

        //spawner other room (except player room )
        foreach (Vector2Int room in roomsToCreate)
        {
            int rand = UnityEngine.Random.Range(0, 4);
            if ((int)(player.transform.position.magnitude) != (int)room.magnitude)
            {
                GameObject enemy = Instantiate(enemies);
                enemies.transform.position = room + new Vector2(rand + 1, rand + 1);
                enemy.transform.position = room + new Vector2(rand, rand);

                instantiatedEnemies.Add(enemy);

            }
        }

        boss.transform.position = (Vector2)roomsToCreate[0];


        float distance = Vector2.Distance(player.transform.position, boss.transform.position);
        foreach (Vector2 index in roomsToCreate)
        {
            distance = Vector2.Distance(player.transform.position, (Vector2)index);

            if (Vector2.Distance(player.transform.position, boss.transform.position) < distance)
            {
                boss.transform.position = index;
            }
        }

        // fin (ana zedtha)

        return roomPositions;
    }

    private List<List<Vector2Int>> CreateCorridors(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> potentialRoomPositions)
    {
        var currentPosition = startPosition;
        potentialRoomPositions.Add(currentPosition);
        List<List<Vector2Int>> corridors = new List<List<Vector2Int>>();
        for (var i = 0; i < corridorCount; i++)
        {
            var corridor = ProcedurakGenerationAlgorithme.RandomWalkCorridor(currentPosition, corridorLength);
            corridors.Add(corridor);
            currentPosition = corridor[corridor.Count - 1];
            potentialRoomPositions.Add(currentPosition);
            floorPositions.UnionWith(corridor);
        }

        return corridors;
    }
}

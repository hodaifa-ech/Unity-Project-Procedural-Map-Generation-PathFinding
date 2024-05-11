using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public static class ProcedurakGenerationAlgorithme
{
   
    public static HashSet<Vector2Int> SimpleRandomWalk(Vector2Int startPosition, int Walklength)
    {
        HashSet<Vector2Int> path= new HashSet<Vector2Int>();
        path.Add(startPosition);
        var previousposition = startPosition;
        for (int i = 0; i < Walklength; i++) {
            var newposition = previousposition+Direction2d.GetRandomCardinaldirection();
            path.Add(newposition);
            previousposition = newposition;

        }
        return path;
    }
    public static List<Vector2Int> RandomWalkCorridor(Vector2Int startPosition, int corridorlength)
    {
        List<Vector2Int> corridor = new List<Vector2Int>();
       var direction = Direction2d.GetRandomCardinaldirection();
        var currentPosition= startPosition;
        corridor.Add(currentPosition);
        for (int i = 0; i < corridorlength; i++)
        {
            currentPosition += direction;
            corridor.Add(currentPosition);
        }
        return corridor;
    }
    public static List<BoundsInt> BinarySpacePartitioning(BoundsInt zpaceToSplit, int minWidth,int minHeight)
    {
        Queue<BoundsInt> roomsQues= new Queue<BoundsInt>();
        List<BoundsInt> roomList= new List<BoundsInt>();
        roomsQues.Enqueue(zpaceToSplit);
        while(roomsQues.Count > 0)
        {
            var room =roomsQues.Dequeue();
            if(room.size.y>= minHeight &&room.size.x>=minWidth)
            {
                if(Random.value < 0.5f)
                {
                    if (room.size.y >= minHeight * 2)
                    {
                        SplitHorizontaly(minHeight,roomsQues,room);
                    }
                    else if(room.size.x >= minWidth * 2)
                    {
                        SplitVertically(minWidth, roomsQues, room);
                    }
                    else if( room.size.x >= minWidth && room.size.y >= minHeight)
                    {
                        roomList.Add(room);
                    }
                }
                else
                {
                  
                     if (room.size.x >= minWidth * 2)
                    {
                        SplitVertically(minWidth, roomsQues, room) ;
                    }
                    else if (room.size.y >= minHeight * 2)
                    {
                        SplitHorizontaly(minHeight, roomsQues, room);
                    }
                    else if (room.size.x >= minWidth && room.size.y >= minHeight)
                    {
                        roomList.Add(room);
                    }
                }
            }
        }
        return roomList;
    }

    private static void SplitVertically(int minWidth,  Queue<BoundsInt> roomsQues, BoundsInt room)
    {
        var xSplit = Random.Range(1,room.size.x);
        BoundsInt room1 = new BoundsInt(room.min,new Vector3Int(xSplit,room.size.y,room.size.z));
        BoundsInt room2 = new BoundsInt( new Vector3Int(room.min.x+xSplit,room.min.y,room.min.z),
            new Vector3Int(room.size.x-xSplit,room.size.y,room.size.z));
        roomsQues.Enqueue(room1); 
        roomsQues.Enqueue(room2);
    }

    private static void SplitHorizontaly( int minWidth, Queue<BoundsInt> roomsQues, BoundsInt room)
    {
        var ySplit = Random.Range(1,room.size.y);
        BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(room.size.x, ySplit, room.size.z));
        BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x, room.min.y+ySplit, room.min.z),
            new Vector3Int(room.size.x, room.size.y-ySplit, room.size.z));
        roomsQues.Enqueue(room1);
        roomsQues.Enqueue(room2);
    }
}
public static class Direction2d
{
    public static List<Vector2Int> cardinalDirectionsList = new List<Vector2Int>
    {
        new Vector2Int(0,1),//UP
         new Vector2Int(1,0),//right
        new Vector2Int(0,-1),//down
        new Vector2Int(-1,0)//left

    };
    public static List<Vector2Int> diagonalDirectionsList = new List<Vector2Int>
    {
        new Vector2Int(1,1),//UP_right
         new Vector2Int(1,-1),//right-down
        new Vector2Int(-1,-1),//down-left
        new Vector2Int(-1,1)//left-up

    };
    public static List<Vector2Int> eightDirectionList = new List<Vector2Int>
    {
        new Vector2Int(0,1),//UP
        new Vector2Int(1,1),//UP_right
         new Vector2Int(1,0),//right
         new Vector2Int(1,-1),//right-down
        new Vector2Int(0,-1),//down
        new Vector2Int(-1,-1),//down-left
        new Vector2Int(-1,0),//left
        new Vector2Int(-1,1)//left-up
    };
    public static Vector2Int GetRandomCardinaldirection()
    {
        return cardinalDirectionsList[Random.Range(0,cardinalDirectionsList.Count)];
    }
}

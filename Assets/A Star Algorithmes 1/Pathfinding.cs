using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Pathfinding : MonoBehaviour
{
    PathRequestManager requestManager;
    Grid grid;

    public Button a_star; 
    public Button dijkstra;


    void Awake()
    {
        requestManager = GetComponent<PathRequestManager>();
        grid = GetComponent<Grid>();
    }

    public void StartFindPath(Vector3 startPos, Vector3 targetPos) 
    {
        string buttonName = EventSystem.current.currentSelectedGameObject.name;
        if(buttonName == "A*")
        {
            Debug.Log(buttonName);
            StartCoroutine(astar(startPos, targetPos));
        }
        if (buttonName == "Dijikstra")
        {
            Debug.Log(buttonName);
            StartCoroutine(Dijkstra(startPos, targetPos));
        }
        if (buttonName == "BFS")
        {
            Debug.Log(buttonName);
            StartCoroutine(bfs(startPos, targetPos));
        }
        if (buttonName == "DFS")
        {
            Debug.Log(buttonName);
            StartCoroutine(dfs(startPos, targetPos));
        }
        
        //StartCoroutine(Dijkstra(startPos, targetPos));

    }



 

    /***************/

    IEnumerator dfs(Vector3 startPos, Vector3 targetPos)
    {
        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        if (startNode != null && targetNode != null) // Check if start and target nodes are valid
        {
            if (startNode.walkable && targetNode.walkable)
            {
                Stack<Node> stack = new Stack<Node>();
                HashSet<Node> visited = new HashSet<Node>();

                stack.Push(startNode);
                visited.Add(startNode);

                while (stack.Count > 0)
                {
                    Node currentNode = stack.Pop();

                    if (currentNode == targetNode)
                    {
                        pathSuccess = true;
                        break;
                    }

                    foreach (Node neighbour in grid.GetNeighbours(currentNode))
                    {
                        if (!neighbour.walkable || visited.Contains(neighbour))
                        {
                            continue;
                        }

                        visited.Add(neighbour);
                        neighbour.parent = currentNode;
                        stack.Push(neighbour);
                    }
                }
            }
        }

        yield return null;

        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
        }

        requestManager.FinishedProcessingPath(waypoints, pathSuccess);
    }



    IEnumerator bfs(Vector3 startPos, Vector3 targetPos)
    {   
        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        if (startNode != null && targetNode != null) // Check if start and target nodes are valid
        {
            if (startNode.walkable && targetNode.walkable)
            {
                Queue<Node> queue = new Queue<Node>();
                HashSet<Node> visited = new HashSet<Node>();

                queue.Enqueue(startNode);
                visited.Add(startNode);

                while (queue.Count > 0)
                {
                    Node currentNode = queue.Dequeue();

                    if (currentNode == targetNode)
                    {
                        pathSuccess = true;
                        break;
                    }

                    foreach (Node neighbour in grid.GetNeighbours(currentNode))
                    {
                        if (!neighbour.walkable || visited.Contains(neighbour))
                        {
                            continue;
                        }

                        visited.Add(neighbour);
                        neighbour.parent = currentNode;
                        queue.Enqueue(neighbour);
                    }
                }
            }
        }

        yield return null;

        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
        }

        requestManager.FinishedProcessingPath(waypoints, pathSuccess);
    }




    public IEnumerator astar(Vector3 startPos, Vector3 targetPos)
    {

        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);


        if (startNode.walkable && targetNode.walkable)
        {
            Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    pathSuccess = true;
                    break;
                }

                foreach (Node neighbour in grid.GetNeighbours(currentNode))
                {
                    if (!neighbour.walkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    int newMovementCostToNeighbour = currentNode.gCost + HGetDistance(currentNode, neighbour);
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = HGetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                    }
                }
            }
        }
        yield return null;
        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
        }
        requestManager.FinishedProcessingPath(waypoints, pathSuccess);

    }

    // Update dijkstra 
    IEnumerator Dijkstra(Vector3 startPos, Vector3 targetPos)
    {
        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        if (startNode != null && targetNode != null) // Check if start and target nodes are valid
        {
            if (startNode.walkable && targetNode.walkable)
            {
                Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
                HashSet<Node> closedSet = new HashSet<Node>();

                openSet.Add(startNode);

                while (openSet.Count > 0)
                {
                    Node currentNode = openSet.RemoveFirst();
                    closedSet.Add(currentNode);

                    if (currentNode == targetNode)
                    {
                        pathSuccess = true;
                        break;
                    }

                    foreach (Node neighbour in grid.GetNeighbours(currentNode))
                    {
                        if (!neighbour.walkable || closedSet.Contains(neighbour))
                        {
                            continue;
                        }

                        int newDistance = currentNode.gCost + GetDistance(currentNode, neighbour);

                        if (newDistance < neighbour.gCost || !openSet.Contains(neighbour))
                        {
                            neighbour.gCost = newDistance;
                            neighbour.parent = currentNode;

                            if (!openSet.Contains(neighbour))
                                openSet.Add(neighbour);
                            else
                                openSet.UpdateItem(neighbour);
                        }
                    }
                }
            }
        }

        yield return null;

        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
        }

        requestManager.FinishedProcessingPath(waypoints, pathSuccess);
    }



    // old dijkstra 
    IEnumerator Dijkstra1(Vector3 startPos, Vector3 targetPos)
    {
        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        if (startNode.walkable && targetNode.walkable)
        {
            Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    pathSuccess = true;
                    break;
                }

                foreach (Node neighbour in grid.GetNeighbours(currentNode))
                {
                    if (!neighbour.walkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                    }
                }
            }
        }

        yield return null;

        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
        }

        requestManager.FinishedProcessingPath(waypoints, pathSuccess);
    }
    

    Vector3[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;
    }

    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if (directionNew != directionOld)
            {
                waypoints.Add(path[i].worldPosition);
            }
            directionOld = directionNew;
        }

        return waypoints.ToArray();
    }
    // heuristics for A* algorithm
    int HGetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }


    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        return dstX + dstY; // Dijkstra's algorithm doesn't use any heuristics
    }
}
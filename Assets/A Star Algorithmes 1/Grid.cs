using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

public class Grid : MonoBehaviour
{

    public bool displayGridGizmos;
    public LayerMask unwalkableMask;
    private Vector2 gridWorldSize; // bdltha 
    public float nodeRadius;
    public Node[,] grid;

    
    public TilemapRenderer spriteRenderer;

    [SerializeField]
    private AbstractDungeonGenerator dungeonGenerator;


    float nodeDiameter;
    int gridSizeX, gridSizeY;


   
    void Awake()
    {


        
        
        // center of the map 
        Vector3 center = spriteRenderer.bounds.center;

        // size of the map  
        float width = spriteRenderer.bounds.size.x;
        float height = spriteRenderer.bounds.size.y;


        //gridWorldSize.x = width + width/2;
        //gridWorldSize.y = height + height/2;
        gridWorldSize.x = width * 2;
        gridWorldSize.y = height * 2;


        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }

    void CreateGrid()
    {        
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - new Vector3(0, 1, 0) * gridWorldSize.y / 2;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + new Vector3(0, 1, 0) * (y * nodeDiameter + nodeRadius) ;
                
                //BoxCollider2D bc = gameObject.AddComponent<BoxCollider2D>() as BoxCollider2D;
                Vector2 box = new Vector2(nodeDiameter - 0.1f, nodeDiameter - 0.1f);
                bool walkable = !(Physics2D.OverlapBox(worldPoint, box, 90, unwalkableMask));
                //bc.transform.position = worldPoint;
                grid[x, y] = new Node(walkable, worldPoint, x, y);
                //print(box);
	
            }
        }
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }


    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y,1));
        if (grid != null && displayGridGizmos)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }
}
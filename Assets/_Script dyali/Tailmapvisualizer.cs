using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
public class Tailmapvisualizer : MonoBehaviour
{
    public float detectRadius;
    public Tilemap[] tilemaps; // Set this to an array of all the Tilemaps you want to detect tiles on


    /**/
    public List<GameObject> instantiatedItems = new List<GameObject>();

    public List<GameObject> items; //here 
    public GameObject Spawner; 


    [SerializeField]
    private Tilemap floorTilemap,wallTilemap;
    [SerializeField]
    private TileBase floorTile, wallTop;
    [SerializeField]
    private TileBase wallSideRight, wallSideLeft, wallBottom, wallFull ,
        wallInnerCornnerDownLeft,wallInnerCornnerDownRight,wallDiagonalCornnerDownRight,
        wallDiagonalCornnerDownLeft, wallDiagonalCornnerUpRight, wallDiagonalCornnerUpLeft;




    



    // detect tile by position 
    public bool DetectedTilesAtPosition(Vector3 position)
    {
        foreach (Tilemap tilemap in tilemaps)
        {
            int radiusInCells = Mathf.CeilToInt(detectRadius / tilemap.cellSize.x);
            Vector3Int centerCellPosition = tilemap.WorldToCell(position);
            BoundsInt bounds = new BoundsInt(centerCellPosition - new Vector3Int(radiusInCells, radiusInCells, 0),
                                             new Vector3Int(radiusInCells * 2 + 1, radiusInCells * 2 + 1, 1));
            TileBase[] tiles = tilemap.GetTilesBlock(bounds);

            foreach (TileBase tile in tiles)
            {
                if (tile != null)
                {
                    // Tile was found at position bounds
                    return true;
                }
            }
        }
        // No conflicting tiles found
        return false;
    }



    // ana zedtha 
    // destroy items bax mayb9awx yduplicaw 
    public void DestroyAllItems()
    {
        foreach (GameObject item in instantiatedItems)
        {
            DestroyImmediate(item);
        }
        instantiatedItems.Clear(); // Clear the list after destroying enemies
    }

    // instantiate items
    public void Item(Vector2Int pos, string binaryType, int decision)
    {
        int typeAsInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;
        if (WallTypesHelper.wallTop.Contains(typeAsInt))
        {
            tile = wallTop;
        }
        else if (WallTypesHelper.wallSideRight.Contains(typeAsInt))
        {
            tile = wallSideRight;
        }
        else if (WallTypesHelper.wallSideLeft.Contains(typeAsInt))
        {
            tile = wallSideLeft;
        }
        else if (WallTypesHelper.wallBottm.Contains(typeAsInt))
        {
            tile = wallBottom;
        }
        else if (WallTypesHelper.wallFull.Contains(typeAsInt))
        {
            tile = wallFull;
        }

        // Item Probability
        if (decision == 1 )
        {
            int rand = UnityEngine.Random.Range(0, 5);
            if (tile == wallTop && rand == 1)
            {
                int randIndex = UnityEngine.Random.Range(0, items.Count);
                var tileposition = wallTilemap.WorldToCell((Vector3Int)pos);
                if (DetectedTilesAtPosition(wallTilemap.GetCellCenterWorld(tileposition) + new Vector3Int(0, -1, 0)) == true)
                {
                    GameObject item = Instantiate(items[randIndex]);
                    item.transform.position = wallTilemap.GetCellCenterWorld(tileposition) + new Vector3Int(0, -1, 0); // get the center of tile and +x = 1 bax t7at flimn 7daha 
                    instantiatedItems.Add(item);
                }
                
            }
            else if (tile == wallSideRight && rand == 2)
            {
                int randIndex = UnityEngine.Random.Range(0, items.Count);
                var tileposition = wallTilemap.WorldToCell((Vector3Int)pos);
                if (DetectedTilesAtPosition(wallTilemap.GetCellCenterWorld(tileposition) + new Vector3Int(-1, 0, 0)) == true)
                {
                    GameObject item = Instantiate(items[randIndex]);
                    item.transform.position = wallTilemap.GetCellCenterWorld(tileposition) + new Vector3Int(-1, 0, 0); // get the center of tile and +x = 1 bax t7at flimn 7daha 
                    instantiatedItems.Add(item);
                }

            }
            else if (tile == wallSideLeft && rand == 3)
            {
                int randIndex = UnityEngine.Random.Range(0, items.Count);
                var tileposition = wallTilemap.WorldToCell((Vector3Int)pos);
                if(DetectedTilesAtPosition(wallTilemap.GetCellCenterWorld(tileposition) + new Vector3Int(1, 0, 0)) == true)
                {
                    GameObject item = Instantiate(items[randIndex]);
                    item.transform.position = wallTilemap.GetCellCenterWorld(tileposition) + new Vector3Int(1, 0, 0); // get the center of tile and +x = 1 bax t7at flimn 7daha 
                    instantiatedItems.Add(item);
                }
            }
            else if (tile == wallBottom && rand == 4)
            {
                int randIndex = UnityEngine.Random.Range(0, items.Count);
                var tileposition = wallTilemap.WorldToCell((Vector3Int)pos);
                if (DetectedTilesAtPosition(wallTilemap.GetCellCenterWorld(tileposition) + new Vector3Int(0, 1, 0)) == true)
                {
                    GameObject item = Instantiate(items[randIndex]);
                    item.transform.position = wallTilemap.GetCellCenterWorld(tileposition) + new Vector3Int(0, 1, 0); // get the center of tile and +x = 1 bax t7at flimn 7daha 
                    instantiatedItems.Add(item);
                }
            }
            
        }

        // Spawner Probabiblity 
        else if (decision == 2)
        {
            int rand = UnityEngine.Random.Range(0, 5);
            if (tile == wallTop && rand == 1)
            {
                var tileposition = wallTilemap.WorldToCell((Vector3Int)pos);
                if (DetectedTilesAtPosition(wallTilemap.GetCellCenterWorld(tileposition) + new Vector3Int(0, -1, 0)) == true)
                {
                    GameObject spawnerobj = Instantiate(Spawner);
                    spawnerobj.transform.position = wallTilemap.GetCellCenterWorld(tileposition) + new Vector3Int(0, -1, 0); // get the center of tile and +x = 1 bax t7at flimn 7daha 
                    instantiatedItems.Add(spawnerobj);
                }

            }
            else if(tile == wallSideRight && rand == 2)
            {
                var tileposition = wallTilemap.WorldToCell((Vector3Int)pos);
                if (DetectedTilesAtPosition(wallTilemap.GetCellCenterWorld(tileposition) + new Vector3Int(-1, 0, 0)) == true)
                {
                    GameObject spawnerobj = Instantiate(Spawner);
                    spawnerobj.transform.position = wallTilemap.GetCellCenterWorld(tileposition) + new Vector3Int(-1, 0, 0); // get the center of tile and +x = 1 bax t7at flimn 7daha 
                    instantiatedItems.Add(spawnerobj);
                }
            }
            else if (tile == wallSideLeft && rand == 3)
            {
                var tileposition = wallTilemap.WorldToCell((Vector3Int)pos);
                if (DetectedTilesAtPosition(wallTilemap.GetCellCenterWorld(tileposition) + new Vector3Int(1, 0, 0)) == true)
                {
                    GameObject spawnerobj = Instantiate(Spawner);
                    spawnerobj.transform.position = wallTilemap.GetCellCenterWorld(tileposition) + new Vector3Int(1, 0, 0); // get the center of tile and +x = 1 bax t7at flimn 7daha 
                    instantiatedItems.Add(spawnerobj);
                }
            }
            else if (tile == wallBottom && rand == 4)
            {
                var tileposition = wallTilemap.WorldToCell((Vector3Int)pos);
                if (DetectedTilesAtPosition(wallTilemap.GetCellCenterWorld(tileposition) + new Vector3Int(0, 1, 0)) == true)
                {
                    GameObject spawnerobj = Instantiate(Spawner);
                    spawnerobj.transform.position = wallTilemap.GetCellCenterWorld(tileposition) + new Vector3Int(0, 1, 0); // get the center of tile and +x = 1 bax t7at flimn 7daha 
                    instantiatedItems.Add(spawnerobj);
                }
            }
        }

    }

    public void Item2(Vector2Int pos, string binaryType, int decision)
    {
        int typeAsInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;
        if (WallTypesHelper.wallInnerCornerDownLeft.Contains(typeAsInt))
        {
            tile = wallInnerCornnerDownLeft;
        }
        else if (WallTypesHelper.wallInnerCornerDownRight.Contains(typeAsInt))
        {
            tile = wallInnerCornnerDownRight;
        }
        else if (WallTypesHelper.wallDiagonalCornerDownLeft.Contains(typeAsInt))
        {
            tile = wallDiagonalCornnerDownLeft;
        }
        else if (WallTypesHelper.wallDiagonalCornerDownRight.Contains(typeAsInt))
        {
            tile = wallDiagonalCornnerDownRight;
        }
        else if (WallTypesHelper.wallDiagonalCornerUpLeft.Contains(typeAsInt))
        {
            tile = wallDiagonalCornnerUpLeft;
        }
        else if (WallTypesHelper.wallDiagonalCornerUpRight.Contains(typeAsInt))
        {
            tile = wallDiagonalCornnerUpRight;
        }
        else if (WallTypesHelper.wallFullEightDirections.Contains(typeAsInt))
        {
            tile = wallFull;
        }
        else if (WallTypesHelper.wallBottmEightDirections.Contains(typeAsInt))
        {
            tile = wallBottom;
        }

        // Item Probability
       

        // Spawner Probabiblity 
        if (decision == 2)
        {
            int rand = UnityEngine.Random.Range(0, 5);
            if (tile == wallInnerCornnerDownLeft && rand == 1)
            {
                var tileposition = wallTilemap.WorldToCell((Vector3Int)pos);
                if (DetectedTilesAtPosition(wallTilemap.GetCellCenterWorld(tileposition) + new Vector3Int(1, 1, 0)) == true)
                {
                    GameObject spawnerobj = Instantiate(Spawner);
                    spawnerobj.transform.position = wallTilemap.GetCellCenterWorld(tileposition) + new Vector3Int(1, 1, 0); // get the center of tile and +x = 1 bax t7at flimn 7daha 
                    instantiatedItems.Add(spawnerobj);
                }

            }
            else if (tile == wallInnerCornnerDownRight && rand == 2)
            {
                var tileposition = wallTilemap.WorldToCell((Vector3Int)pos);
                if (DetectedTilesAtPosition(wallTilemap.GetCellCenterWorld(tileposition) + new Vector3Int(-1, 1, 0)) == true)
                {
                    GameObject spawnerobj = Instantiate(Spawner);
                    spawnerobj.transform.position = wallTilemap.GetCellCenterWorld(tileposition) + new Vector3Int(-1, 1, 0); // get the center of tile and +x = 1 bax t7at flimn 7daha 
                    instantiatedItems.Add(spawnerobj);
                }
            }
        }

    }



    /*******/
    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        PaintFloorTiles(floorPositions, floorTilemap, floorTile);
    }


    private void PaintFloorTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tile)
    {
        foreach (var position in positions) {
            PaintSingleTile(tilemap,tile,position);      
        }
    }

    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
    {
        var tileposition = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tileposition, tile);
    }
    public void Clear()
    {
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
    }

    internal void PaintSingleBasicWall(Vector2Int position,string binaryType)
    {
        int typeAsInt=Convert.ToInt32(binaryType,2);
        TileBase tile = null;
        if(WallTypesHelper.wallTop.Contains(typeAsInt))
        {
            tile=wallTop;
        }
        else if(WallTypesHelper.wallSideRight.Contains(typeAsInt))
        {
            tile=wallSideRight;
        }
        else if (WallTypesHelper.wallSideLeft.Contains(typeAsInt))
        {
            tile = wallSideLeft;
        }
        else if (WallTypesHelper.wallBottm.Contains(typeAsInt))
        {
            tile = wallBottom;
        }
        else if (WallTypesHelper.wallFull.Contains(typeAsInt))
        {
            tile = wallFull;
        }
        if (tile!=null)
        {
            PaintSingleTile(wallTilemap,tile,position);
            
        }

    }

    internal void PaintSingleCornerWall(Vector2Int position, string binaryType)
    {
        int typeAsInt=Convert.ToInt32(binaryType,2);
        TileBase tile = null;
        if(WallTypesHelper.wallInnerCornerDownLeft.Contains(typeAsInt))
        {
            tile = wallInnerCornnerDownLeft;
        }
        else if (WallTypesHelper.wallInnerCornerDownRight.Contains(typeAsInt))
        {
            tile = wallInnerCornnerDownRight;
        }
        else if (WallTypesHelper.wallDiagonalCornerDownLeft.Contains(typeAsInt))
        {
            tile = wallDiagonalCornnerDownLeft;
        }
        else if (WallTypesHelper.wallDiagonalCornerDownRight.Contains(typeAsInt))
        {
            tile=wallDiagonalCornnerDownRight;
        }
        else if (WallTypesHelper.wallDiagonalCornerUpLeft.Contains(typeAsInt))
        {
            tile= wallDiagonalCornnerUpLeft;
        }
        else if (WallTypesHelper.wallDiagonalCornerUpRight.Contains(typeAsInt))
        {
            tile= wallDiagonalCornnerUpRight;
        }
        else if(WallTypesHelper.wallFullEightDirections.Contains(typeAsInt))
        {
            tile = wallFull;
        }
        else if (WallTypesHelper.wallBottmEightDirections.Contains(typeAsInt))
        {
            tile = wallBottom;
        }
        if (tile != null)
        {
            PaintSingleTile(wallTilemap,tile, position);
        }
    }
}

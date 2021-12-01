using UnityEngine;

public class Ship : MonoBehaviour
{
    public Grid playerGrid;
    public int Length;
    public bool placed = false;

    private bool dragging = false;
    private Vector3 shipDragStartPos;
    private Tile placedTile = null;
    Tile[,] tiles;

    private Vector3 spawnPos;

    private void Start()
    {
        spawnPos = transform.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && dragging)
        {
            if (transform.rotation == Quaternion.Euler(0, 0, 90))
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 90);
            }
        }
    }

    private void OnMouseDrag()
    {
        if (dragging)
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, shipDragStartPos.z);

            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
            transform.position = curPosition;
        }
    }


    private void OnMouseDown()
    {
        tiles = playerGrid.returnTiles();
        dragging = true;
        shipDragStartPos = transform.localPosition;
        if(transform.position != spawnPos)
        {
            UpdateTiles(placedTile, tiles, false);
        }
    }

    private void OnMouseUp()
    {
        Vector2 shipCenterPos = transform.position;
        dragging = false;
        Vector2 closestTile = tiles[0,0].pos;
        float smallestDist = (new Vector2(closestTile.x, closestTile.y) - shipCenterPos).magnitude;
        foreach(Tile tile in tiles)
        {
            float dist = (new Vector2(tile.transform.position.x, tile.transform.position.y) - shipCenterPos).magnitude;
            if (dist < smallestDist)
            {
                closestTile = tile.pos;
                placedTile = tile;
                smallestDist = dist;
            }
        }
        if (smallestDist < 0.6 && CheckIfValidPlacement(placedTile, tiles))
        {
            transform.position = closestTile;
            if (Length % 2 == 0)
            {
                transform.position += transform.rotation == Quaternion.Euler(0, 0, 90) ? new Vector3(0.5f, 0, 0) : new Vector3(0, 0.5f, 0);
            }
            UpdateTiles(placedTile, tiles, true);
            placed = true;
        }
        else
        {
            placed = false;
            transform.position = spawnPos;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        
    }

    private bool CheckIfValidPlacement(Tile placementTile, Tile[,] allTiles)
    {
        int max = Length % 2 == 0 ? Length / 2 : (Length - 1) / 2;
        bool isEven = Length % 2 == 0;
        bool isRotated = transform.rotation == Quaternion.Euler(0, 0, 90);
        for (int i = 0; i <= max; i++)
        {
            try
            {
                if (allTiles[placementTile.xPos + (isRotated ? i : 0), placementTile.yPos + (isRotated ? 0 : -i)].shipOn == true)
                {
                    return false;
                }
                else if (allTiles[placementTile.xPos - (isRotated?(isEven ? ((i - 1) <= 0 ? 0 : 1) : i):0), placementTile.yPos + (isRotated ? 0:(isEven ? ((i - 1) <= 0 ? 0 : 1) : i))].shipOn == true)
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        return true;
    }

    private void UpdateTiles(Tile placementTile, Tile[,] allTiles, bool AddOrRemove)
    {
        int max = Length % 2 == 0 ? Length / 2 : (Length - 1) / 2;
        bool isEven = Length % 2 == 0;
        bool isRotated = transform.rotation == Quaternion.Euler(0, 0, 90);
        for (int i = 0; i <= max; i++)
        {
            allTiles[placementTile.xPos + (isRotated ? i : 0), placementTile.yPos + (isRotated ? 0 : -i)].shipOn = AddOrRemove;
            allTiles[placementTile.xPos - (isRotated ? (isEven ? ((i - 1) <= 0 ? 0 : 1) : i) : 0), placementTile.yPos + (isRotated ? 0 : (isEven ? ((i - 1) <= 0 ? 0 : 1) : i))].shipOn = AddOrRemove;
        }
    }
}
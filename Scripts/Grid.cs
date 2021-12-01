using UnityEngine;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{
    public GameObject Tile;

    public Tile[,] tileGrid;

    private float gridWidth;
    private float gridHeight;
    private float tileWidth;
    private float tileHeight;

    private int tileCountX = 10;
    private int tileCountY = 10;

    private Vector2 firstSpawnPos;

    // Start is called before the first frame update
    private void Awake()
    {
        gridHeight = transform.localScale.y;
        gridWidth = transform.localScale.x;
        tileWidth = gridWidth/tileCountX;
        tileHeight = gridHeight/tileCountY;
        firstSpawnPos = transform.position + new Vector3(-transform.localScale.x / 2, transform.localScale.y / 2, 0) + new Vector3(tileWidth/2, -tileHeight/2, 0);
        tileGrid = new Tile[tileCountX, tileCountY];
        GenerateGridEmpty();
    }

    public void GenerateGridEmpty()
    {
        for(int i = 0; i < tileCountX; i+=1)
        {
            for(int j = 0; j < tileCountY; j+=1)
            {
                GameObject tileSpawn = Instantiate<GameObject>(Tile);
                tileSpawn.transform.position = firstSpawnPos + new Vector2(i * tileWidth, -j * tileHeight);
                tileSpawn.transform.localScale = new Vector3(tileWidth, tileHeight, 1);
                tileSpawn.transform.SetParent(this.transform);
                tileSpawn.name = "Tile" + "(" + i + ")" + "(" + j + ")";
                tileSpawn.GetComponent<Tile>().xPos = i;
                tileSpawn.GetComponent<Tile>().yPos = j;
                tileGrid[i, j] = tileSpawn.GetComponent<Tile>();
            }
        }
    }

    public void UpdateGridFromData(Tile[,] data)
    {
        for(int i = 0; i < tileCountX; i++)
        {
            for(int j = 0; j < tileCountY; j++)
            {
                tileGrid[i, j].shipOn = data[i, j].shipOn;
                tileGrid[i, j].missileHit = data[i, j].missileHit;
                tileGrid[i, j].UpdateColor();
            }
        }
    }

    public void UpdateGridFromDataNoShip(Tile[,] data)
    {
        for (int i = 0; i < tileCountX; i++)
        {
            for (int j = 0; j < tileCountY; j++)
            {
                tileGrid[i, j].shipOn = data[i, j].shipOn;
                tileGrid[i, j].missileHit = data[i, j].missileHit;
                tileGrid[i, j].UpdateColorNoShip();
            }
        }
    }

    public void UpdateGridFromData()
    {
        foreach(Tile tile in tileGrid)
        {
            tile.UpdateColor();
        }
    }

    public Tile[,] returnTiles()
    {
        return tileGrid;
    }

    public void printTiles()
    {
        string output = "\n";
        for (int i = 0; i < gridHeight; i++)
        {
            
            for(int j = 0; j < gridWidth; j++)
            {
                output += (tileGrid[j, i].shipOn ? '1' : '0') + " ";
            }
            output += "\n";
        }
        Debug.Log(output);
    }
}

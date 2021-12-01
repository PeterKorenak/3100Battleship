using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Tile[,] enemyTileGrid;
    public GameObject tileClone;
    public bool lastShotHit = false;
    public Vector2Int lastShot;

    private readonly int[] ships = { 5, 4, 3, 3, 2 };

    private void Start()
    {
        enemyTileGrid = new Tile[10, 10];
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                GameObject tileSpawn = Instantiate<GameObject>(tileClone);
                Destroy(tileSpawn.GetComponent<SpriteRenderer>());
                tileSpawn.transform.SetParent(transform);
                enemyTileGrid[i, j] = tileSpawn.GetComponent<Tile>();
            }
        }
    }

    public void RandomizeGrid()
    {
        for (int i = 0; i < ships.Length; i++)
        {
            bool placed = false;

            while (placed == false)
            {
                int rand_x = Random.Range(0, 10);
                int rand_y = Random.Range(0, 10);

                bool ship_in_x = false;
                bool ship_in_y = false;

                for (int j = rand_x + 1 - ships[i]; j <= rand_x; j++)
                {
                    if (rand_x + 1 >= ships[i] && enemyTileGrid[rand_x, j].shipOn == true)
                    {
                        ship_in_x = true;
                    }
                }

                for (int j = rand_y + 1 - ships[i]; j <= rand_y; j++)
                {
                    if (rand_y + 1 >= ships[i] && enemyTileGrid[j, rand_y].shipOn == true)
                    {
                        ship_in_y = true;
                    }
                }

                if (rand_x + 1 >= ships[i] && ship_in_x == false)
                {
                    for (int j = rand_x + 1 - ships[i]; j <= rand_x; j++)
                    {
                        enemyTileGrid[rand_x, j].shipOn = true;
                    }
                    placed = true;
                }
                else if (rand_y + 1 >= ships[i] && ship_in_y == false)
                {
                    for (int j = rand_y + 1 - ships[i]; j <= rand_y; j++)
                    {
                        enemyTileGrid[j, rand_y].shipOn = true;
                    }
                    placed = true;
                }
            }
        }
    }

    public void EnemyAttackEasy(out int x, out int y)
    {
        x = Random.Range(0, 9);
        y = Random.Range(0, 9);
    }

    public void EnemyAttackMedium(out int x, out int y)
    {
        x = lastShot.x;
        y = lastShot.y;
        if (lastShotHit)
        {
            int roll = Random.Range(0, 3);
            x = (roll == 0 ? lastShot.x + 1 : x);
            x = (roll == 1 ? lastShot.x - 1 : x);
            y = (roll == 2 ? lastShot.y + 1 : y);
            y = (roll == 3 ? lastShot.y - 1 : y);
        }
        if (!lastShotHit || x > 9 || x < 0 || y > 9 || x < 0)
        {
            x = Random.Range(0, 9);
            y = Random.Range(0, 9);
        }
    }

    public void EnemyAttackHard(out int x, out int y, Grid PlayerGrid)
    {
        x = lastShot.x;
        y = lastShot.y;
        if (lastShotHit)
        {
            int roll = Random.Range(0, 4);
            x = (roll == 0 ? lastShot.x + 1 : x);
            x = (roll == 1 ? lastShot.x - 1 : x);
            y = (roll == 2 ? lastShot.y + 1 : y);
            y = (roll == 3 ? lastShot.y - 1 : y);
        }
        if (!lastShotHit || x > 9 || x < 0 || y > 9 || x < 0)
        {
            x = Random.Range(0, 9);
            y = Random.Range(0, 9);
            if (Random.Range(0, 3) == 0)
            {
                foreach(Tile tile in PlayerGrid.returnTiles())
                {
                    if (tile.shipOn && !tile.missileHit)
                    {
                        x = tile.xPos;
                        y = tile.yPos;
                    }
                }
            }
            
        }
    }
}

using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameRunner : MonoBehaviour
{
    private int difficulty = 1;

    [Header("UI Elements")]
    public Button NextBtn;
    public Text infoText;

    [Header("Sound Stuff")]
    public SoundManager soundManager;

    [Header("Player stuff")]
    public Grid interactGrid;
    public Grid displayGrid;
    public Ship[] ships;

    [Header("Enemy stuff")]
    public Enemy enemyAI;
    public Grid enemyGrid;

    private Tile markedTile = null;
    private GameManager diffCheck;
    private bool midAttack = false;

    private void Start()
    {
        diffCheck = GameObject.Find("GameManager").GetComponent<GameManager>();
        difficulty = diffCheck.aiDifficulty;
        NextBtn.onClick.AddListener(EndPlacingPhase);
    }

    private void EndPlacingPhase()
    {
        foreach(Ship ship in ships)
        {
            if(ship.placed == false)
            {
                return;
            }
        }
        displayGrid.UpdateGridFromData(interactGrid.tileGrid);
        foreach(Ship ship in ships)
        {
            Destroy(ship.gameObject);
        }
        enemyAI.RandomizeGrid();
        enemyGrid.UpdateGridFromData(enemyAI.enemyTileGrid);
        foreach(Tile tile in interactGrid.tileGrid)
        {
            tile.Clickable = true;
        }
        infoText.text = "Choose a target square!";
        soundManager.PlaySound(0);
        NextBtn.transform.GetChild(0).GetComponent<Text>().text = "Fire!";
        NextBtn.onClick.RemoveAllListeners();
        NextBtn.onClick.AddListener(DeclareAttack);
    }

    private void DeclareAttack()
    {
        if (!markedTile || midAttack) return;
        Tile enemyTile = enemyAI.enemyTileGrid[markedTile.xPos, markedTile.yPos];
        if (enemyTile.missileHit == false)
        {
            enemyTile.missileHit = true;
            soundManager.PlaySound(3);
            midAttack = true;
            StartCoroutine(WaitForSound(enemyTile));
        }
    }

    private void AttackResolution(Tile enemyTile)
    {
        infoText.text = enemyTile.shipOn ? "Hit enemy ship!" : "Missed enemy ship!";
        if (enemyTile.shipOn)
        {
            int rand = Random.Range(0,15);
            if (rand == 3)
            {
                soundManager.PlaySound(4);
            }
            else
            {
                soundManager.PlaySound(1);
            }
        }
        else
        {
            soundManager.PlaySound(2);
        }
        infoText.text += "\n";
        enemyGrid.UpdateGridFromData(enemyAI.enemyTileGrid);
        interactGrid.UpdateGridFromDataNoShip(enemyAI.enemyTileGrid);
        EnemyAttack();
        markedTile = null;
        CheckWinLose();
        midAttack = false;
    }

    private IEnumerator WaitForSound(Tile enemyTile)
    {
        yield return new WaitUntil(() => !soundManager.CheckIfPlaying());
        AttackResolution(enemyTile);
    }

    private void EnemyAttack()
    {
        switch (difficulty)
        {
            case 1:
                EnemyAttackEasy();
                break;
            case 2:
                EnemyAttackMedium();
                break;
            case 3:
                EnemyAttackHard();
                break;
            default:
                EnemyAttackEasy();
                break;
        }
    }

    private void EnemyAttackEasy()
    {
        int attackIndexX, attackIndexY;
        do
        {
            enemyAI.EnemyAttackEasy(out attackIndexX, out attackIndexY);
        } while (displayGrid.tileGrid[attackIndexX, attackIndexY].missileHit == true);
        enemyAI.lastShot = new Vector2Int(attackIndexX, attackIndexY);
        enemyAI.lastShotHit = displayGrid.tileGrid[attackIndexX, attackIndexY].shipOn;
        displayGrid.tileGrid[attackIndexX, attackIndexY].missileHit = true;
        infoText.text += displayGrid.tileGrid[attackIndexX, attackIndexY].shipOn ? "Enemy hit your ship!" : "Enemy missed your ship!";
        displayGrid.UpdateGridFromData();
    }

    private void EnemyAttackMedium()
    {
        int attackIndexX, attackIndexY;
        do
        {
            enemyAI.EnemyAttackMedium(out attackIndexX, out attackIndexY);
        } while (displayGrid.tileGrid[attackIndexX, attackIndexY].missileHit == true);
        enemyAI.lastShot = new Vector2Int(attackIndexX, attackIndexY);
        enemyAI.lastShotHit = displayGrid.tileGrid[attackIndexX, attackIndexY].shipOn;
        displayGrid.tileGrid[attackIndexX, attackIndexY].missileHit = true;
        infoText.text += displayGrid.tileGrid[attackIndexX, attackIndexY].shipOn ? "Enemy hit your ship!" : "Enemy missed your ship!";
        displayGrid.UpdateGridFromData();
    }

    private void EnemyAttackHard()
    {
        int attackIndexX, attackIndexY;
        do
        {
            enemyAI.EnemyAttackHard(out attackIndexX, out attackIndexY, displayGrid);
        } while (displayGrid.tileGrid[attackIndexX, attackIndexY].missileHit == true);
        enemyAI.lastShot = new Vector2Int(attackIndexX, attackIndexY);
        enemyAI.lastShotHit = displayGrid.tileGrid[attackIndexX, attackIndexY].shipOn;
        displayGrid.tileGrid[attackIndexX, attackIndexY].missileHit = true;
        infoText.text += displayGrid.tileGrid[attackIndexX, attackIndexY].shipOn ? "Enemy hit your ship!" : "Enemy missed your ship!";
        displayGrid.UpdateGridFromData();
    }

    public void SetMarkedTile(Tile tile)
    {
        if (midAttack)
        {
            markedTile = null;
            return;
        }
        if (markedTile)
        {
            markedTile.GetComponent<SpriteRenderer>().color = markedTile.ogColor;
        }
        markedTile = tile;
        markedTile.GetComponent<SpriteRenderer>().color = markedTile.hoverColor;
        soundManager.PlaySound(5);

    }

    private void CheckWinLose()
    {
        bool win = true;
        bool loss = true;
        foreach(Tile tile in enemyGrid.returnTiles())
        {
            if(tile.shipOn && !tile.missileHit)
            {
                win = false;
            }
        }
        if (win)
        {
            diffCheck.won = true;
            SceneManager.LoadScene(2);
        }
        foreach(Tile tile in displayGrid.returnTiles())
        {
            if(tile.shipOn && !tile.missileHit)
            {
                loss = false;
            }
        }
        if (loss)
        {
            diffCheck.won = false;
            SceneManager.LoadScene(3);
        }
    }
}

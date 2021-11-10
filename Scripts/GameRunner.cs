using UnityEngine.UI;
using UnityEngine;

public class GameRunner : MonoBehaviour
{
    private int difficulty = 1;

    [Header("UI Elements")]
    public Button NextBtn;
    public Text infoText;

    [Header("Player stuff")]
    public Grid interactGrid;
    public Grid displayGrid;
    public Ship[] ships;

    [Header("Enemy stuff")]
    public Enemy enemyAI;
    public Grid enemyGrid;

    private Tile markedTile = null;

    private void Start()
    {
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
        NextBtn.onClick.RemoveAllListeners();
        NextBtn.onClick.AddListener(DeclareAttack);
    }

    private void DeclareAttack()
    {
        if (!markedTile) return;
        Tile enemyTile = enemyAI.enemyTileGrid[markedTile.xPos, markedTile.yPos];
        if (enemyTile.missileHit == false)
        {
            enemyTile.missileHit = true;
            infoText.text = enemyTile.shipOn ? "Hit enemy ship!" : "Missed enemy ship!";
            infoText.text += "\n";
            enemyGrid.UpdateGridFromData(enemyAI.enemyTileGrid);
            interactGrid.UpdateGridFromDataNoShip(enemyAI.enemyTileGrid);
            EnemyAttack();
        }
        markedTile = null;
    }

    private void EnemyAttack()
    {
        switch (difficulty)
        {
            case 1:
                EnemyAttackEasy();
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

    public void SetMarkedTile(Tile tile)
    {
        if (markedTile)
        {
            markedTile.GetComponent<SpriteRenderer>().color = markedTile.ogColor;
        }
        markedTile = tile;
        markedTile.GetComponent<SpriteRenderer>().color = markedTile.hoverColor;

    }
}

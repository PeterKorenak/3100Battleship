using UnityEngine;

public class Tile : MonoBehaviour
{
    public Color32 hitColor;
    public Color32 shipColor;
    public Color32 missColor;
    public Color32 ogColor;
    public Color32 hoverColor;

    private GameRunner gm;

    public bool missileHit = false;
    public bool shipOn = false;
    public bool Clickable = false;
    public Vector2 pos;
    public int xPos;
    public int yPos;

    private SpriteRenderer sprt;

    private void Start()
    {
        sprt = GetComponent<SpriteRenderer>();
        pos = transform.position;
        ogColor = sprt.color;
        gm = GameObject.Find("GameRunner").GetComponent<GameRunner>();
    }

    public void UpdateColor()
    {
        if (missileHit && shipOn)
        {
            sprt.color = hitColor;
        }
        else if(missileHit)
        {
            sprt.color = missColor;
        }
        else if (shipOn)
        {
            sprt.color = shipColor;
        }
        else
        {
            sprt.color = ogColor;
        }
    }

    public void UpdateColorNoShip()
    {
        if (missileHit && shipOn)
        {
            sprt.color = hitColor;
        }
        else if (missileHit)
        {
            sprt.color = missColor;
        }
        else
        {
            sprt.color = ogColor;
        }
    }

    private void OnMouseDown()
    {
        if (Clickable && !missileHit)
        {
            gm.SetMarkedTile(GetComponent<Tile>());
        }
    }
}

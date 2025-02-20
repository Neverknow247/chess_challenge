using UnityEngine;

public class ChessPiece : MonoBehaviour
{
    public GameObject controller;
    public GameObject moveTile;

    private int xPos = 0;
    public void SetXPos(int x) { xPos = x; }
    public int GetXPos() { return xPos; }
    private int yPos = 0;
    public void SetYPos(int y) { yPos = y; }
    public int GetYPos() { return yPos; }

    private string player;

    public Sprite black_queen, black_pawn;
    public Sprite white_queen, white_pawn;

    public void SetPiece()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        SetPosition();
        switch (this.name)
        {
            case "black_queen": this.GetComponent<SpriteRenderer>().sprite = black_queen; player = "black"; break;
            case "black_pawn": this.GetComponent<SpriteRenderer>().sprite = black_pawn; player = "black"; break;
            case "white_queen": this.GetComponent<SpriteRenderer>().sprite = white_queen; player = "white"; break;
            case "white_pawn": this.GetComponent<SpriteRenderer>().sprite = white_pawn; player = "white"; break;
        }
    }

    public void SetPosition()
    {
        float x = xPos;
        float y = yPos;

        x *= 0.75f;
        y *= 0.75f;

        x += -2.25f;
        y += -2.25f;

        this.transform.position = new Vector3(x, y, -1.0f);
    }

    private void OnMouseUp()
    {
        Debug.Log("mouse up");
        if (!controller.GetComponent<Controller>().IsGameOver() && controller.GetComponent<Controller>().GetCurrentPlayer() == player)
        {
            Debug.Log("tiles set");
            DestroyMoveTiles();
            BuildMoveTiles();
        }
    }

    private void OnMouseDrag()
    {
        Debug.Log("mouse drag");
    }

    public void DestroyMoveTiles()
    {
        GameObject[] moveTiles = GameObject.FindGameObjectsWithTag("MoveTile");
        for (int i = 0; i < moveTiles.Length; i++)
        {
            Destroy(moveTiles[i]);
        }
    }

    public void BuildMoveTiles()
    {
        switch (this.name)
        {
            case "black_queen":
            case "white_queen":
                LineMove(1,0);
                LineMove(0,1);
                LineMove(1,1);
                LineMove(-1,0);
                LineMove(0,-1);
                LineMove(-1,-1);
                LineMove(-1,1);
                LineMove(1,-1);
                break;
            case "black_pawn":
                PawnMove(xPos, yPos - 1);
                break;
            case "white_pawn":
                PawnMove(xPos,yPos + 1);
                break;
        }
    }

    public void LineMove(int _x, int _y)
    {
        Controller c = controller.GetComponent<Controller>();
        int x = xPos + _x;
        int y = yPos + _y;

        while (c.PositionOnBoard(x,y) && c.GetPosition(x,y) == null)
        {
            SpawnMoveTile(x,y);
            x += _x;
            y += _y;
        }
        if (c.PositionOnBoard(x, y) && c.GetPosition(x, y).GetComponent<ChessPiece>().player != player)
        {
            SpawnMoveTile(x, y, true);
        }
    }
    
    public void PawnMove(int x, int y)
    {
        Controller c = controller.GetComponent<Controller>();
        if (c.PositionOnBoard(x, y))
        {
            if (c.GetPosition(x, y) == null)
            {
                SpawnMoveTile(x,y);
            }
            if (c.PositionOnBoard(x+1,y) && c.GetPosition(x+1,y) != null && c.GetPosition(x+1,y).GetComponent<ChessPiece>().player != player)
            {
                SpawnMoveTile(x + 1, y, true);
            }
            if (c.PositionOnBoard(x - 1, y) && c.GetPosition(x - 1, y) != null && c.GetPosition(x - 1, y).GetComponent<ChessPiece>().player != player)
            {
                SpawnMoveTile(x - 1, y, true);
            }
        }
    }

    public void SpawnMoveTile(int _x, int _y, bool _attack = false)
    {
        float x = _x;
        float y = _y;

        x *= 0.75f;
        y *= 0.75f;

        x += -2.25f;
        y += -2.25f;

        GameObject mt = Instantiate(moveTile, new Vector3(x,y,-3.0f), transform.rotation);
        MoveTile mts = mt.GetComponent<MoveTile>();
        mts.attack = _attack;
        mts.SetReference(gameObject);
        mts.SetCoords(_x, _y);
    }
}

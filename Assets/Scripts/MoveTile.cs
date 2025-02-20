using UnityEngine;

public class MoveTile : MonoBehaviour
{
    public GameObject controller;

    GameObject reference = null;

    int posX;
    int posY;

    public bool attack = false;

    public void OnMouseUp()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");
        if (attack == true)
        {
            GameObject cp = controller.GetComponent<Controller>().GetPosition(posX, posY);
            controller.GetComponent<Controller>().PlayAudioClip("capture_piece");
            Destroy(cp);
        }
        else
        {
            controller.GetComponent<Controller>().PlayAudioClip("move_piece");
        }
            controller.GetComponent<Controller>().SetPositionEmpty(reference.GetComponent<ChessPiece>().GetXPos(),
            reference.GetComponent<ChessPiece>().GetYPos());
        reference.GetComponent<ChessPiece>().SetXPos(posX);
        reference.GetComponent<ChessPiece>().SetYPos(posY);
        reference.GetComponent<ChessPiece>().SetPosition();
        controller.GetComponent<Controller>().SetPosition(reference);
        reference.GetComponent<ChessPiece>().DestroyMoveTiles();
        if (controller.GetComponent<Controller>().GetCurrentPlayer() == "white" && posY == 5)
        {
            controller.GetComponent<Controller>().Winner("White");
            controller.GetComponent<Controller>().PlayAudioClip("game_over");
        }
        else if (controller.GetComponent<Controller>().GetCurrentPlayer() == "black" && posY == 0)
        {
            controller.GetComponent<Controller>().Winner("Black");
            controller.GetComponent<Controller>().PlayAudioClip("game_over");
        }
        controller.GetComponent<Controller>().NextTurn();
    }

    public void SetCoords(int x, int y)
    {
        posX = x; posY = y;
    }

    public void SetReference(GameObject reference)
    {
        this.reference = reference;
    }

    public GameObject GetReference() { return reference; }

}

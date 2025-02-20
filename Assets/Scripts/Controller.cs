using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    public int board_width = 6;
    public int board_height = 6;
    public float tile_size = 0.75f;

    public GameObject whiteTile;
    public GameObject blackTile;
    public GameObject chesspiece;

    public AudioClip gameOverSound;
    public AudioClip movePieceSound;
    public AudioClip capturePieceSound;

    private GameObject[,] positions = new GameObject[6,6];
    private GameObject[] player_black = new GameObject[8];
    private GameObject[] player_white = new GameObject[8];

    private string currentPlayer = "white";

    private bool gameOver = false;

    void Start()
    {
        GenerateBoard();
        player_white = new GameObject[]
        {
            Create("white_queen",0,0),Create("white_queen",1,0),Create("white_queen",2,0),
            Create("white_queen",3,0),Create("white_queen",4,0),Create("white_queen",5,0),
            Create("white_pawn",0,1),Create("white_pawn",1,1),Create("white_pawn",2,1),
            Create("white_pawn",3,1),Create("white_pawn",4,1),Create("white_pawn",5,1)
        };
        player_black = new GameObject[]
        {
            Create("black_queen",0,5),Create("black_queen",1,5),Create("black_queen",2,5),
            Create("black_queen",3,5),Create("black_queen",4,5),Create("black_queen",5,5),
            Create("black_pawn",0,4),Create("black_pawn",1,4),Create("black_pawn",2,4),
            Create("black_pawn",3,4),Create("black_pawn",4,4),Create("black_pawn",5,4)
        };
        for (int i = 0; i < player_white.Length; i++)
        {
            SetPosition(player_white[i]);
            SetPosition(player_black[i]);
        }
    }

    public void GenerateBoard()
    {
        var tile_number = 1;
        for (int x = 0; x < board_width; x++)
        {
            for (int y = 0; y < board_height; y++)
            {
                //code for grid spaces
                tile_number++;
                if (tile_number % 2 == 0)
                {
                    var spawned_tile = Instantiate(whiteTile, new Vector3(transform.position.x + x * tile_size, transform.position.y + y * tile_size, transform.position.z), transform.rotation);
                }
                else
                {
                    var spawned_tile = Instantiate(blackTile, new Vector3(transform.position.x + x * tile_size, transform.position.y + y * tile_size, transform.position.z), transform.rotation);
                }

            }
            tile_number++;
        }
    }

    public GameObject Create(string name, int x, int y)
    {
        GameObject obj = Instantiate(chesspiece, new Vector3(0, 0, -4), transform.rotation);
        ChessPiece cp = obj.GetComponent<ChessPiece>();
        cp.name = name;
        cp.SetXPos(x);
        cp.SetYPos(y);
        cp.SetPiece();
        return obj;
    }

    public void SetPosition(GameObject obj)
    {
        ChessPiece cp = obj.GetComponent<ChessPiece>();
        positions[cp.GetXPos(), cp.GetYPos()] = obj;
    }

    public void SetPositionEmpty(int x, int y)
    {
        positions[x, y] = null;
    }

    public GameObject GetPosition(int x, int y)
    {
        return positions[x, y];
    }

    public void NextTurn()
    {
        if (currentPlayer == "white")
        {
            currentPlayer = "black";
        }
        else
        {
            currentPlayer = "white";
        }
    }

    public bool IsGameOver()
    {
        return gameOver;
    }

    public string GetCurrentPlayer()
    {
        return currentPlayer;
    }

    public bool PositionOnBoard(int x, int y)
    {
        if(x<0 || y < 0 || x>= positions.GetLength(0) || y >= positions.GetLength(1)) return false;
        return true;
    }

    public void Winner(string playerWinner)
    {
        gameOver = true;
        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().enabled = true;
        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().text = playerWinner + " is the winner!";
        GameObject.FindGameObjectWithTag("RestartText").GetComponent<Text>().enabled = true;
    }

    public void Update()
    {
        if (gameOver && Input.GetMouseButtonDown(0))
        {
            gameOver = false;

            SceneManager.LoadScene("ChessGame");
        }
    }
    public void PlayAudioClip(string name)
    {
        switch(name){
            case "game_over":
                GetComponent<AudioSource>().PlayOneShot(gameOverSound);
                break;
            case "move_piece":
                GetComponent<AudioSource>().PlayOneShot(movePieceSound);
                break;
            case "capture_piece":
                GetComponent<AudioSource>().PlayOneShot(capturePieceSound);
                break;
        }
    }
}

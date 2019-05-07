using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    public int turnsRemaining;
    //TextMesh turnsText;
    //public int xPos, yPos;
    public Vector2 playerPos;
    public GridManager gm;

    public Vector3 startPosition;
    public Vector3 destPosition;
    private bool inSlide = false;
    void Start()
    {
        // This is probably garbage
        gm = GameObject.Find("GameManager").GetComponent<GridManager>();
        //turnsText = this.gameObject.transform.GetChild(0).gameObject.GetComponent<TextMesh>();
        //resetTurns(100);
    }

    void Update ()
    {   
        if (GridManager.slideLerp < 0)
        {
            if (Input.GetKeyDown(KeyCode.W)||Input.GetKeyDown(KeyCode.UpArrow))
            {
                Move(0, -1);
            }
            if (Input.GetKeyDown(KeyCode.S)||Input.GetKeyDown(KeyCode.DownArrow))
            {
                Move(0, 1);
            }
            if (Input.GetKeyDown(KeyCode.A)||Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Move(1, 0);
            }
            if (Input.GetKeyDown(KeyCode.D)||Input.GetKeyDown(KeyCode.RightArrow))
            {
                Move(-1, 0);
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                // Debug.Log("hi");
                //BackgroundAudio.Instance.PlaySound("click");
                RotateCW();
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                // Debug.Log("hi");
                //BackgroundAudio.Instance.PlaySound("click");
                RotateCCW();
            }
        }
        if (inSlide)
        {
            if (GridManager.slideLerp < 0)
            {
                transform.localPosition = destPosition;
                inSlide = false;
            }
            else
            {
                transform.localPosition = Vector3.Lerp(startPosition, destPosition, GridManager.slideLerp);
            }
        }
    }
    
    void Move (int x, int y)
    {   
        Vector2 oldLoc = new Vector2(playerPos.x, playerPos.y);
        Vector2 newLoc = new Vector2(playerPos.x + x, playerPos.y + y);
        if ((int) newLoc.x < GridManager.WIDTH - 1 &&
            (int) newLoc.x >= 0 &&
            (int) newLoc.y < GridManager.HEIGHT - 1 &&
            (int) newLoc.y >= 0)
        {
            playerPos = newLoc;
            GridManager.slideLerp = 0.3f;
            SetupSlide(GridManager.tiles[(int) newLoc.x, (int) newLoc.y].transform.localPosition);
        }
    }

    public void SetupSlide(Vector2 newDestPos)
    {
        inSlide = true;
        startPosition = transform.localPosition;
        destPosition = newDestPos;
        //this.gameObject.name = this.gameObject.name + " " + destPosition.x + " " + destPosition.y + "|";
    }

    void RotateCW()
    {   
        //The player is anchored on this block
        int tempX = (int) playerPos.x;
        int tempY = (int) playerPos.y;
        // Debug.Log(tempX + " " + tempY);
        GameObject temp = GridManager.tiles[tempX, tempY];
        // Vector2 tempPos = GridManager.tiles[tempX, tempY].transform.localPosition;
        /*
         * (0, 0), (0, 1), (1, 1), (1, 0)
         * (0, 0) <- (1, 0)
         * (1, 0) <- (1, 1)
         * (1, 1) <- (0, 1)
         * (0, 1) <- (0, 0)
         */

        // GridManager.tiles[tempX, tempY].transform.localPosition = GridManager.tiles[tempX + 1, tempY].transform.localPosition;
        // GridManager.tiles[tempX + 1, tempY].transform.localPosition = GridManager.tiles[tempX + 1, tempY + 1].transform.localPosition;
        // GridManager.tiles[tempX + 1, tempY + 1].transform.localPosition = GridManager.tiles[tempX, tempY + 1].transform.localPosition;
        // GridManager.tiles[tempX, tempY + 1].transform.localPosition = tempPos;
        // SwapTransformHelper(GridManager.tiles[tempX, tempY], GridManager.tiles[tempX, tempY + 1]);
        // SwapTransformHelper(GridManager.tiles[tempX + 1, tempY], GridManager.tiles[tempX + 1, tempY + 1]);
        // SwapTransformHelper(GridManager.tiles[tempX + 1, tempY], GridManager.tiles[tempX, tempY + 1]);
        LerpRotate(GridManager.tiles[tempX, tempY], 
                GridManager.tiles[tempX, tempY + 1], 
                GridManager.tiles[tempX + 1, tempY + 1], 
                GridManager.tiles[tempX + 1, tempY], "cw");


        GridManager.tiles[tempX, tempY] = GridManager.tiles[tempX + 1, tempY];
        GridManager.tiles[tempX + 1, tempY] = GridManager.tiles[tempX + 1, tempY + 1];
        GridManager.tiles[tempX + 1, tempY + 1] = GridManager.tiles[tempX, tempY + 1];
        GridManager.tiles[tempX, tempY + 1] = temp;

        
        //gm.ToString();


    }

    void RotateCCW()
    {   
        //The player is anchored on this block
        int tempX = (int) playerPos.x;
        int tempY = (int) playerPos.y;
        GameObject temp = GridManager.tiles[tempX, tempY];
        LerpRotate(GridManager.tiles[tempX, tempY], 
                GridManager.tiles[tempX, tempY + 1], 
                GridManager.tiles[tempX + 1, tempY + 1], 
                GridManager.tiles[tempX + 1, tempY], "rcw");


        GridManager.tiles[tempX, tempY] = GridManager.tiles[tempX, tempY + 1];
        GridManager.tiles[tempX, tempY + 1] = GridManager.tiles[tempX + 1, tempY + 1];
        GridManager.tiles[tempX + 1, tempY + 1] = GridManager.tiles[tempX + 1, tempY];
        GridManager.tiles[tempX + 1, tempY] = temp;

        
        //gm.ToString();


    }
    //deprecated
    // public void decrementTurns()
    // {
    //     turnsRemaining--;
    //     //turnsText.text = "" + turnsRemaining;
    // }

    public void EndGame(){
        Debug.Log("Game over!");
        SceneManager.LoadScene("EndScreen");
    }

    void SwapTransformHelper(GameObject g1, GameObject g2) {
        // Vector2 g1Pos = g1.transform.localPosition;
        // g1.transform.localPosition = g2.transform.localPosition;
        // g2.transform.localPosition = g1Pos;
        TileScript ts1 = g1.GetComponent<TileScript>();
        TileScript ts2 = g2.GetComponent<TileScript>();
        GridManager.slideLerp = 0;
        ts1.SetupSlide(g2.transform.localPosition);
        ts2.SetupSlide(g1.transform.localPosition);
    }

    // g1 is UR, g2 is BR, g3 is LR, g4 is UR
    //This one does the real in-scene rotation
    void LerpRotate(GameObject g1, GameObject g2, GameObject g3, GameObject g4, string d){
        TileScript ts1 = g1.GetComponent<TileScript>();
        TileScript ts2 = g2.GetComponent<TileScript>();
        TileScript ts3 = g3.GetComponent<TileScript>();
        TileScript ts4 = g4.GetComponent<TileScript>();
        Vector2 temp = g1.transform.localPosition;
        GridManager.slideLerp = 0.1f;
        if (string.Equals(d, "cw"))
        {
            ts1.SetupSlide(g2.transform.localPosition);
            ts2.SetupSlide(g3.transform.localPosition);
            ts3.SetupSlide(g4.transform.localPosition);
            ts4.SetupSlide(temp);
        }
        else
        {
            ts1.SetupSlide(g4.transform.localPosition);
            ts2.SetupSlide(temp);
            ts3.SetupSlide(g2.transform.localPosition);
            ts4.SetupSlide(g3.transform.localPosition);
        }
        //gm.ToString();
    }
}

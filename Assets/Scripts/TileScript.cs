using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    public int type;
    public Color[] tileColors = 
    {
        Color.red,
        new Color(0.96f, 0.63f, 0f, 1.0f), // Orange
        Color.yellow,
        Color.green,
        Color.blue,
        new Color(0.65f, 0f, 0.96f, 1.0f), // Violet
        Color.white,
        Color.gray
    };
    void Awake()
    {   
        //This does some math on the colors, effectively makes them more pastel-y
        for (int q = 0; q < tileColors.Length; q++){
            tileColors[q] = new Color(0.5f, 0.5f, 0.5f, 1.0f) + 0.5f * tileColors[q];
        }
        transform.localScale = new Vector3(0, 0, 0);
    }

    // public Sprite[] tileColors;
    public Sprite[] plantSprites;

    public Vector3 startPosition;
    public Vector3 destPosition;
    private bool inSlide = false;
    private bool isSlerp = false;

    void FixedUpdate(){
        if (inSlide){
            if (GridManager.slideLerp < 0)
            {
                transform.localPosition = destPosition;
                inSlide = false;
                transform.GetChild(0).SendMessage("BeginContact");
            }
            else if (isSlerp)
            {
                transform.localPosition = Vector3.Slerp(startPosition, destPosition, GridManager.slideLerp);
                if (Vector3.Distance(transform.localPosition, destPosition) == 0) {
                    //Debug.Log("nlerp");
                    isSlerp = false;
                }
            }
            else
            {
                transform.localPosition = Vector3.Lerp(startPosition, destPosition, GridManager.slideLerp);
            }
        }
        else if (transform.localScale != Vector3.one && GridManager.slideLerp < 0) {

            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, 0.25f);
        }
    }
    public void SetSprite(int rand){
        type = rand;
        //GetComponent<SpriteRenderer>().sprite = tileSprites[type];
        if (rand >= 0)
            GetComponent<SpriteRenderer>().color = tileColors[type];
            GetComponent<SpriteRenderer>().sprite = plantSprites[Random.Range(0, plantSprites.Length)];
    }

    public bool IsMatch(GameObject gameObject1, GameObject gameObject2){
        TileScript ts1 = gameObject1.GetComponent<TileScript>();
        TileScript ts2 = gameObject2.GetComponent<TileScript>();
        /* if (ts1 != null && ts2 != null && type == ts1.type && type == ts2.type)
            Debug.Log(type + " " + ts1.type + " " + ts2.type); */
        return ts1 != null && ts2 != null && type == ts1.type && type == ts2.type;
    }

    public void SetupSlide(Vector2 newDestPos){
        inSlide = true;
        startPosition = transform.localPosition;
        destPosition = newDestPos;
        //this.gameObject.name = this.gameObject.name + " " + destPosition.x + " " + destPosition.y + "|";
    }

    public void SetupSlerp(Vector2 newDestPos){
        SetupSlide(newDestPos);
        isSlerp = true;
    }
}

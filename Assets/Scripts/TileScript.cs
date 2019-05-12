using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    public bool isSeed = false;
    public int type;

    public Vector2 gridLoc;
    public Color[] tileColors = 
    {
        Color.red,
        //new Color(0.96f, 0.63f, 0f, 1.0f), // Orange
        Color.yellow,
        Color.green,
        Color.blue,
        new Color(0.65f, 0f, 0.96f, 1.0f), // Violet
        Color.white,
        //Color.gray
    };
    public Sprite[] plantSprites;
    public Sprite seedling;

    SpriteRenderer tileSprite;

    public Vector3 startPosition;
    public Vector3 destPosition;
    private bool inSlide = false;
    //private bool isSlerp = false;
    float swayOffset;
    void Awake()
    {   
        swayOffset = Random.Range(0.0f, 2.0f);
        tileSprite = GetComponent<SpriteRenderer>();
        //This does some math on the colors, effectively makes them more pastel-y
        for (int q = 0; q < tileColors.Length; q++){
            tileColors[q] = new Color(0.5f, 0.5f, 0.5f, 1.0f) + 0.5f * tileColors[q];
        }
        transform.localScale = new Vector3(0, 0, 0);
    }



    void Update(){
        if (inSlide)
        {
            if (GridManager.slideLerp < 0)
            {
                transform.localPosition = destPosition;
                inSlide = false;
                //transform.GetChild(0).SendMessage("BeginContact");
            }
            // else if (isSlerp)
            // {
            //     transform.localPosition = Vector3.Slerp(startPosition, destPosition, GridManager.slideLerp);
            //     if (Vector3.Distance(transform.localPosition, destPosition) == 0) {
            //         //Debug.Log("nlerp");
            //         isSlerp = false;
            //     }
            // }
            else
            {
                transform.localPosition = Vector3.Lerp(startPosition, destPosition, GridManager.slideLerp);
            }
        }
        else if (transform.localScale != Vector3.one) 
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Random.Range(0.0f, 0.1f));
        }
        if (!isSeed)
        {   
            transform.rotation *= Quaternion.Euler( 0, 0, Mathf.Sin(Time.time + swayOffset) * 0.2f);
            //transform.GetChild(0).rotation *= Quaternion.Inverse(Quaternion.Euler( 0, 0, Mathf.Sin(Time.time * 2 + swayOffset) * 0.3f));
        }
    }
    public void SetSprite(int rand){
        type = rand;
        //GetComponent<SpriteRenderer>().sprite = tileSprites[type];
        if (rand >= 0)
        {
            tileSprite.color = tileColors[type];
            tileSprite.sprite = plantSprites[Random.Range(0, plantSprites.Length)];
        }
        if (Random.Range(0.0f, 1.0f) < 0.25f)
        {
            isSeed = true;
            this.tag = "seed";
            tileSprite.sprite = seedling;
            tileSprite.color *= new Color(0.8f, 0.8f, 0.8f, 1.0f);
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public bool IsMatch(GameObject gameObject1, GameObject gameObject2){
        TileScript ts1 = gameObject1.GetComponent<TileScript>();
        TileScript ts2 = gameObject2.GetComponent<TileScript>();
        /* if (ts1 != null && ts2 != null && type == ts1.type && type == ts2.type)
            Debug.Log(type + " " + ts1.type + " " + ts2.type); */
        return ts1 != null && ts2 != null && type == ts1.type && type == ts2.type && !ts1.isSeed && !ts2.isSeed && !isSeed;
    }

    public void SetupSlide(Vector2 newDestPos){
        inSlide = true;
        startPosition = transform.localPosition;
        destPosition = newDestPos;
        //this.gameObject.name = this.gameObject.name + " " + destPosition.x + " " + destPosition.y + "|";
    }

    //Grows a "seedling" into a full plant
    public void GrowUp() {
        isSeed = false;
        this.tag = "Untagged";
        tileSprite.color = tileColors[type];
        tileSprite.sprite = plantSprites[Random.Range(0, plantSprites.Length)];
        transform.localScale = Vector3.one * 0.5f;
        transform.GetChild(0).gameObject.SetActive(false);

        return;
    }
}

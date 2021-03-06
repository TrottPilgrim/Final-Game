﻿using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    //public static GridManager Instance;
    public static GameObject[,] tiles;
    [Header("Prefabs")]
    public GameObject tilePrefab;
    public GameObject playerFab;
    public GameObject flowerFab;
    public GameObject gm;

    [Header("Boilerplate Match Game Code")]
    public const int WIDTH = 9;
    public const int HEIGHT = 7;

    public const float xOffset = WIDTH / 2f - 0.5f;
    public const float yOffset = HEIGHT / 2f - 0.5f;
    
    GameObject gridHolder;
    public GameObject explosion;
    PlayerScript playerScript;
    public static float slideLerp = -1f;
    [Header("Lerp + Score")]
    public float lerpSpeed;
    
    //Score + text
    private int score;
    public Text scoreText;
    // Keeps track of what the game's state is at the moment so that Update doesn't destroy the game
    bool changedState = false;
    bool transitioning = false;
    bool hasNotPlayedEnding = true;
    [Header("UI Elements")]
    public Image[] backgrounds;
    public Slider scoreMeter;
    public Image endScreen;

    //public bool growth;

    void Start()
    {
        lerpSpeed = 0.25f;
        
        tiles = new GameObject[WIDTH, HEIGHT];
        gridHolder = new GameObject();
        gridHolder.transform.position = new Vector3(-1f, -1f, 0);
        
        for (int i = 0; i < WIDTH; i++){
            for (int j = 0; j < HEIGHT; j++){
                GameObject newTile = Instantiate(tilePrefab);
                newTile.name = i + "," + j;
                newTile.transform.parent = gridHolder.transform;

                //WHY does this matrix go from upper right to bottom left. Is this supposed to happen?
                //I know why, it's because then you can just use Input.GetAxis for movement.
                newTile.transform.localPosition = new Vector2(WIDTH - i - xOffset, HEIGHT - j - yOffset);
                //newTile.transform.localPosition = new Vector2( i - 1, HEIGHT - j - yOffset);

                tiles[i, j] = newTile;
                TileScript tileScript = newTile.GetComponent<TileScript>();
                tileScript.SetSprite(Random.Range(0, tileScript.tileColors.Length));
                tileScript.gridLoc = new Vector2(i, j);
            }
        }
        score = 0;
        //Changes tiles that are part of matches as a part of board generation. Potentially will run forever
        //Also probably not cache friendly or something
        while (HasMatch()){
            TileScript temp = HasMatch();
            temp.SetSprite(Random.Range(0, temp.tileColors.Length));
        }
        
        //Initializing the player object
        GameObject player = Instantiate(playerFab);
        playerScript = player.GetComponent<PlayerScript>();
        player.transform.parent = gridHolder.transform;
        player.transform.localPosition = new Vector2(WIDTH - xOffset, HEIGHT - yOffset);
        // for (int i = 0; i < 1000; i++)
        // {
        //     GrowOuterPlant();
        // }
    }

    
    void FixedUpdate()
    {
        if (scoreMeter.value < score) {
            scoreMeter.value += Time.deltaTime / lerpSpeed;
        }
        if (slideLerp < 0 && !Repopulate() && HasMatch()){
            RemoveMatches();
        }
        else if (slideLerp >= 0)
        {
            slideLerp += Time.deltaTime / lerpSpeed;
            //Debug.Log(slideLerp);
            // Debug.Log(Time.deltaTime + " " + slideLerp + " Lerp speed: " + lerpSpeed);

            if (slideLerp >= 1)
                slideLerp = -1;
        }
        if (transitioning)
        {
            LerpBtwBackgrounds(1);
        }
        if (changedState && !BackgroundAudio.Instance.GetComponents<AudioSource>()[1].isPlaying && hasNotPlayedEnding)
        {
            Debug.Log("Pop up game ending");
            hasNotPlayedEnding = false;
            endScreen.gameObject.SetActive(true);
            BackgroundAudio.Instance.PlaySound("greatjob");
            
        }
    }
    //Hasmatch returns an object that has a matching object vertically or horizontally
    public TileScript HasMatch(){
        for (int x = 0; x < WIDTH; x++){
            for (int y = 0; y < HEIGHT; y++){
                TileScript temp = tiles[x, y].GetComponent<TileScript>();
                if (temp is object){
                    if (x < WIDTH - 2 && temp.IsMatch(tiles[x + 1, y], tiles[x + 2, y]))
                        return temp;
                    if (y < HEIGHT - 2 && temp.IsMatch(tiles[x, y + 1], tiles[x, y + 2]))
                        return temp;
                }
            }
        }
        return null;
    }

    public void RemoveMatches(){
        for (int x = 0; x < WIDTH; x++){
            for (int y = 0; y < HEIGHT; y++){
                TileScript temp = tiles[x, y].GetComponent<TileScript>();
                if (temp is object){
                    if (x < WIDTH - 2 && temp.IsMatch(tiles[x + 1, y], tiles[x + 2, y]))
                    {
                        BackgroundAudio.Instance.PlaySound("pop");
                        ParticleSystem e0 = Instantiate(explosion, tiles[x, y].transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
                        ParticleSystem e1 = Instantiate(explosion, tiles[x + 1, y].transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
                        ParticleSystem e2 = Instantiate(explosion, tiles[x + 2, y].transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
                        e0.Play(); e1.Play(); e2.Play();
                        score += 1;
                        //Debug.Log("Horizontal Match " + x + " " + y);
                        Destroy(tiles[x, y]);
                        Destroy(tiles[x + 1, y]);
                        Destroy(tiles[x + 2, y]);
                        Destroy(e0.gameObject, 3f);
                        Destroy(e1.gameObject, 3f);
                        Destroy(e2.gameObject, 3f);
                        scoreText.text = "SCORE: " + score;
                        GrowPlants(temp.type);
                        GrowOuterPlant();
                        //transform.GetChild(0).SendMessage("BeginContact");
                        //playerScript.resetTurns(6);
                        // This is an extremely hacky implementation of transitioning between screens
                        if (score == 20 && !changedState){
                            TransitionToNextState();
                        }
                    }
                    if (y < HEIGHT - 2 && temp.IsMatch(tiles[x, y + 1], tiles[x, y + 2]))
                    {   
                        BackgroundAudio.Instance.PlaySound("pop");
                        ParticleSystem e0 = Instantiate(explosion, tiles[x, y].transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
                        ParticleSystem e1 = Instantiate(explosion, tiles[x, y + 1].transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
                        ParticleSystem e2 = Instantiate(explosion, tiles[x, y + 2].transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
                        e0.Play(); e1.Play(); e2.Play();
                        score += 1;
                        //Debug.Log("Vertical Match " + x + " " + y);
                        Destroy(tiles[x, y]);
                        Destroy(tiles[x, y + 1]);
                        Destroy(tiles[x, y + 2]);
                        Destroy(e0.gameObject, 3f);
                        Destroy(e1.gameObject, 3f);
                        Destroy(e2.gameObject, 3f);
                        scoreText.text = "SCORE: " + score;
                        GrowPlants(temp.type);
                        GrowOuterPlant();
                        //transform.GetChild(0).SendMessage("BeginContact");
                        //playerScript.resetTurns(6);
                        if (score == 20 && !changedState){
                            TransitionToNextState();
                        }
                    }
                }
            }
        }
    }

    public bool Repopulate(){
        bool repop = false;
        for (int x = 0; x < WIDTH; x++){
            for (int y = 0; y < HEIGHT; y++){
                //Check if there is an empty spot in the array
                if (tiles[x, y] == null){
                    //Instantiates the new flowers at the top and lerps them down
                    //The game knows we're about to repopulate
                    repop = true;
                    //Check if we're on the top row of the grid
                     if (y == 0){
                        //If so, make a token
                        tiles[x, y] = Instantiate(tilePrefab);
                        tiles[x, y].name = x + "," + y;
                        TileScript tileScript = tiles[x, y].GetComponent<TileScript>();
                        tileScript.SetSprite(Random.Range(0, tileScript.tileColors.Length));
                        tiles[x, y].transform.parent = gridHolder.transform;
                        tiles[x, y].transform.localPosition = new Vector2 (WIDTH - x - xOffset, HEIGHT - y - yOffset);

                    }
                    else {
                        slideLerp = 0.0f;
                        tiles[x, y] = tiles[x, y - 1];
                        TileScript tileScript = tiles[x, y].GetComponent<TileScript>();
                        if (tileScript != null){
                            tileScript.SetupSlide(new Vector2(WIDTH - x - xOffset, HEIGHT - y - yOffset));
                            //tileScript.gameObject.transform.localPosition = new Vector2(WIDTH - x - xOffset, HEIGHT - y - yOffset);
                            tiles[x, y - 1] = null;
                            //tiles[x, y].transform.localPosition = Vector3.Lerp(tileScript.startPosition, tileScript.destPosition, lerpSpeed);
                        }
                    }

                    // //Instantiates the new flowers in place
                    // tiles[x, y] = Instantiate(tilePrefab);
                    // TileScript tileScript = tiles[x, y].GetComponent<TileScript>();
                    // tileScript.SetSprite(Random.Range(0, tileScript.tileColors.Length));
                    // tiles[x, y].transform.parent = gridHolder.transform;
                    // tiles[x, y].transform.localPosition = new Vector2 (WIDTH - x - xOffset, HEIGHT - y - yOffset);
                }
            }
        }
        return repop;
    }

    void GrowPlants(int type) {
        GameObject[] seeds = GameObject.FindGameObjectsWithTag("seed");
        //Debug.Log(seeds);
        for (int  i = 0; i < seeds.Length; i++){
            if (seeds[i].GetComponent<TileScript>().type == type)
                seeds[i].SendMessage("GrowUp");
        }
    }

    //
    void TransitionToNextState()
    {
        //Debug.Log("I'm a believer");
        changedState = true;
        BackgroundAudio.Instance.nearingEnd.TransitionTo(3f);
        BackgroundAudio.Instance.GetComponents<AudioSource>()[1].PlayDelayed(3f);
        transitioning = true;
    }

    /*
        LerpBtwBackgrounds fades in one background and fades out the previous background
        x is used to indicate what background it is transitioning to
        (ie, calling with x = 1 goes to background 1 from the previous background 0)
     */
    void LerpBtwBackgrounds(int x)
    {
        Image bg0 = backgrounds[x - 1];
        Image bg1 = backgrounds[x];
        if (bg0.color.a > 0) {
            bg0.color -= new Color (0, 0, 0, 0.01f);
            bg1.color += new Color (0, 0, 0, 0.01f);
        }
        else
        {
            transitioning = false;
        }
    }

    //Grows decorative plants *outside* the border of the play area. 
    void GrowOuterPlant(){
        GameObject newFlower = Instantiate(flowerFab, gm.transform);
        Vector3 minXY = tiles[0,0].transform.position;
        float newX = Random.Range(0, 1f) > 0.5 ? 1 : -1;
        float newY = Random.Range(0, 1f) > 0.5 ? 1 : -1;
        newX *= minXY.x + Random.Range(1f, 4f);
        newY *= minXY.y + Random.Range(1f, 4f);
        float c = Random.Range(0, 1f);
        if (c > 0.66f) {
            newX = Random.Range (- WIDTH / 2, WIDTH / 2);
        }
        else if ( c > 0.33f) {
            newY = Random.Range ( - HEIGHT / 2, HEIGHT / 2);
        }
        newFlower.transform.position = new Vector3 (newX, newY, 0);
    }


    public override string ToString()
    {
        Debug.Log("matrix: ");
        for (int y = 0 ; y < HEIGHT; y++)
        
        {
            StringBuilder line = new StringBuilder();
            for (int x = WIDTH - 1; x >= 0; x--)
            {
                line.Append(tiles[x, y].GetComponent<TileScript>().type + "\t");
            }
            Debug.Log(line + "\n");
        }
        return "";
    }
}

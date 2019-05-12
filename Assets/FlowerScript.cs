using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerScript : MonoBehaviour
{
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
    float swayOffset;
    SpriteRenderer tileSprite;

    public Sprite[] plantSprites;
    void Awake()
    {   
        swayOffset = Random.Range(0.0f, 2.0f);
        tileSprite = GetComponent<SpriteRenderer>();
        //This does some math on the colors, effectively makes them more pastel-y
        for (int q = 0; q < tileColors.Length; q++){
            tileColors[q] = new Color(0.5f, 0.5f, 0.5f, 1.0f) + 0.5f * tileColors[q];
        }
        transform.localScale = new Vector3(0, 0, 0);
        tileSprite.color = tileColors[Random.Range(0, tileColors.Length)];
        tileSprite.sprite = plantSprites[Random.Range(0, plantSprites.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localScale != Vector3.one) {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime / 0.25f);
        }
        transform.rotation *= Quaternion.Euler( 0, 0, Mathf.Sin(Time.time + swayOffset) * 0.2f);
    }
}

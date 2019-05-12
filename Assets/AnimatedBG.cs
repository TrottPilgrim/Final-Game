using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimatedBG : MonoBehaviour
{
    public Sprite[] grassIcons;
    float timer;
    int currentSprite = 0;
    Image myBG;

    void Start()
    {
        myBG = GetComponent<Image>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 0.25)
        {
            //Debug.Log("hey");
            myBG.sprite = grassIcons[currentSprite];
            currentSprite++;
            if (currentSprite == grassIcons.Length)
            {
                currentSprite = 0;
            }
            timer = 0;
        }
    }
}

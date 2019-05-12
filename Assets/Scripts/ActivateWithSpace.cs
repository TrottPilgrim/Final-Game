using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivateWithSpace : MonoBehaviour
{
    Camera cam;
    public GameObject flowerFab;

    void Awake()
    {
        for (int i = 0; i < 100; i++)
        {
            cam = Camera.main;
            float camHeight = cam.orthographicSize;
            float camWidth = camHeight * cam.aspect;
            float x = Random.Range(-camWidth, camWidth);
            float y = Random.Range(-camHeight, camHeight);
            Vector3 pos = new Vector3 (x, y ,0);
            GameObject newFlower = Instantiate(flowerFab, pos, Quaternion.identity);
        }
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            GetComponent<Button>().onClick.Invoke();
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    UNUSED!! this script was written when I was working animating the background.
 */

public class OffsetScroller : MonoBehaviour
{
    public float scrollSpeed;
    private Vector2 savedOffset;

    // Start is called before the first frame update
    void Start()
    {
        savedOffset = GetComponent<MeshRenderer>().sharedMaterial.GetTextureOffset ("_MainTex");
    }

    // Update is called once per frame
    void Update()
    {
        float y = Mathf.Repeat(Time.time * scrollSpeed, 1);
        Vector2 offset = new Vector2(Mathf.Cos(Time.time), -Mathf.Sin(Time.time));
        GetComponent<MeshRenderer>().sharedMaterial.SetTextureOffset("_MainTex", offset);
        GetComponent<MeshRenderer>().sharedMaterial.SetColor("_Color", Color.black);
    }
}

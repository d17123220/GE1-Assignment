using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaucerMove : MonoBehaviour
{
    
    public void MoveDown(float units, float maxY)
    {
        // move down saucer
        transform.Translate(0.0f, -units, 0.0f);


        // recalculate colors
        foreach (Transform child in transform)
        {
            var Y = child.transform.position[1];
            float minY = 8.0f;
            var cubeColor = Color.HSVToRGB(0.33f + (1.0f - (Y - minY)/(maxY - minY))*0.67f, 1.0f, 1.0f);
            cubeColor.a = 0.9f;
            child.GetComponent<Renderer>().material.color = cubeColor;
        }
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

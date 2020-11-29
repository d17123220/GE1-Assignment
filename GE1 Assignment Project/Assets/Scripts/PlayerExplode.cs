﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExplode : MonoBehaviour
{

    public float maxVelocity = 50.0f;

    public void Explode()
    {
        // enumerate all children
        foreach (Transform child in transform)
        {
            // remove parent
            child.transform.parent = null;
            // remove collision box
            //child.GetComponent<Collider>().enabled = false;
            // add rigid body
            var rb = child.gameObject.AddComponent<Rigidbody>();
            rb.useGravity = true;
            rb.mass = 100.0f;
            rb.AddForce(Physics.gravity * rb.mass * rb.mass * 3);
            Debug.Log(Physics.gravity);
            rb.isKinematic = false;
            
            //add randomized volocity
            Vector3 v = new Vector3(
                Random.Range((float) -1 * maxVelocity / 2, (float) maxVelocity / 2)
                , Random.Range((float) maxVelocity, (float) maxVelocity * 2)
                , Random.Range((float) -1 * maxVelocity / 2, (float) maxVelocity / 2)
                );
            rb.velocity = v;

            // destroy shredded cubes after 6 seconds (even if they didn't fall to ground yet)
            Destroy(child.gameObject, 20);
        }

        // instantly destroy saucer object
        Destroy(gameObject);
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

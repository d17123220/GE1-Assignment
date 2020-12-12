using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Explode object:
    - make a random acceleration and direction of all child parts
    - remove child's collision box
    - add to child rigidbody
    - remove self from game objects
*/

public class SaucerExplode : MonoBehaviour
{
    public float maxVelocity = 50.0f;

    public bool isDestroyed = false;

    // use randomized delay because bullet can trigger multiple blocks to try to explode
    // use delay to set flag that it was destroyed correctly and ignore all additional triggers
    public void RandomExplode()
    {
        float delay = Random.Range(0.01f, 0.5f);
        Invoke("Explode",delay);
    }

    public void Explode()
    {
        // if already "explodes" - do nothing
        if (isDestroyed)
            return;
        
        isDestroyed = true;

        // repeat 8 times - for some reason only half of the objects are getting rigidbody on each iteration
        for (int i=0; i<8; i++)
        {
            // enumerate all children
            foreach (Transform child in transform)
            {
                // remove collision box
                child.GetComponent<Collider>().enabled = false;
                // add rigid body
                var rb = child.gameObject.GetComponent<Rigidbody>();
                rb.useGravity = true;
                rb.mass = 5.0f;
                rb.AddForce(Physics.gravity * rb.mass * rb.mass * 100);
                rb.isKinematic = false;
                
                //add randomized volocity
                Vector3 v = new Vector3(
                    Random.Range(-1 * maxVelocity *2, maxVelocity *2)
                    , Random.Range(0, maxVelocity)
                    , Random.Range(-1 * maxVelocity *2, maxVelocity *2)
                    );
                rb.velocity = v;

                // remove parent
                child.transform.parent = null;

                // destroy shredded cubes after 6 seconds (even if they didn't fall to ground yet)
                Destroy(child.gameObject, 6);
            }
        }
        
        // instantly destroy saucer object
        Destroy(gameObject,0.1f);
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

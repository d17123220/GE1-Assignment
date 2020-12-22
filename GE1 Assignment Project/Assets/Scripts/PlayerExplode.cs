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
public class PlayerExplode : MonoBehaviour
{
    public float maxVelocity = 50.0f;
    public bool isDestroyed = false;
    public bool readyForExplosion = false;
    public GameObject mainCamera;
    public GameObject newLight;

    // use randomized delay because bullet can trigger multiple blocks to try to explode
    // use delay to set flag that it was destroyed correctly and ignore all additional triggers
    public void RandomExplode()
    {
        float delay = Random.Range(0.01f, 0.5f);
        Invoke("BeginExplode",delay);
    }

    // add synematic camera jump
    public void BeginExplode()
    {
        if (isDestroyed)
            return;
        
        isDestroyed = true;

        // calculate where to move camera
        GameObject cage = GameObject.Find("/Cage");
        float x = cage.transform.localScale.x * 25 / 100;
        float z = cage.transform.localScale.z * 25 / 100;
        float xDirection, zDirection;
        if (transform.position.x > 0)
            xDirection = -1.0f;
        else
            xDirection = 1.0f;
    
        if (transform.position.z > 0)
            zDirection = -1.0f;
        else
            zDirection = 1.0f;
        
        Vector3 destination = new Vector3(xDirection * x, 25.0f, zDirection * z);
        Vector3 direction = destination - transform.position;
        direction.Normalize();
        direction *= 50.0f;
        destination = transform.position + direction;

        GameObject spotlight = GameObject.Instantiate<GameObject>(newLight);
        spotlight.transform.position = destination;
        spotlight.transform.LookAt(transform);
        //transform.LookAt(target.parent);

        mainCamera.gameObject.GetComponent<CameraBind>().MoveFromExplosion(destination);
        gameObject.GetComponent<PlayerBuilder>().destroyed = true;
    }

    System.Collections.IEnumerator Explode()
    {
        while (transform.childCount > 5)       
        {
            // enumerate all children
            foreach (Transform child in transform)
            {
                // skip if child is a spotlight
                if (child.gameObject.name.Contains("Spot"))
                    continue;

                // remove parent
                child.transform.parent = null;
                // remove collision box
                child.GetComponent<Collider>().enabled = false;
                // add rigid body
                var rb = child.gameObject.AddComponent<Rigidbody>();
                rb.useGravity = true;
                rb.mass = 100.0f;
                rb.isKinematic = false;
                
                //add randomized volocity
                Vector3 v = new Vector3(
                    Random.Range((float) -1 * maxVelocity / 2, (float) maxVelocity / 2)
                    , Random.Range((float) maxVelocity / 4, (float) maxVelocity / 2)
                    , Random.Range((float) -1 * maxVelocity / 2, (float) maxVelocity / 2)
                    );
                rb.velocity = v;

                // destroy shredded cubes after 10 seconds (even if they didn't fall to ground yet)
                Destroy(child.gameObject, 10.0f);
            }
            yield return new WaitForSeconds(0.15f);
        }

        mainCamera.GetComponent<CameraBind>().isInFreeMove = true;
        mainCamera.GetComponent<PlayerShooting>().canShoot = false;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (readyForExplosion)
        {
            readyForExplosion = false;
            StartCoroutine(Explode());
        }
    }
}

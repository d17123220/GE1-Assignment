using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotFlight : MonoBehaviour
{
    public float bulletSpeed = 100.0f;
    
    // get collision with other objects
    public void OnCollisionEnter(Collision collision)
    {
        // destroy this shot
        Destroy(gameObject);

        // if collided with saucer
        if (collision.gameObject.name.Contains("AlienCube"))
        {
            collision.gameObject.transform.parent.gameObject.GetComponent<SaucerExplode>().RandomExplode();
        }


        
        

    }

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 10);
    }

    // Update is called once per frame
    void Update()
    {
        // fly up (relatevly)
        transform.Translate(0, bulletSpeed * Time.deltaTime, 0);

        // check if it flew outside of the box
        GameObject cage = GameObject.Find("/Cage");
        if (transform.position[1] > cage.transform.localScale[1] ||
            transform.position[1] < 0 ||
            transform.position[0] < -cage.transform.localScale[0] / 2 ||
            transform.position[0] > cage.transform.localScale[0] / 2 ||
            transform.position[2] < -cage.transform.localScale[2] / 2 ||
            transform.position[2] > cage.transform.localScale[2] /2 )
        {
            Destroy(gameObject);
        }
    }
}

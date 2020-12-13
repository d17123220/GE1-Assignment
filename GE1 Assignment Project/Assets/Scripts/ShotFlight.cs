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
        if (transform.position.y > cage.transform.localScale.y ||
            transform.position.y < 0 ||
            transform.position.x < -cage.transform.localScale.x / 2 ||
            transform.position.x > cage.transform.localScale.x / 2 ||
            transform.position.z < -cage.transform.localScale.z / 2 ||
            transform.position.z > cage.transform.localScale.z /2 )
        {
            Destroy(gameObject);
        }
    }
}

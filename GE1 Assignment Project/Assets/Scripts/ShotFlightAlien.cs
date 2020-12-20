using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotFlightAlien : MonoBehaviour
{
    public float bulletSpeed = 25.0f;
    
    // get collision with other objects
    public void OnCollisionEnter(Collision collision)
    {


        // if collided with player's tower
        if (collision.gameObject.name.Contains("DefenderCube"))
        {
            // destroy this shot
            Destroy(gameObject);
            collision.gameObject.transform.parent.gameObject.GetComponent<PlayerExplode>().RandomExplode();
        }
    }


    // Start is called before the first frame update
    void Start()
    {
         //Destroy(this.gameObject, 30);
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

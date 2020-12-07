using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    // define prefab for shot
    public GameObject shotPrefab;

    // define state
    public bool canShoot = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if player can shoot and 
        if (canShoot && Input.GetButton("Fire1"))
        {
            // check if there is no shot exist
            if (GameObject.Find("/Shot(Clone)") == null)
            {
                // set spawning point 5 units away from camera
                Vector3 spawnPoint = transform.position + 5 * transform.forward;
                
                // spawn shot
                GameObject shot = GameObject.Instantiate<GameObject>(shotPrefab);
                // position it at spawn point
                shot.transform.position = spawnPoint;
                // rotate accordingly
                shot.transform.rotation = transform.rotation;
                shot.transform.Rotate(Vector3.right, 90.0f);
            }
        }
    }
}

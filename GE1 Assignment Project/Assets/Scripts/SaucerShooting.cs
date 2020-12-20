using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaucerShooting : MonoBehaviour
{
    // define prefab for shot
    public GameObject shotPrefab;
    public float prefabsize = 1.0f;
    public float saucerheight = 8.0f;

    // whome to shoot
    public GameObject target;

    // Shoot him!
    public void Shoot()
    {
        // set spawning point 5 units away from camera
        Vector3 spawnPoint = transform.position - (saucerheight + 2) * prefabsize * transform.up;
        
        // spawn shot
        GameObject shot = GameObject.Instantiate<GameObject>(shotPrefab);
        // position it at spawn point
        shot.transform.position = spawnPoint;
        // rotate accordingly
        Vector3 direction = target.transform.position + new Vector3(0.0f,10.0f,0.0f) - transform.position;
        Quaternion rotateTo = Quaternion.LookRotation(direction);
        
        shot.transform.rotation = rotateTo;
        shot.transform.Rotate(Vector3.right, 90.0f);
    }



    // Start is called before the first frame update
    void Start()
    {
        target = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

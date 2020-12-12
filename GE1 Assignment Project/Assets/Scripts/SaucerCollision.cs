using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaucerCollision : MonoBehaviour
{
    // get collision with other objects
    public void OnCollisionEnter(Collision collision)
    {
        // if collided with saucer
        if (collision.gameObject.name.Contains("Wall"))
        {
            transform.parent.transform.parent.gameObject.GetComponent<FleetBuilder>().RandomCollisionNotification();
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

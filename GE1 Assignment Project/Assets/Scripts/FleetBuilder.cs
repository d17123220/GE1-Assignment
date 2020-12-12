using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Build alien fleet out of placeholder objects
*/
public class FleetBuilder : MonoBehaviour
{
    // Define prefabricated placeholders
    public GameObject smallAlien;
    public GameObject mediumAlien;
    public GameObject bigAlien;
    public GameObject motherShip;
    public float prefabsize = 1.0f;
    public static float saucerWidth = 12.0f;
    public static float saucerDepth = 6.0f; 
    public static float saucerHeight = 8.0f;   
    public float horizontalSpacer = saucerWidth / 2;
    public float verticalSpacer = saucerHeight;
    public float depthSpacer = saucerDepth;
    
    // Define local constants
    private const int SAUCER_SMALL = 0;
    private const int SAUCER_MEDIUM = 1;
    private const int SAUCER_BIG = 2;
    private const int MOTHER_SHIP = 3;

    // define initial movement speed in bricks per second
    public float moveSpeed = 3;
    public bool ready = false;
    public bool destroyed = false;
    private float xDirection = 1.0f;
    private bool collisionNotification = false;
    
    // define fleet dimentions
    public int fleetWidth = 5;
    public int fleetDepth = 3;

    // define camera to move it to player's tower
    public GameObject mainCamera;

    // define how alien saucers are located in the fleet
    public int[] fleetHeight = {SAUCER_BIG, SAUCER_BIG, SAUCER_MEDIUM, SAUCER_MEDIUM, SAUCER_SMALL};

    private Vector3 CalculateRelativePosition(int i, int j, int k)
    {
        // vertical coordinate Y
        float fullY = fleetHeight.Length;
        float Y = (float) ((-1) * fullY + i) * saucerHeight + ((-1) * (fullY-1) +i + 0.5f) * verticalSpacer;
        
        // forward coordinate Z
        float halfZ = fleetDepth / 2;
        float halfSpaceZ = (fleetDepth - 1) / 2;
        float Z = (float) ((-1) * halfZ + j) * saucerDepth + ((-1) * halfSpaceZ + j ) * depthSpacer;

        // horizontal coordinate X
        float halfX = fleetWidth / 2;
        float halfSpaceX = (fleetWidth - 1) / 2;
        float X = (float) ((-1) * halfX + k) * saucerWidth + ((-1) * halfSpaceX + k ) * horizontalSpacer;       
        
        return new Vector3(X,Y,Z);
    }

    // coroutine to build fleet
    System.Collections.IEnumerator BuildFleet()
    {
        // Build level by level
        int i = 0;
        foreach (int saucer in fleetHeight)
        {
            for (int k=0; k<fleetWidth; k++)
            {
                for (int j=0; j<fleetDepth; j++)
                {
                    // Calculate position of each new saucer
                    Vector3 position = CalculateRelativePosition(i, j, k);

                    // getSaucerType
                    GameObject prefab = null;
                    switch (saucer)
                    {
                        case SAUCER_SMALL: 
                            prefab = smallAlien; 
                            break;
                        case SAUCER_MEDIUM: 
                            prefab = mediumAlien;
                            break;
                        case SAUCER_BIG: 
                            prefab = bigAlien;
                            break;
                        case MOTHER_SHIP: 
                            prefab = motherShip;
                            break;
                    }

                    // Instantiate object
                    GameObject newSaucer = GameObject.Instantiate<GameObject>(prefab);

                    // Set it as a child of this object
                    newSaucer.transform.parent = this.transform;

                    // Set new position
                    newSaucer.transform.localPosition = position;

                    // Rotate to parent rotation
                    newSaucer.transform.rotation = Quaternion.LookRotation(transform.forward, transform.up);
                }
                
                // Calculate total number of saucers
                int numSaucers = fleetHeight.Length * fleetWidth; //* fleetDepth
                
                // wait between spawning saucers. Total wait no more than 2 seconds 
                yield return new WaitForSeconds(2.0f / numSaucers);
            }

            i++;
        }
        ready = true;

        yield return new WaitForSeconds(5);

        mainCamera.GetComponent<CameraBind>().MoveToTower();

    }

    // invert movement using random delay becasue multiple blocks can signal that wall collision detected
    public void RandomCollisionNotification()
    {
        float delay = Random.Range(0.001f, 0.005f);
        Invoke("CollisionNotification",delay);
    }

    public void CollisionNotification()
    {
        // if already notified - do nothing
        if (collisionNotification)
            return;

        collisionNotification = true;
        // collision happened when moved by X coordinate
        xDirection *= -1;

        // move one size down each saucer
        foreach (Transform child in transform)
        {
            child.gameObject.GetComponent<SaucerMove>().MoveDown(saucerHeight, transform.position[1]);
        }
    }

    // coroutine to move fleet
    System.Collections.IEnumerator MoveFleet()
    {
        // if fleet is not ready - do nothing
        while (!ready)
            yield return null;
        
        // if player is not yet in the tower - do nothing
        while (!mainCamera.GetComponent<CameraBind>().isInTower)
            yield return null;

        while (!destroyed)
        {
            float delay = 0.2f;
            yield return new WaitForSeconds(delay);
            collisionNotification = false;
            transform.Translate(xDirection * moveSpeed, 0.0f, 0.0f);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        // before start alien fleet, choose rotation as random angle around Y axis
        transform.rotation = Quaternion.LookRotation(new Vector3(Random.Range(-100,101), 0, Random.Range(-100,101)), transform.up);

        // generate fleet using co-routine
        StartCoroutine(BuildFleet());

        // move fleet using co-routine
        StartCoroutine(MoveFleet());

    }

    // Update is called once per frame
    void Update()
    {
        // move fleet
        
    }
}

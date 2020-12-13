using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
    Build a player tower out of DefenderCubes
*/
public class PlayerBuilder : MonoBehaviour
{
    // define prefabricated object - DefenderCube
    public GameObject prefab;
    public float prefabsize = 1.0f;

    // define initial movement speed in bricks per second
    public float moveSpeed = 20;
    public float rotateSpeed = 50;

    // Blueprint of player's tower. 3-d model build of cubes, 6x7x11
    public int[,,] blueprint = new int[,,]
    {
        {
            {1,1,1,1,1,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,1,1,1,1},
            {0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0},
            {1,1,1,1,1,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,1,1,1,1}           
        },
        {
            {1,1,1,1,1,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,1,1,1,1},
            {0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0},
            {1,1,1,1,1,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,1,1,1,1}           
        },
        {
            {1,1,1,1,1,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,1,1,1,1}           
        },
        {
            {0,0,0,0,0,0,0,0,0,0,0},
            {0,1,1,1,1,1,1,1,1,1,0},
            {0,1,1,1,1,1,1,1,1,1,0},
            {0,1,1,1,1,1,1,1,1,1,0},
            {0,1,1,1,1,1,1,1,1,1,0},
            {0,1,1,1,1,1,1,1,1,1,0},
            {0,0,0,0,0,0,0,0,0,0,0}
        },
        {
            {0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,1,1,1,0,0,0,0},
            {0,0,0,0,1,1,1,0,0,1,0},
            {0,0,0,0,1,1,1,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0}
        },
        {
            {0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,1,1,1,0,0,0,0},
            {0,0,0,0,1,0,1,0,0,0,0},
            {0,0,0,0,1,1,1,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0}
        }
    };

    // define state flags
    public bool destroyed = false;
    public bool ready = false;

    // define camera object to drag around
    public GameObject mainCamera;

    private Vector3 CalculateRelativePosition(int i, int j, int k)
    {
        // vertical coordinate Y - One cube down + another cube for each additional i 
        float fullY = blueprint.GetLength(0);
        float Y = (float) (0.5 - fullY + i) * prefabsize ; 
        // forward coordinate Z
        float halfZ = blueprint.GetLength(2) / 2;
        float Z = (float) ((-1) * halfZ + k ) * prefabsize;
        // horizontal coordinate X
        float halfX = blueprint.GetLength(1) / 2;
        float X = (float) ((-1) * halfX + j ) * prefabsize;
        
        return new Vector3(X,Y,Z);
    }

    // Coroutine to build player's tower
    System.Collections.IEnumerator BuildTower()
    {
        // wait initially 2 seconds
        yield return new WaitForSeconds(2);
        
        // create player's tower based on blueprint
        // each cube is size one on all edges, and everything is built below point of script

        for (int i=0; i<blueprint.GetLength(0); i++)
        {
            for (int j=0; j<blueprint.GetLength(1); j++)
            {
                for (int k=0; k<blueprint.GetLength(2); k++)
                {
                    // Do it only if cube in this part of the model defined            
                    if (blueprint[i,j,k] == 1)
                    {
                        // Calculate coordinates
                        Vector3 position = CalculateRelativePosition(i,j,k);
                        // Instantiate object
                        GameObject newCube = GameObject.Instantiate<GameObject>(prefab);

                        // Set it as a child of this object
                        newCube.transform.parent = this.transform;

                        // Set new position
                        newCube.transform.localPosition = position;

                        // Rotate to parent rotation
                        newCube.transform.rotation = Quaternion.LookRotation(transform.forward, transform.up);
                    }
                }
            }
        }

        // wait for additional second after building tower
        yield return new WaitForSeconds(1);

        // Move camera to the top of the tank
        ready = true;
    }

    private Vector3 MovementWithinCageBoundary(Vector3 currentPosition, Vector3 moveVector)
    {
        Vector3 newPosition, finalMove;
        // check if it flew outside of the box
        GameObject cage = GameObject.Find("/Cage");
        // get half depth of the prefab
        float halfTowerBody = blueprint.GetLength(1) / 2 * prefabsize;

        Vector3 allowedMove = currentPosition;
        Vector3 xMovement = moveVector;
        xMovement.z = 0.0f;
        Vector3 zMovement = moveVector;
        zMovement.x = 0.0f;

        // initialize what is allowed
        finalMove = new Vector3(0.0f, 0.0f, 0.0f);

        // check if new position by X is still within boundaries
        newPosition = allowedMove + xMovement;
        if (newPosition.x - halfTowerBody >= -cage.transform.localScale.x / 2 && newPosition.x + halfTowerBody <= cage.transform.localScale.x / 2)
        {
            finalMove += xMovement;
        }
 
        // check if new position by Z is still within boundaries
        newPosition = allowedMove + zMovement;
        if (newPosition.z - halfTowerBody >= -cage.transform.localScale.z / 2 && newPosition.z + halfTowerBody <= cage.transform.localScale.z /2 )
        {    
            finalMove += zMovement;
        }

        allowedMove += finalMove;

        return allowedMove;
    }

    // move whole tower forward or backward
    private void Move(float units, int direction)
    {
        // no move - don't waste time
        if (units == 0.0f)
            return;
        
        Vector3 moveDirection;   
        // take tower's current position, and move left-right or forward-backward based on camera's current rotation
        if (direction  == 0)
            moveDirection  = mainCamera.transform.right;
        else if (direction == 2)
            moveDirection  = mainCamera.transform.forward;
        else
            // do notyhing, as wrong coordinate
            return;

        moveDirection.y = 0.0f;
        moveDirection.Normalize();
        // get allowed movement which is still within cage boundary
        Vector3 newPosition = MovementWithinCageBoundary(transform.position, moveDirection * units);
        
        // move to this new positions
        transform.position = newPosition;
        // drag camera to the same point
        mainCamera.transform.position = newPosition;
    }

    // rotate whole tower around vertical axis
    private void Rotate(float angle)
    {
        // create rotation quaternion around vertical axis
        Quaternion rotate = Quaternion.AngleAxis(angle, Vector3.up);
        transform.rotation = rotate * transform.rotation;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Use co-routine to build tower
        StartCoroutine(BuildTower());


    }

    // Update is called once per frame
    void Update()
    {
        // use controls to move tower around - only if playr is in tower
        if (mainCamera.GetComponent<CameraBind>().isInTower)
        {
            float move, strafe;
            move = Input.GetAxis("Vertical");
            strafe = Input.GetAxis("Horizontal");

            Move(move * moveSpeed * Time.deltaTime, 2);
            Move(strafe * moveSpeed * Time.deltaTime, 0);

            // slowly turn tower to match camera
            //Vector3 direction = mainCamera.transform.rotation - transform.rotation;
            //Quaternion rotateTo = Quaternion.LookRotation(direction);
            
            Quaternion targetOrientation = mainCamera.transform.rotation;
            targetOrientation.x = 0.0f;
            targetOrientation.z = 0.0f;
            
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetOrientation, rotateSpeed / 2 * Time.deltaTime);
        }
    }
}

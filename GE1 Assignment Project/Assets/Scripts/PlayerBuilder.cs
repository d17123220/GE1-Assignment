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

    // move whole tower forward or backward
    private void Move(float units)
    {
        // take tower's current position, and move forward or backward based on camera's current rotation
        Vector3 moveDirection  = mainCamera.transform.forward;
        moveDirection.y = 0.0f;
        moveDirection.Normalize();
        transform.position += moveDirection * units;
        // drag camera to the same point
        mainCamera.transform.position = transform.position;
    }

    private void Strafe(float units)
    {
        // take tower's current position, and move right or left based on camera's current rotation
        Vector3 moveDirection  = mainCamera.transform.right;
        moveDirection.y = 0.0f;
        moveDirection.Normalize();
        transform.position += moveDirection * units;
        // drag camera to the same point
        mainCamera.transform.position = transform.position;
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
            float move, rotate;
            move = Input.GetAxis("Vertical");
            rotate = Input.GetAxis("Horizontal");

            Move(move * moveSpeed * Time.deltaTime);
            //Rotate(rotate * rotateSpeed * Time.deltaTime);
            Strafe(rotate * moveSpeed * Time.deltaTime);

            // slowly turn tower to match camera
            //Vector3 direction = mainCamera.transform.rotation - transform.rotation;
            //Quaternion rotateTo = Quaternion.LookRotation(direction);
            
            Quaternion targetOrientation = mainCamera.transform.rotation;
            targetOrientation.x = 0.0f;
            targetOrientation.z = 0.0f;
            
            if (rotate == 0.0f)
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetOrientation, rotateSpeed / 2 * Time.deltaTime);
        }
    }
}

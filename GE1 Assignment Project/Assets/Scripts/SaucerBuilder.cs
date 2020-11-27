using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Build an alien saucer out of AlienCubes
*/
public class SaucerBuilder : MonoBehaviour
{
    // define prefabricated object - DefenderCube
    public GameObject prefab;
    public float prefabsize = 1.0f;

    // Blueprint of saucers. 3-d model build of cubes, 8x6x12
    public int[,,] blueprintBig = new int[,,]
    {
        {
            {0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0},
            {1,1,0,0,0,0,0,0,0,0,1,1},
            {1,1,0,0,0,0,0,0,0,0,1,1},
            {0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0}          
        },
        {
            {0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,1,1,0,1,1,0,1,1,0,0},
            {0,0,1,1,0,1,1,0,1,1,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0}
        },
        {
            {0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,1,1,0,0,1,1,0,0,0},
            {0,0,0,1,1,0,0,1,1,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0} 
        },
        {
            {0,0,1,1,1,1,1,1,1,1,0,0},
            {0,1,1,1,1,1,1,1,1,1,1,0},
            {1,1,1,1,1,1,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,1,1,1,1,1},
            {0,1,1,1,1,1,1,1,1,1,1,0},
            {0,0,1,1,1,1,1,1,1,1,0,0}
        },
        {
            {0,0,1,0,0,1,1,0,0,1,0,0},
            {0,1,1,0,0,1,1,0,0,1,1,0},
            {1,1,1,0,0,1,1,0,0,1,1,1},
            {1,1,1,0,0,1,1,0,0,1,1,1},
            {0,1,1,0,0,1,1,0,0,1,1,0},
            {0,0,1,0,0,1,1,0,0,1,0,0}
        },
        {
            {0,0,1,1,1,1,1,1,1,1,0,0},
            {0,1,1,1,1,1,1,1,1,1,1,0},
            {1,1,1,1,1,1,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,1,1,1,1,1},
            {0,1,1,1,1,1,1,1,1,1,1,0},
            {0,0,1,1,1,1,1,1,1,1,0,0}
        },
        {
            {0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,1,1,1,1,1,1,1,1,0,0},
            {0,1,1,1,1,1,1,1,1,1,1,0},
            {0,1,1,1,1,1,1,1,1,1,1,0},
            {0,0,1,1,1,1,1,1,1,1,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0}
        },
        {
            {0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,1,1,1,1,0,0,0,0},
            {0,0,0,0,1,1,1,1,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0}
        }
    };

    public int[,,] blueprintMedium = new int[,,]
    {
        {
            {0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0},
            {0,1,0,0,0,0,0,0,0,1,0},
            {0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0}          
        },
        {
            {0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0},
            {0,0,1,0,0,0,0,0,1,0,0},
            {0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0}  
        },
        {
            {0,0,0,0,0,0,0,0,0,0,0},
            {0,0,1,1,1,1,1,1,1,0,0},
            {0,1,1,1,1,1,1,1,1,1,0},
            {0,0,1,1,1,1,1,1,1,0,0},
            {0,0,0,0,0,0,0,0,0,0,0}
        },
        {
            {0,0,0,1,1,1,1,1,0,0,0},
            {0,1,1,1,1,1,1,1,1,1,0},
            {1,1,1,1,1,1,1,1,1,1,1},
            {0,1,1,1,1,1,1,1,1,1,0},
            {0,0,0,1,1,1,1,1,0,0,0}
        },
        {
            {0,0,0,0,1,1,1,0,0,0,0},
            {0,0,1,0,1,1,1,0,1,0,0},
            {1,1,1,0,1,1,1,0,1,1,1},
            {0,0,1,0,1,1,1,0,1,0,0},
            {0,0,0,0,1,1,1,0,0,0,0}
        },
        {
            {0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,1,1,1,1,1,0,0,0},
            {1,0,1,1,1,1,1,1,1,0,1},
            {0,0,0,1,1,1,1,1,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0}
        },
        {
            {0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0},
            {1,0,0,1,0,0,0,1,0,0,1},
            {0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0}
        },
        {
            {0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0},
            {0,0,1,0,0,0,0,0,1,0,0},
            {0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0}
        }
    };

    public int[,,] blueprintSmall = new int[,,]
    {
        {
            {0,0,0,0,0,0,0,0},
            {1,0,1,0,0,1,0,1},
            {1,0,1,0,0,1,0,1},
            {0,0,0,0,0,0,0,0}         
        },
        {
            {0,0,0,0,0,0,0,0},
            {0,1,0,1,1,0,1,0},
            {0,1,0,1,1,0,1,0},
            {0,0,0,0,0,0,0,0}  
        },
        {
            {0,0,0,0,0,0,0,0},
            {0,0,1,0,0,1,0,0},
            {0,0,1,0,0,1,0,0},
            {0,0,0,0,0,0,0,0}  
        },
        {
            {0,1,1,1,1,1,1,0},
            {1,1,1,1,1,1,1,1},
            {1,1,1,1,1,1,1,1},
            {0,1,1,1,1,1,1,0}
        },
        {
            {0,1,0,1,1,0,1,0},
            {1,1,0,1,1,0,1,1},
            {1,1,0,1,1,0,1,1},
            {0,1,0,1,1,0,1,0}
        },
        {
            {0,0,1,1,1,1,0,0},
            {0,1,1,1,1,1,1,0},
            {0,1,1,1,1,1,1,0},
            {0,0,1,1,1,1,0,0}
        },
        {
            {0,0,0,1,1,0,0,0},
            {0,0,1,1,1,1,0,0},
            {0,0,1,1,1,1,0,0},
            {0,0,0,1,1,0,0,0}
        },
        {
            {0,0,0,0,0,0,0,0},
            {0,0,0,1,1,0,0,0},
            {0,0,0,1,1,0,0,0},
            {0,0,0,0,0,0,0,0}
        }
    };




    // defin state flags
    public bool destroyed = false;
    public bool initialFigure = true;

    private Vector3 CalculateRelativePosition(int i, int j, int k, int[,,] blueprint)
    {
        // vertical coordinate Y - One cube down + another cube for each additional i 
        float fullY = blueprint.GetLength(0);
        float Y = (float) (-0.5 - fullY + i) * prefabsize ; 
        // forward coordinate Z
        float halfZ = blueprint.GetLength(2) / 2;
        float Z = (float) ((-1) * halfZ + j + 0.5) * prefabsize;
        // horizontal coordinate X
        float halfX = blueprint.GetLength(1) / 2;
        float X = (float) ((-1) * halfX + k + 0.5) * prefabsize;
        
        return new Vector3(X,Y,Z);
    }



    // Start is called before the first frame update
    void Start()
    {
        // define vertical coordinate to use in future
        float Y = 0.0f;
        // define maximum height of the fleet
        float maxY = transform.parent.gameObject.transform.position[1];
        float minY = 8.0f;
        Color cubeColor;

        int[,,] blueprint;
        
        // Select blueprint based on name of the object
        switch (gameObject.name)
        {
            case string a when a.Contains("BigShip"):
                blueprint = blueprintBig;
                break;
            case string a when a.Contains("MediumShip"):
                blueprint = blueprintMedium;
                break;
            case string a when a.Contains("SmallShip"):
                blueprint = blueprintSmall;
                break;
            default:
                blueprint = blueprintBig;
                break;
        }
        
        //var blueprint = blueprintSmall;


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
                        Vector3 position = CalculateRelativePosition(i,j,k, blueprint);
                        // Instantiate object
                        GameObject newCube = GameObject.Instantiate<GameObject>(prefab);

                        // Set it as a child of this object
                        newCube.transform.parent = this.transform;

                        // Set new position
                        newCube.transform.localPosition = position;

                        // set color for cube based on it's height
                        Y = newCube.transform.position[1];
                        cubeColor = Color.HSVToRGB(0.33f + (1.0f - (Y - minY)/(maxY - minY))*0.67f, 1.0f, 1.0f);
                        // Set it slightly transparent
                        cubeColor.a = 0.9f;

                        // Rotate to parent rotation
                        newCube.transform.rotation = Quaternion.LookRotation(transform.forward, transform.up);

                        // paint with hue depending on height
                        newCube.GetComponent<Renderer>().material.color = cubeColor;

                    }
                }
            }
        }



    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

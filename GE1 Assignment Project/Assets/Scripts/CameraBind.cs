using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBind : MonoBehaviour
{
    // Define state for player
    public bool isInCinematicMove = false;
    public bool isInExplosionMove = false;
    public bool isInFreeMove = true;
    public bool isInTower = false;
    public bool ready = false;

    // define points to look at
    public GameObject playerTower;
    public GameObject alienFleet;
    public Vector3 explosionScenery;

    // define speed for movement
    public float lookSpeed = 150.0f;
    public float moveSpeed = 50.0f;

    // define crosshair texture
    public Texture2D crosshairImage;

    // Move camera to tower
    public void MoveToTower()
    {
        if (isInFreeMove)
        {
            isInFreeMove = false;
            isInCinematicMove = true;
        }
    }

    public void MoveFromExplosion(Vector3 destination)
    {
        isInExplosionMove = true;
        isInTower = false;
        ready = false;
        explosionScenery = destination;
        gameObject.GetComponent<PlayerShooting>().canShoot = false;
    }

    // move camera forward or backward (without rotation)
    void Walk(float units)
    {
        // take camera's current position, and move forward or backward based on camera's rotation
        transform.position += transform.forward * units;
    }

    // move camera side to side (without rotation)
    void Strafe(float units)
    {
        // take camera's current position, and move to the side based on camera's rotation
        transform.position += transform.right * units;
    }

    // move camera up or down (without rotation)
    void Climb(float units)
    {
        // take camera's current position, and move up/down based on camera's rotation
        transform.position += transform.up * units;
    }

    // turn camera left to right
    private void Yaw(float angle) 
    {
        // create rotation quaternion around vertical axis
        Quaternion rotate = Quaternion.AngleAxis(angle, Vector3.up);
        transform.rotation = rotate * transform.rotation;
    }

    // turn camera up and down
    private void Pitch(float angle, float limitDown = -85.0f, float limitUp = 85.0f)
    { 
        // get cos between vector.up and vertical angle of camera
        float verticalAngle = Vector3.Dot(transform.forward, Vector3.up);
        
        // limit rotation to limitDown and limitUp
        if ( (angle > 0 && verticalAngle <= Mathf.Cos((90-limitDown) * Mathf.Deg2Rad)) || (angle < 0 && verticalAngle >= Mathf.Cos((90-limitUp) * Mathf.Deg2Rad)) )
        {
            // do nothing
            return;
        }

        // create rotation quaternion around horizontal axis
        Quaternion rotate = Quaternion.AngleAxis(angle, transform.right);
        transform.rotation = rotate * transform.rotation;
    }

    // Start is called before the first frame update
    void Start()
    {
        // remove cursor
        Cursor.visible = false;
    }

    // Draw various things on camera's GUI
    void OnGUI()
    {
        float textureSize = Screen.height / 10;
        float xMin = (Screen.width / 2) - (textureSize / 2);
        float yMin = (Screen.height / 2) - (textureSize / 2);
        
        // if in tower - draw crosshair
        if (isInTower)
            GUI.DrawTexture(new Rect(xMin, yMin, textureSize, textureSize), crosshairImage);
    }

    // Update is called once per frame
    void Update()
    {
        // get mouse movement
        float mouseX, mouseY, walk, strafe;
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
          walk = Input.GetAxis("Vertical");
        strafe = Input.GetAxis("Horizontal");

        // quit by pressing Esc
        if (Input.GetKey(KeyCode.Escape))
        {
            Cursor.visible = true;
            Application.Quit();
        }

        // If moving to explosion place
        if (isInExplosionMove)
        {
            // calculate if looking straight at target
            Vector3 direction = playerTower.transform.position - transform.position;
            
            if (! (direction.x == 0.0f && direction.y == 0.0f && direction.z == 0.0f) )  
            {
                Quaternion rotateTo = Quaternion.LookRotation(direction);
                // slowly look at target
                transform.rotation = Quaternion.Lerp(transform.rotation, rotateTo, lookSpeed / 60 * Time.deltaTime);
            }

            // move to explosionScenery
            transform.position = Vector3.Lerp(transform.position, explosionScenery, moveSpeed / 30 * Time.deltaTime);
            var distance = Vector3.Distance(transform.position,explosionScenery);

            if (distance < 0.08f)
            {
                // finally explode player's tower 
                playerTower.gameObject.GetComponent<PlayerExplode>().readyForExplosion = true;
                isInExplosionMove = false;
            }
        }

        // if moving to player's tower
        if (isInCinematicMove)
        {
            Transform moveTo = playerTower.transform;
            Vector3 moveToVector = moveTo.position + new Vector3(0.0f, 1.5f, 0.0f);
            Transform lookAt;

            // check where to look
            var distance = Vector3.Distance(transform.position,moveToVector);
            if (distance > 0.8)
            {
                lookAt = moveTo;
            }
            else
            {
                lookAt = alienFleet.transform;
            }

            // calculate if looking straight at target
            Vector3 direction = lookAt.position - transform.position;
            Quaternion rotateTo = Quaternion.LookRotation(direction);
            
            // if distance below and looking at a fleet
            if (distance < 0.05 && rotateTo[1] < 0.05 && rotateTo[2] < 0.05)
            {
                isInCinematicMove = false;
                ready = true;
                // if alien fleet and player's tower are ready - set camera into tower mode 
                if (alienFleet.GetComponent<FleetBuilder>().ready && playerTower.GetComponent<PlayerBuilder>().ready)
                {
                    // enable shooting
                    gameObject.GetComponent<PlayerShooting>().canShoot = true;
                    isInTower = true;
                }
            }
            // otherwise move and look
            else
            {
                // slowly look at target
                transform.rotation = Quaternion.Lerp(transform.rotation, rotateTo, lookSpeed / 30 * Time.deltaTime);
                // show cinematic LEARP from one point to another
                transform.position = Vector3.Lerp(transform.position, moveToVector, moveSpeed / 30 * Time.deltaTime);
            }
        }
        // otherwise either free move or tower management
        else if (isInFreeMove)
        {
            // look around using mouse
            Yaw(mouseX * lookSpeed * Time.deltaTime);
            Pitch(-mouseY * lookSpeed * Time.deltaTime);

            // move around using keyboard
            Walk(walk * moveSpeed * Time.deltaTime);
            Strafe(strafe * moveSpeed * Time.deltaTime);

            // pull up or down
            int flyDirection = 0;
            if (Input.GetKey(KeyCode.R))
                flyDirection = 1;
            else if (Input.GetKey(KeyCode.F))
                flyDirection = -1;
            Climb(flyDirection * moveSpeed * Time.deltaTime);

            if (Input.GetKey(KeyCode.Space) && !playerTower.gameObject.GetComponent<PlayerBuilder>().destroyed)
                MoveToTower();

        }
        // otherwise if in tower
        else if (isInTower)
        {
            // look around using mouse
            Yaw(mouseX * lookSpeed * Time.deltaTime);
            Pitch(-mouseY * lookSpeed * Time.deltaTime, 0.0f);

        }
    }
}

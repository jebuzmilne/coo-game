using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controls : MonoBehaviour
{
    public float horizontalInput;
    public float verticalInput;
    public float speed = 50.0f;
    public float xRange = 200.0f;
    public float zRange = 200.0f;
    public Quaternion Face;
    public float turnSpeed = 5.0f;
    public Vector3 inputVector;
    public Vector3 currentPosition;
    public GameObject grassPrefab;
    public GameObject PooPrefab;
    public float GridSpaceSize = 40f;
    public int height = 5;
    public int width = 5;
    public Vector3Int ClosestGridnum;
    public Vector3 ClosestGridposition;
    public Vector3 Newgrassloc;
    public GameObject poop;
    public GameObject neargrid;
    public GameObject neargrass;



    public class GameManager : MonoBehaviour
    {
        public static bool IsInputEnabled = true;
    }


    void Start()
    {
    }

 
    void Update()
    {
      
        //call get grid from world functions to get grid closest grid number and position
        ClosestGridnum = GetGridPosFromWorld();
        ClosestGridposition = GetWorldPosFromGridPos();
        //get horizontal input
        horizontalInput = Input.GetAxis("Horizontal");
        //translate coo left or right in global space (not local) as coo will be rotating
        transform.Translate(Vector3.right * horizontalInput * Time.deltaTime * speed, Space.World);
        //get vertical input from axis
        verticalInput = Input.GetAxis("Vertical");
        //translate coo up or down in global space (not local) as coo will be rotating
        transform.Translate(Vector3.forward * verticalInput * Time.deltaTime * speed, Space.World);
        
        //get vector of input (X,0,Z) based on up down left right input
        inputVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        //update current position
        currentPosition = transform.position;
        //add current position to input vector to make input vector position (x,0,z distance from coo)
        inputVector += currentPosition;
        //make Quaternion variable = to the input vector - currentposition) i.e rotational difference between where it is currently looking and where the input vector is pointing
        Face = Quaternion.LookRotation(inputVector - currentPosition);
        //turn coo using quaternion.slerp from current rotation to new face (x,0,Z) at turnspeed currently = 5
        transform.rotation = Quaternion.Slerp(transform.rotation, Face, turnSpeed);
       
        //update nearest grid and nearest grass objects
        neargrid= GameObject.Find("Grid Space (X: " + ClosestGridnum.x.ToString() + ", Z: " + ClosestGridnum.z.ToString() + ")");
        neargrass = GameObject.Find("Grass (X: " + ClosestGridnum.x.ToString() + ", Z: " + ClosestGridnum.z.ToString() + ")");


        //defining boundaries for coo movement will delete in later levels once I've set up boundaries with rigid body
        if (transform.position.x < -xRange)
        {
            transform.position = new Vector3(-xRange, transform.position.y, transform.position.z);
        }
        if (transform.position.x > xRange)
        {
            transform.position = new Vector3(xRange, transform.position.y, transform.position.z);
        }

        if (transform.position.z < -zRange)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -zRange);
        }
        if (transform.position.z > zRange)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, zRange);
        }

        //call poop code on space
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Poop());

        }

        //IEnumerator function to allow use of waitforseconds
        IEnumerator Poop()
        {
            //gameobject new grass to be updated after grass is made see below
            GameObject newGrass;
            //set speed to zero so coo can't move any further away from grid
            speed = 0f;
            //move coo to closest grid (Note: currently set to instant lock to grid appears to work because of animation but might change to while loop and slow increments)
            transform.position = Vector3.MoveTowards(currentPosition, ClosestGridposition, 40f);
            //check if nearest grid object is occupied 
            if (neargrid.GetComponent<Gridscript>().isOccupied == false)
            {
                //wait one second then check if poop has not already been laid down
                yield return new WaitForSeconds(1);
                if (!poop)
                {
                    //set newgrass spawn location to current closest grid, set y to -1 so grass bottom is below grid
                    Newgrassloc = ClosestGridposition;
                    Newgrassloc.y = -1;
                   
                    // create new poop (quaternion could just be set to identity but I've messed up the axis with blender models so have just worked around by setting correct rotation relative to gameworld coordinates)
                        poop = Instantiate(PooPrefab, transform.position, Quaternion.AngleAxis(-10f, Vector3.forward));
                    //coo can now move again
                    speed = 50f;
                }
                //coo can now move again 
                speed = 50f;
                //wait 5 seconds (will probably change this wait once I've seen how game runs)
                yield return new WaitForSeconds(5);
                //destroy the poop
                Destroy(poop);
                //create new grass prefab (to be updated to use grass prefab array)
               newGrass = Instantiate(grassPrefab, Newgrassloc, Quaternion.AngleAxis(-90f, Vector3.right));
                //add new grass to grass spawner as child
                newGrass.transform.parent = GameObject.Find("GrassSpawner").transform;
                //rename to Grass (X,Z) so eat function can find again
                newGrass.name = "Grass (X: " + ClosestGridnum.x.ToString() + ", Z: " + ClosestGridnum.z.ToString() + ")";
               
            }
            //coo can run again
            speed = 50f;
        }
        //end poop code


        //eat code
        if (Input.GetKeyDown(KeyCode.Return))
        {
            //on return hit start eat function
            StartCoroutine(Eat());
        }
            //IEnumerator to allow while loop for movement
            IEnumerator Eat() {
            //check nearest grid is occupied 
            if (neargrid.GetComponent<Gridscript>().isOccupied == true)
            {
                //coo can't move
                speed = 0f;
                //while coo is not at grid position move incrementally to grid
                while (currentPosition.x != ClosestGridposition.x)
                {
                    transform.LookAt(ClosestGridposition);
                    transform.position = Vector3.MoveTowards(currentPosition, ClosestGridposition, 0.5f);
                  //return null to end while loop  
                yield return null;
                }
               //delete eaten grass   
                    Destroy(neargrass);
                //run grid grasseaten function to change is occupied to false
                neargrid.GetComponent<Gridscript>().grassEaten();
                //coo can walk again
               speed = 50f;
                }
           //coo can walk again
                speed = 50f;
            //return null to end function
            yield return null;
            }
       
    }
    //note I have repeated function in gridspawner script but can't work out way to just run that function for coo
        Vector3Int GetGridPosFromWorld()
        {
            //variables x & z = floor of world position / gridspace size (i.e. grid number in x & z axis)
            int x = Mathf.FloorToInt((currentPosition.x + 20) / GridSpaceSize);

            int z = Mathf.FloorToInt((currentPosition.z + 20) / GridSpaceSize);

        //change float to int 
            x = Mathf.RoundToInt((x) * 10 / 10);
            z = Mathf.RoundToInt((z) * 10 / 10);
        //return grid position as vector (+2 is to account for coo start position)
        return new Vector3Int(x + 2, 0, z + 2);
        }


        Vector3 GetWorldPosFromGridPos()
        {
            float x = (ClosestGridnum.x * GridSpaceSize) - 80;
            float z = (ClosestGridnum.z * GridSpaceSize) - 80;

            return new Vector3(x, 0, z);
        }
    }


    





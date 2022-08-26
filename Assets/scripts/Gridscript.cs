using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gridscript : MonoBehaviour
{
    //colliders to be called from collider script
    private BoxCollider collider1;
    private BoxCollider collider2;
    //add coo to check grid/coo relations
    private GameObject coo;
    //variable to track grid position
    public int posX;
    public int posZ;
    //reference to a grass that gets placed in this cell
    public GameObject inThisGridSpace = null;

        //saves if grid space is occupied or not
    public bool isOccupied = false;
    

    private void Update()
    {
       

    }
    //set position of this grid cell on the grid when created
    public void SetPosition(int x, int y, int z)
{
        posX = x;
        posZ = z;
    }

    //Get position of this gridspace on Grid
    public Vector3Int GetPosition()
    {
        return new Vector3Int(posX, 0, posZ);
    }

    //if object enters collider
    public void OnTriggerEnter(Collider other)
    {
        //collider 2 = this grid box collider
     collider2 = gameObject.GetComponent(typeof(BoxCollider)) as BoxCollider;
        //find coo and update sctipt object
        coo = GameObject.Find("coo");
        //collider 1 = coos box collider (note change names as more complexity is added)
        collider1 = coo.GetComponent(typeof(BoxCollider)) as BoxCollider;

        //ignore any collistion between coo and grid
    Physics.IgnoreCollision(collider1, collider2, true);
        //id anything other than coo collides with object
        if (other.attachedRigidbody)
            //object is now occupied
        isOccupied = true;
        //update inthisgridspace object
        inThisGridSpace = other.attachedRigidbody.gameObject;
    }
    //on object exit remove in this grid space object and make isoccupied false
    public void OnTriggerExit(Collider other)
    {

        if (other.attachedRigidbody)
            isOccupied = false;
        inThisGridSpace = null;
    }

    //when coo eats grass change inthisgrid space object and make isoccupied false
    public void grassEaten()
    {
        isOccupied = false;
        inThisGridSpace = null;
    }

}

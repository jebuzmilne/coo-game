using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gridCell : MonoBehaviour
{

    private int posX;
    private int posZ;

    public GameObject objectToThisGridSpace = null;
    public bool isOccupied = false;

   public void SetPosition(int x, int z)
    {
        posX = x;
        posZ = z;

    }    

    public Vector3Int GetPosition()
    {
        return new Vector3Int(posX, 0, posZ);
    }    
}

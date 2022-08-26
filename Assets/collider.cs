using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collider : MonoBehaviour
{
    public BoxCollider collider1;
        

   
    void Start()
    {
//declare all colliders as variables for ignoring certain physics
        collider1 = gameObject.GetComponent(typeof(BoxCollider)) as BoxCollider;
    }

   
}

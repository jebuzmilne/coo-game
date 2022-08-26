using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animatejump : MonoBehaviour
{
   
  
    void Start()
    {
        
    }

 
    void Update()
    {
        //play coo flip animation on space regardless of any other activity
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Animator>().Play("New Animation");
        }
    }
}

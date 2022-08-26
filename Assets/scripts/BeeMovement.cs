using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeMovement : MonoBehaviour
{

    public GameObject coo;
   public Vector3 DistanceToCoo;
    public Vector3 AbsDistancetoCoo;

    // Start is called before the first frame update
    void Start()
    {
        coo = GameObject.Find("coo");
    }

    // Update is called once per frame
    void Update()
    {
      DistanceToCoo = transform.position - coo.transform.position;
        AbsDistancetoCoo = new Vector3(Mathf.Sqrt(DistanceToCoo.x * DistanceToCoo.x), Mathf.Sqrt(DistanceToCoo.y * DistanceToCoo.y), Mathf.Sqrt(DistanceToCoo.z * DistanceToCoo.z));

        if (AbsDistancetoCoo.x <= 40)
        {
            GetComponent<Animator>().Play("Armature|ArmatureAction_001");
            
        }
    }
 

}

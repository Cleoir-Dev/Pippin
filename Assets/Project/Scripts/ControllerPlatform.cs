using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerPlatform : MonoBehaviour
{
    [Header("Config Platform")]
    public Transform platform, pointA, pointB;
    public float speedPlatform;
    public Vector3 pointDestine;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        platform.position = pointA.position;
        pointDestine = pointB.position;    
    }

    // Update is called once per frame
    void Update()
    {
        
        if(platform.position == pointA.position)
        {
            pointDestine = pointB.position;
        }

        if(platform.position == pointB.position)
        {
            pointDestine = pointA.position;
        }

        platform.position = Vector3.MoveTowards(platform.position, pointDestine, speedPlatform);
    }
}

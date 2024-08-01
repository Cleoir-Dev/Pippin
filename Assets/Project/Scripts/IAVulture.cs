using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAVulture : MonoBehaviour
{
    public float speed = 2;
    public float distanceAttack = 6.0f;
    public bool chase = false;
    public Transform startingPoint;
    private float oldSpeed;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        oldSpeed = speed;
        player = GameObject.FindGameObjectWithTag("Player");
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player == null)
        {
            return;
        }

        if(chase == true)
        {
          Chase();
        } else
        {
            ReturnStartPoint();
        }
        
        Flip();
    }

    private void Chase()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        if(oldSpeed == speed && Vector2.Distance(transform.position, player.transform.position) <= distanceAttack) 
        {
            // TODO: change shotting, animation e etc.
            //plus speed;
            speed = speed * 2;
        } else
        {
            //reset variables
            speed = oldSpeed;
        }
    }

    private void ReturnStartPoint()
    {
        transform.position = Vector2.MoveTowards(transform.position, startingPoint.position, speed * Time.deltaTime);
    }

    private void Flip()
    {
        if(transform.position.x < player.transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        } else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
}


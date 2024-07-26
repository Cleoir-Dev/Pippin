using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Transform firePosition;
    public GameObject projectile;

    private ControllerGame controlGame;

    private void Start()
    {
        controlGame = FindObjectOfType(typeof(ControllerGame)) as ControllerGame;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1") && controlGame.GetScore() > 0)
        {
            controlGame.Points(-1);
            Instantiate(projectile, firePosition.position, firePosition.rotation);
        }       
    }
}

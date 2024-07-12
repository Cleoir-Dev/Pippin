using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    private GameObject currentTeleporter;
    private CameraControl cameraControl;

    void Start()
    {
        cameraControl = Camera.main.GetComponent<CameraControl>();
    }

    void FixedUpdate()
    {
        if(currentTeleporter != null)
        {
            transform.position = currentTeleporter.GetComponent<Teleport>().GetDestination().position;

            if (cameraControl != null)
            {
                cameraControl.SmoothMoveTo();
            }
        } 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Teleporter"))
        {
            currentTeleporter = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Teleporter"))
        {
            if(collision.gameObject == currentTeleporter)
            {
                currentTeleporter = null;
            }
        }
    }
}

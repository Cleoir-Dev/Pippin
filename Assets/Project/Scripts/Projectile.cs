using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Projectile : MonoBehaviour
{
    public float projectileSpeed;
    public GameObject impactEffect;

    private PlayerControl player;

    private Rigidbody2D rg;

    // Start is called before the first frame update
    void Start()
    {
        rg = GetComponent<Rigidbody2D>();
        player = FindObjectOfType(typeof(PlayerControl)) as PlayerControl;
        var flip = player.transform.localScale.x;
        rg.velocity = flip < 0 ? -transform.right * projectileSpeed : transform.right * projectileSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag != "Player")
        {
            Destroy(gameObject);
            GameObject tempExplosion = Instantiate(impactEffect, transform.position, Quaternion.identity);
            Destroy(tempExplosion, 0.5f);

            if(collision.tag == "Enemy")
            {
                player.DeathEnemy(collision);
            }
        }
        
    }
}

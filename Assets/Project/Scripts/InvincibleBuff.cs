using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/InvincibleBuff")]
public class InvincibleBuff : PowerUpEffect
{
    public float timeInvincible;
    public override void Apply(GameObject target)
    {
        var player = target.GetComponent<PlayerControl>();
        player.invincible = true;
        player.SetInvincible(timeInvincible , Color.yellow);
        
    } 
}

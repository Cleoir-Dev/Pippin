using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/HealthBuff")]
public class HealthBuff : PowerUpEffect
{
    [Range(1, 3)]
    public int amount;

    private int valueDefault = 1;

    public override void Apply(GameObject target)
    {
        var player = target.GetComponent<PlayerControl>();
        int life = player.life;

        if (life == player.maxLife && amount == player.maxLife)
        {
            target.GetComponent<PlayerControl>().SetBarStars(valueDefault);
        }
        else if (life == player.maxLife && amount <= player.maxLife)
        {
            target.GetComponent<PlayerControl>().SetPoints(amount);
        }
        else if (life < amount)
        {
            target.GetComponent<PlayerControl>().SetBarLife(amount);
        }
        else
        {
            life++;
            target.GetComponent<PlayerControl>().SetBarLife(life);
        }
    }
}

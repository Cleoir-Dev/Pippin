using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Powerups/HealthBuff")]
public class HealthBuff : PowerUpEffect
{
    [Range(2, 3)]
    public int amount;

    public override void Apply(GameObject target)
    {
        int life = target.GetComponent<PlayerControl>().life;
        if (life < amount)
        {
            target.GetComponent<PlayerControl>().SetBarLife(amount);
        }
    }
}

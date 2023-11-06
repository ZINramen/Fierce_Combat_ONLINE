using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitColider : MonoBehaviour
{
    public float flyingAttackForce = 0;
    public float attackForce = 10;
    public float thrustValue = 0.5f;
    public Entity owner;
    private void OnTriggerEnter2D(Collider2D other)
    {
        Entity entity = other.GetComponent<Entity>();
        if(entity)
        if (entity != owner)
        {
            entity.flyingDamagedPower = flyingAttackForce;
       
            if (owner.transform.localEulerAngles.y == 180)
                entity.Dameged(attackForce, (-attackForce) * thrustValue);
            else
                entity.Dameged(attackForce, attackForce * thrustValue);
        }
    }
}

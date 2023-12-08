using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitColider : MonoBehaviour
{
    public bool stunTarget = false;
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
            entity.stun = stunTarget;
            entity.flyingDamagedPower = flyingAttackForce;
            if (owner.transform.localEulerAngles.y == 180)
            {
                entity.Damaged(attackForce, (-attackForce) * thrustValue);
            }
            else
            {
                entity.Damaged(attackForce, attackForce * thrustValue);
            }
        }
    }
}

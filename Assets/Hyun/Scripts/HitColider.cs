using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitColider : MonoBehaviour
{
    public bool telp = false;
    public bool stunTarget = false;
    public float flyingAttackForce = 0;
    public float attackForce = 10;
    public float thrustValue = 0.5f;
    public Entity owner;
    private void OnTriggerEnter2D(Collider2D other)
    {
        Entity entity = other.GetComponent<Entity>();
        if(entity)
        if(entity != owner)
        {
            if (telp) 
            {
                owner.transform.position = entity.transform.position + owner.transform.right;
                if(owner.transform.localEulerAngles.y != 0) 
                    owner.transform.localEulerAngles = new Vector3(0,0,0);
                else
                    owner.transform.localEulerAngles = new Vector3(0,-180,0);
                    Destroy(gameObject,0.2f);
            }
            else
            {
                entity.stun = stunTarget;
                entity.flyingDamagedPower = flyingAttackForce;
                if (owner && owner.transform.localEulerAngles.y == 180)
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
}

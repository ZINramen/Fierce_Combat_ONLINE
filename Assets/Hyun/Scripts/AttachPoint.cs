using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AttachPoint : MonoBehaviour
{
    PhotonPlayer network;
    public bool noCatch = false;
    public Entity owner;
    public Entity target;

    private void Start()
    {
        network = GetComponent<PhotonPlayer>();
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        Entity entity = other.GetComponent<Entity>();
        if(!noCatch)
        if (entity && target == null)
            if (entity != owner && entity.DamageBlock != Entity.DefenseStatus.invincible)
            {
                target = entity;
                entity.Network_Catch = true;
            }
    }
    private void Update()
    {
        if (target)
        {
            target.Damaged(0, 0);
            target.transform.parent = transform;
            target.movement.Freeze();
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        Entity entity = other.GetComponent<Entity>();
        if (entity)
            if (entity == target)
            {
                entity.SetHp(entity.GetHp()-5.0f);
                target = null;
                entity.movement.UnFreeze();
                entity.transform.parent = null; 
                entity.Network_Catch = false;
            } 
    }
}

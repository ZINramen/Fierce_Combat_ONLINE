using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachPoint : MonoBehaviour
{
    public bool noCatch = false;
    public Entity owner;
    public Entity target;
    private void OnTriggerStay2D(Collider2D other)
    {
        Entity entity = other.GetComponent<Entity>();
        if(!noCatch)
        if (entity && target == null)
            if (entity != owner)
            {
                target = entity;
            }
    }
    private void Update()
    {
        target.Dameged(0, 0);
        target.transform.parent = transform;
        target.movement.Freeze();
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        Entity entity = other.GetComponent<Entity>();
        if (entity)
            if (entity == target)
            {
                target = null;
                entity.movement.UnFreeze();
                entity.transform.parent = null;
                entity.Dameged(5, -10);
            } 
    }
}

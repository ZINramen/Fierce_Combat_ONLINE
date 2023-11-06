using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectCreator : MonoBehaviour
{
    public void PlayEffect(string effectName, RaycastHit2D hit)
    {
        GameObject temp = Resources.Load<GameObject>("Effects/" + effectName);
        temp = Instantiate(temp);
        temp.transform.position = hit.point;

        HitColider hitAction = temp.GetComponent<HitColider>();
        if (hitAction)
            temp.GetComponent<HitColider>().owner = gameObject.GetComponent<Entity>();
    }
}

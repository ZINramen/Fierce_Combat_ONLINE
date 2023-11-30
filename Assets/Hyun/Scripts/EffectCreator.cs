using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EffectCreator : MonoBehaviour
{
    PhotonPlayer network;
    private void Start()
    {
        network = GetComponent<PhotonPlayer>();
    }
    public void PlayEffect(string effectName, RaycastHit2D hit)
    {
        GameObject temp = Resources.Load<GameObject>("Effects/" + effectName);
        temp = Instantiate(temp);
        temp.transform.position = hit.point;
        HitColider hitAction = temp.GetComponent<HitColider>();
        if (hitAction)
        {
            temp.GetComponent<HitColider>().owner = gameObject.GetComponent<Entity>();
        }
        if (network) 
        {
            network.NetworkSyncEffect(hit.point);
        }
    }
}

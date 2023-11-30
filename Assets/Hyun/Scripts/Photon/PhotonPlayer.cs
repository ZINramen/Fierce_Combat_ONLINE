using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonPlayer : MonoBehaviour, IPunObservable
{
    public GameObject effect;

    public PhotonView pv;
    Movement mv;
    Entity entity;

    public AnimationManager am;

    float posX;
    float posY;

    bool rot;

    public int combo;


    public void RunTriggerRpc(string name)
    {
        pv.RPC("Network_Trigger", RpcTarget.Others, name);
    }
    public void Thrust(float value)
    {
        pv.RPC("Network_Thrust", RpcTarget.Others, value);
    }
    public void ComboShow()
    {
        pv.RPC("ShowCombo", RpcTarget.Others);
    }
    public void HpChange()
    {
        pv.RPC("SetHpNetwork", RpcTarget.Others, entity.GetHp());
    }
    public void NetworkSyncEffect(Vector2 pos) 
    {
        pv.RPC("ShowEffect", RpcTarget.Others, pos);
    }

    [PunRPC]
    public void ShowEmoticon(int value)
    {
        entity.emoticon.SetValue(value);
    }

    [PunRPC]
    public void ShowEffect(Vector2 pos)
    {
        GameObject temp = Instantiate(effect);
        temp.transform.position = pos;
    }

    [PunRPC]
    public void ShowCombo()
    {
        ComboView cv = Instantiate(entity.ComboUI);
        ComboView.owner = entity;
    }

    [PunRPC]
    public void Network_Thrust(float thrustValue)
    {
        if (thrustValue < 0)
            transform.localEulerAngles = new Vector3(0, 0, 0);
        else
            transform.localEulerAngles = new Vector3(0, 180, 0);
        mv.SetThrustForceX(thrustValue);
    }



    [PunRPC]
    public void Network_Trigger(string name)
    {
        am.Network_SetTrigger(name);
    }

    [PunRPC]
    public void SetHp(float value)
    {
        entity.SetHp(value);
    }

    // Start is called before the first frame update
    void Start()
    {
        entity = GetComponent<Entity>();
        pv = GetComponent<PhotonView>();
        mv = GetComponent<Movement>();
        am = GetComponent<AnimationManager>();

        if (pv.IsMine)
        {
            mv.PlayerType = true;
            am.isPlayer = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pv.IsMine)
        {
            posX = transform.position.x;
            posY = transform.position.y;
            combo = ComboView.currValue;

            if (transform.localEulerAngles.y == 0)
                rot = true;
            else
                rot = false;
        }
        else
        {
            if (!entity.Network_Catch)
                transform.position = Vector3.Lerp(transform.position, new Vector3(posX, posY, transform.position.z), 0.3f);

            if (rot)
                transform.localEulerAngles = new Vector3(0, 0, 0);
            else
                transform.localEulerAngles = new Vector3(0, 180, 0);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(posX);
            stream.SendNext(posY);
            stream.SendNext(rot);
            stream.SendNext(combo);
        }
        else
        {
            posX = (float)stream.ReceiveNext();
            posY = (float)stream.ReceiveNext();
            rot = (bool)stream.ReceiveNext();
            combo = (int)stream.ReceiveNext();
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonPlayer : MonoBehaviour, IPunObservable
{
    PhotonView pv;
    Movement mv;
    AnimationManager am;

    float posX;
    float posY;

    bool rot;

    // Start is called before the first frame update
    void Start()
    {
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

            if (transform.localEulerAngles.y == 0)
                rot = true;
            else
                rot = false;
        }
        else 
        {
            transform.position = Vector3.Lerp(transform.position ,new Vector3(posX, posY, transform.position.z), 0.2f);
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
        }
        else
        {
            posX = (float)stream.ReceiveNext();
            posY = (float)stream.ReceiveNext();
            rot = (bool)stream.ReceiveNext();
        }
    }

}

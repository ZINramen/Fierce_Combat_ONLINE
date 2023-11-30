using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WaittingRoomCtrl : MonoBehaviourPun
{
    public GameObject L_Button;
    public GameObject R_Button;

    private PhotonView pv;

    public string stageName = "";

    public bool isColl_LButton = false;
    public bool isColl_RButton = false;
    public bool isGameStart = false;

    private void Start()
    {
        pv = this.GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        isColl_LButton = L_Button.GetComponent<ButtonCtrl>().Is_CollPlayer;
        isColl_RButton = R_Button.GetComponent<ButtonCtrl>().Is_CollPlayer;

        if (isGameStart == false && isColl_LButton && isColl_RButton)
        {
            isGameStart = true;
            pv.RPC("StartGame", RpcTarget.All);
        }
    }

    [PunRPC]
    private void StartGame()
    {
        StartCoroutine(StartGameCoroutine());
    }

    private IEnumerator StartGameCoroutine()
    {
        yield return new WaitForSeconds(1f);
        PhotonNetwork.LoadLevel("Test_Scene");
    }
}

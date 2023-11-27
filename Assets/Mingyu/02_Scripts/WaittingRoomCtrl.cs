using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WaittingRoomCtrl : MonoBehaviour
{
    public GameObject L_Button;
    public GameObject R_Button;

    public string stageName = "";

    public bool isColl_LButton = false;
    public bool isColl_RButton = false;

    private void Update()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings(); // PhotonNetwork 연결

            Debug.Log("끊김");
        }
        else
        {
            isColl_LButton = L_Button.GetComponent<ButtonCtrl>().Is_CollPlayer;
            isColl_RButton = R_Button.GetComponent<ButtonCtrl>().Is_CollPlayer;

            if (PhotonNetwork.IsMasterClient && isColl_LButton && isColl_RButton)
            {
                PhotonNetwork.LoadLevel("Test_Scene");
            }
        }
    }
}

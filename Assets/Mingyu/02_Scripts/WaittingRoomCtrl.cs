using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaittingRoomCtrl : MonoBehaviour
{
    public GameObject L_Tv;
    public GameObject L_Button;

    public GameObject R_Tv;
    public GameObject R_Button;

    public string stageName = "";

    private bool isColl_LTV = false;
    private bool isColl_LButton = false;

    private bool isColl_RTV = false;
    private bool isColl_RButton = false;

    private void Start()
    {
        isColl_LTV         = L_Tv.GetComponent<TV_ButtonCtrl>().Is_CollPlayer;
        isColl_LButton     = L_Button.GetComponent<TV_ButtonCtrl>().Is_CollPlayer;

        isColl_RTV         = R_Tv.GetComponent<TV_ButtonCtrl>().Is_CollPlayer;
        isColl_RButton     = R_Button.GetComponent<TV_ButtonCtrl>().Is_CollPlayer;
    }

    private void Update()
    {
        if (isColl_LTV && isColl_LButton)
            if(isColl_RTV && isColl_RButton)
                Mingyu_Photon_Lobby.Instance.GameStart(stageName);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaittingRoomCtrl : MonoBehaviour
{
    public GameObject L_Button;
    public GameObject R_Button;

    public string stageName = "";

    public bool isColl_LButton = false;
    public bool isColl_RButton = false;

    private void Update()
    {
        isColl_LButton = L_Button.GetComponent<ButtonCtrl>().Is_CollPlayer;
        isColl_RButton = R_Button.GetComponent<ButtonCtrl>().Is_CollPlayer;

        if (isColl_LButton && isColl_RButton)
        {
            Debug.Log("AA");
            // 게임 시작하는 부분
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Mingyu_RoomCtrl : MonoBehaviourPunCallbacks
{
    public Text roomNameT;
    public Text playerNumberT;
    public Text StageNameT;

    private int roomIndex;

    // 방 리스트를 업데이트 할때, 인덱스를 업데이트함
    public int Set_RoomIndex { set => roomIndex = value; }

    public void Btn_EnterButton()
    {
        Mingyu_Photon_Lobby.Instance.BtnEvent_JoinRoom(roomIndex);
    }

    public void Btn_EnterPWRoom()
    {
        Mingyu_Photon_Lobby.Instance.EnterRoomWithPW(roomIndex);
    }

    public void joinRoom()
    {
        PhotonNetwork.JoinRoom(roomNameT.text);
    }
}

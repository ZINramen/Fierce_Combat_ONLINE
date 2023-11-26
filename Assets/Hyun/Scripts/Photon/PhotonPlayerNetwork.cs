using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonPlayerNetwork : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject Player1;
    [SerializeField] GameObject Player2;

    PhotonView pv;

    private void Start()
    {
        /////////////////////네트워크////////////////////////
        PhotonNetwork.GameVersion = "FierceFight 1.0";
        PhotonNetwork.ConnectUsingSettings();
        ///////////////////////////////////////////////////// 
        DefaultPool pool = PhotonNetwork.PrefabPool as DefaultPool;
        pool.ResourceCache.Add(Player1.name, Player1);
        pool.ResourceCache.Add(Player2.name, Player2);
    }


    /////////////////////네트워크////////////////////////
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        GameObject currPlayer;
        if (PhotonNetwork.IsMasterClient)
        {
            currPlayer = PhotonNetwork.Instantiate(Player1.name, Vector3.zero, Quaternion.identity);
        }
        else
        {
            currPlayer = PhotonNetwork.Instantiate(Player2.name, Vector3.zero, Quaternion.identity);
        }
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);

        PhotonNetwork.CreateRoom("Fight", new RoomOptions { MaxPlayers = 2 });
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinRoom("Fight");
    }
    /////////////////////////////////////////////////////
}

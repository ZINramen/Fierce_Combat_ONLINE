using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonPlayerNetwork : MonoBehaviourPunCallbacks
{
    public DynamicCamera cam;
    public bool isLobby = false;

    [SerializeField] GameObject Player1;
    [SerializeField] GameObject Player2;
    [SerializeField] GameObject PlayerLobby;

    DefaultPool pool;

    public void StageLoad(string name)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(name);
        }
    }

    private void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            /////////////////////네트워크////////////////////////
            PhotonNetwork.GameVersion = "FierceFight 1.0";
            PhotonNetwork.ConnectUsingSettings();
        }

        if (!isLobby)
        {
            cam = Camera.main.GetComponent<DynamicCamera>();
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Instantiate(Player1.name, Vector3.zero, Quaternion.identity);
            }
            else
            {
                PhotonNetwork.Instantiate(Player2.name, Vector3.zero, Quaternion.identity);
            }
        }
        
            Debug.Log("실행");

        if(isLobby == true && PhotonNetwork.IsConnected)
        {

            //////////////////////////  /////////////////////////// 
            pool = PhotonNetwork.PrefabPool as DefaultPool;

            pool.ResourceCache.Add(Player1.name, Player1);
            pool.ResourceCache.Add(Player2.name, Player2);
            pool.ResourceCache.Add(PlayerLobby.name, PlayerLobby);

            StartCoroutine(WaitSpawn(pool, PlayerLobby.name));
        }
    }

    IEnumerator WaitSpawn(DefaultPool pool, string lobbyPlayerName)
    {
        yield return new WaitUntil(() => pool.ResourceCache.ContainsKey(lobbyPlayerName));
        PhotonNetwork.Instantiate(lobbyPlayerName, Vector3.zero, Quaternion.identity);
    }

    private void Update()
    {
        if(cam)
        if (!cam.enabled)
        {
            Entity[] entitys = FindObjectsByType<Entity>(FindObjectsSortMode.None);
            if (entitys.Length > 1)
            {
                cam.enabled = true;
            }
        }
    }


    /////////////////////네트워크////////////////////////
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
    }
    /////////////////////////////////////////////////////
}

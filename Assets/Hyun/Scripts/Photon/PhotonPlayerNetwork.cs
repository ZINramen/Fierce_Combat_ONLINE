using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class PhotonPlayerNetwork : MonoBehaviourPunCallbacks
{
    public bool isLobby = false;
    public StageCtrl stage;

    [SerializeField] GameObject PlayerLobby;
    [SerializeField] GameObject Player1Level;
    [SerializeField] GameObject Player2Level;

    DefaultPool pool;

    private void Awake()
    {
        if (!isLobby)
        {
            if(PhotonNetwork.IsMasterClient)
                PhotonNetwork.Instantiate(Player1Level.name, stage.playerFirstLocations[0].position, Quaternion.identity); 
            else
                PhotonNetwork.Instantiate(Player2Level.name, stage.playerFirstLocations[1].position, Quaternion.identity);
        }
    }

    private void Start()
    {
        pool = PhotonNetwork.PrefabPool as DefaultPool;
        Debug.Log("실행");

        if (pool != null && !pool.ResourceCache.ContainsKey(PlayerLobby.name))
        {
            pool.ResourceCache.Add(PlayerLobby.name, PlayerLobby);
            pool.ResourceCache.Add(Player1Level.name, Player1Level);
            pool.ResourceCache.Add(Player2Level.name, Player2Level);

            // PhotonNetwork.InRoom으로 방에 있을 때만 실행되도록 수정
            if (PhotonNetwork.InRoom && SceneManager.GetActiveScene().name == "WaitingRoom")
            {
                StartCoroutine(WaitSpawn(pool));
            }
            else
            {
                // 방에 들어갈 때까지 대기하는 코루틴을 실행
                StartCoroutine(WaitForJoinRoom(pool));
            }
        }
    }

    IEnumerator WaitForJoinRoom(DefaultPool pool)
    {
        yield return new WaitUntil(() => PhotonNetwork.InRoom && SceneManager.GetActiveScene().name == "WaitingRoom");

        // 방에 들어간 후에 WaitSpawn 코루틴 실행
        StartCoroutine(WaitSpawn(pool));
    }

    IEnumerator WaitSpawn(DefaultPool pool)
    {
        yield return new WaitForSeconds(0.5f); // 적절한 대기 시간
        PhotonNetwork.Instantiate(PlayerLobby.name, Vector3.zero, Quaternion.identity);

        Debug.Log("생성");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class PhotonPlayerNetwork : MonoBehaviourPunCallbacks
{
    public bool isLobby = false;

    [SerializeField] GameObject Player1;
    [SerializeField] GameObject Player2;
    [SerializeField] GameObject PlayerLobby;

    DefaultPool pool;

    private void Start()
    {
        pool = PhotonNetwork.PrefabPool as DefaultPool;
        Debug.Log("실행");

        if (!pool.ResourceCache.ContainsKey(Player1.name))
        {
            pool.ResourceCache.Add(Player1.name, Player1);
            pool.ResourceCache.Add(Player2.name, Player2);
            pool.ResourceCache.Add(PlayerLobby.name, PlayerLobby);

            // PhotonNetwork.InRoom으로 방에 있을 때만 실행되도록 수정
            if (PhotonNetwork.InRoom && SceneManager.GetActiveScene().name == "WaitingRoom")
            {
                StartCoroutine(WaitSpawn(pool, PlayerLobby.name));
            }
            else
            {
                // 방에 들어갈 때까지 대기하는 코루틴을 실행
                StartCoroutine(WaitForJoinRoom(pool, PlayerLobby.name));
            }
        }
    }

    IEnumerator WaitForJoinRoom(DefaultPool pool, string lobbyPlayerName)
    {
        yield return new WaitUntil(() => PhotonNetwork.InRoom && SceneManager.GetActiveScene().name == "WaitingRoom");

        // 방에 들어간 후에 WaitSpawn 코루틴 실행
        StartCoroutine(WaitSpawn(pool, lobbyPlayerName));
    }

    IEnumerator WaitSpawn(DefaultPool pool, string lobbyPlayerName)
    {
        yield return new WaitForSeconds(0.1f); // 적절한 대기 시간

        // 캐시에 있는 플레이어 생성
        PhotonNetwork.Instantiate(lobbyPlayerName, Vector3.zero, Quaternion.identity);
    }
}

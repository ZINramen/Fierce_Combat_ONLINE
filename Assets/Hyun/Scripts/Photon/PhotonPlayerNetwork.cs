using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class PhotonPlayerNetwork : MonoBehaviourPunCallbacks
{
    public bool isLobby = false;

    [SerializeField] GameObject PlayerLobby;

    DefaultPool pool;

    private void Start()
    {
        pool = PhotonNetwork.PrefabPool as DefaultPool;
        Debug.Log("실행");

        if (pool != null && !pool.ResourceCache.ContainsKey(PlayerLobby.name))
        {
            pool.ResourceCache.Add(PlayerLobby.name, PlayerLobby);

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

        else
            Debug.Log("저장x");

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

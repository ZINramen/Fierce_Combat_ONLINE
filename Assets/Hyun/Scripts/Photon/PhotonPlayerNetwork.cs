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

    // 생성된 p1, p2 - Mingyu
    private GameObject spawn_P1;
    private GameObject spawn_P2;

    [SerializeField] GameObject Ready_UI;
    [SerializeField] GameObject Go_UI;

    DefaultPool pool;

    private void Awake()
    {
        if (!isLobby)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                spawn_P1 = PhotonNetwork.Instantiate(Player1Level.name, stage.playerFirstLocations[0].position, Quaternion.identity);
                StartCoroutine(Set_ReadyOption(spawn_P1));
            }
            else 
            {
                spawn_P2 = PhotonNetwork.Instantiate(Player2Level.name, stage.playerFirstLocations[1].position, Quaternion.identity);
                StartCoroutine(Set_ReadyOption(spawn_P2));
            }
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

    IEnumerator Set_ReadyOption(GameObject spawnPlayer)
    {
        // 생성 후, 플레이어의 상태를 무적으로 만듬
        spawnPlayer.GetComponent<Entity>().DamageBlock = Entity.DefenseStatus.invincible;
        Ready_UI.SetActive(true);

        yield return new WaitForSecondsRealtime(1.5f);
        StartCoroutine(GameStart(spawnPlayer));
    }

    IEnumerator GameStart(GameObject spawnPlayer)
    {
        spawnPlayer.GetComponent<Entity>().DamageBlock = Entity.DefenseStatus.Nope;
        Ready_UI.SetActive(false);
        Go_UI.SetActive(true);

        yield return new WaitForSecondsRealtime(0.5f);
        Go_UI.SetActive(false);
    }
}

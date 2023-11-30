using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class StageCtrl : MonoBehaviour
{
    public Entity[] PlayerList //player 목록
    { get; set; }
    private int playerNumber; //살아 있는 player 수
    private Entity superPlayer; //가장 HP가 많은 player

    public Transform[] playerFirstLocations; //player 시작 위치

    public enum GameResult { Win, Draw, Lose };
    public static GameResult result = GameResult.Draw;

    //public void addPlayerList(Entity player)
    //{
    //    playerList[]
    //}

    void Awake()
    {
        PlayerList = FindObjectsOfType<Entity>(); //Entity를 가진 객체로 리스트를 만듬
        playerNumber = PlayerList.Length; //처음은 살아있으므로 리스트에 넣음
        superPlayer = PlayerList[0]; //임시로 superPlayer 설정
        //플레이어 위치 설정
        PlayerList[0].transform.position = playerFirstLocations[0].position;
        PlayerList[1].transform.position = playerFirstLocations[1].position;

        //Debug.Log("플레이어 수" + playerNumber);
        //Debug.Log("플레이어1" + PlayerList[0].name);
        //Debug.Log("플레이어2" + PlayerList[1].name);
        StartCoroutine(PlayerCheck());
    }

    //player 체력 체크 후 승패 체크
    IEnumerator PlayerCheck()
    {
        while (playerNumber > 0)
        {
            for (int i = 0; i < PlayerList.Length; i++)
            {
                if (PlayerList[i].GetHp() >= superPlayer.GetHp())
                    superPlayer = PlayerList[i];
            }

            //승리 조건
            if (superPlayer.isDie == true)
            {
                result = GameResult.Draw;
                Debug.Log("Draw");
            }
            else if (PlayerList[1].isDie == true)
            {
                result = GameResult.Win;
                Debug.Log(PlayerList[0].name + " Win");
            }
            else if (PlayerList[0].isDie == true)
            {
                result = GameResult.Lose;
                Debug.Log(PlayerList[1].name + " Win");
            }

            //Debug.Log("수뻐 플레이어 : " + superPlayer.name);
            //Debug.Log("Player1 HP :" + PlayerList[0].GetHp() + "Player2 HP :" + PlayerList[1].GetHp());
            yield return new WaitForSecondsRealtime(0.5f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}

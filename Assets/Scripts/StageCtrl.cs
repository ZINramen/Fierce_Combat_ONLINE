using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class StageCtrl : MonoBehaviour
{
    private Entity[] playerList; //player 목록
    private int playerNumber; //살아 있는 player 수
    private Vector3[] playerFirstLocations; //player 시작 위치
    private Entity superPlayer; //가장 HP가 많은 player

    public enum GameResult { Win, Draw, Lose };
    public static GameResult result = GameResult.Draw;

    //public void addPlayerList(Entity player)
    //{
    //    playerList[]
    //}

    void Awake()
    {
        playerList = FindObjectsOfType<Entity>(); //Entity를 가진 객체로 리스트를 만듬
        playerNumber = playerList.Length; //처음은 살아있으므로 리스트에 넣음
        superPlayer = playerList[0]; //임시로 superPlayer 설정

        Debug.Log("플레이어 수" + playerNumber);
        Debug.Log("플레이어1" + playerList[0].name);
        Debug.Log("플레이어2" + playerList[1].name);
        StartCoroutine(PlayerCheck());
    }

    //player 체력 체크 후 승패 체크
    IEnumerator PlayerCheck()
    {
        while (playerNumber > 0)
        {
            for (int i = 0; i < playerList.Length; i++)
            {
                if (playerList[i].GetHp() >= superPlayer.GetHp())
                    superPlayer = playerList[i];
            }

            if (superPlayer.isDie == true)
            {
                result = GameResult.Draw;
                Debug.Log("Draw");
            }
            else if (playerList[1].isDie == true)
            {
                result = GameResult.Win;
                Debug.Log(playerList[0].name + " Win");
            }
            else if (playerList[0].isDie == true)
            {
                result = GameResult.Lose;
                Debug.Log(playerList[1].name + " Win");
            }

            Debug.Log("수뻐 플레이어 : " + superPlayer.name);
            Debug.Log("Player1 HP :" + playerList[0].GetHp() + "Player2 HP :" + playerList[1].GetHp());
            yield return new WaitForSecondsRealtime(1.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}

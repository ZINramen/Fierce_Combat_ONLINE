using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class StageCtrl : MonoBehaviour
{
    private bool stageSettingEnd = false; // 스테이지 설정 완료 여부
    private int playerNumber; //살아 있는 player 수
    private Entity superPlayer; //가장 HP가 많은 player

    public Entity[] PlayerList { get; set; } //player 목록
    public Transform[] playerFirstLocations; //player 시작 위치

    [Space]
    public Image HP_Bar1P;
    public Image HP_Bar2P;

    [Space]
    public Image SpecialGauge_1P;
    public Image SpecialGauge_2P;

    [Space]
    public Image[] SkillIcons1P;
    public Image[] SkillIcons2P;

    public enum GameResult { Win, Draw, Lose, None};
    public static GameResult result = GameResult.None;

    public DynamicCamera dCam;

    public CastleCtrl castle;

    //public void addPlayerList(Entity player)
    //{
    //    playerList[]
    //}

    void Update()
    {
        if (PlayerList == null || PlayerList.Length < 2)
        {
            PlayerList = FindObjectsOfType<Entity>(); //Entity를 가진 객체로 리스트를 만듬
        }
        if(PlayerList != null && PlayerList.Length > 1 && !stageSettingEnd)
        {
            stageSettingEnd = true; // 다중 실행 방지
            if (castle) 
            {
                castle.enabled = true;
            }
            dCam.enabled = true;
            playerNumber = PlayerList.Length; //처음은 살아있으므로 리스트에 넣음
            superPlayer = PlayerList[0]; //임시로 superPlayer 설정

            CreateUIManager(); //UIManager 생성
                               //Debug.Log("플레이어 수" + playerNumber);
                               //Debug.Log("플레이어1" + PlayerList[0].name);
                               //Debug.Log("플레이어2" + PlayerList[1].name);

            StartCoroutine(PlayerCheck());
        }
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
            Debug.Log("Player1 HP :" + PlayerList[0].GetHp() + "Player2 HP :" + PlayerList[1].GetHp());
            yield return new WaitForSecondsRealtime(0.5f);
        }
    }

    void CreateUIManager()
    {
        GameObject UIObj = new GameObject("UIManager");
        UICtrl myUICtrl = UIObj.AddComponent<UICtrl>();

        myUICtrl.HP_Bar1P = HP_Bar1P;
        myUICtrl.HP_Bar2P = HP_Bar2P;
        myUICtrl.SpecialGauge_1P = SpecialGauge_1P;
        myUICtrl.SpecialGauge_2P = SpecialGauge_2P;
        myUICtrl.SkillIcons1P = SkillIcons1P;
        myUICtrl.SkillIcons2P = SkillIcons2P;

        foreach (Entity player in PlayerList)
        {
            if (player.network.pv.IsMine)
            {
                if (PhotonNetwork.IsMasterClient)
                    myUICtrl.Player1 = player;
                else
                    myUICtrl.Player2 = player;
            }
            else 
            {
                if (PhotonNetwork.IsMasterClient)
                    myUICtrl.Player2 = player;
                else
                    myUICtrl.Player1 = player;
            }

        }
    }
}

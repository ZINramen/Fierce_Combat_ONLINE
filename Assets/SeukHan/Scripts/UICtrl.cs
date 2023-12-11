using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class UICtrl : MonoBehaviour
{
    public Image HP_Bar1P;
    public Image HP_Bar2P;

    public Image SpecialGauge_1P;
    public Image SpecialGauge_2P;

    public Image[] SkillIcons1P;
    public Image[] SkillIcons2P;

    private bool[] SkillActive1P = new bool[5];
    private bool[] SkillActive2P = new bool[5];

    public Entity Player1;
    public Entity Player2;

    private void Start()
    {
        StartCoroutine(CheckHP());

        for(int i = 0; i < SkillActive1P.Length; i++) { 
            SkillActive1P[i] = false;
            SkillActive2P[i] = false;
        }
    }

    //함수로 바꿔서 호출 가능하게 할 예정
    IEnumerator CheckHP()
    {
        var wait = new WaitForSecondsRealtime(0.5f);
        while(true)
        {
            HP_Bar1P.fillAmount = Player1.GetHp() / Player1.maxHP;
            HP_Bar2P.fillAmount = Player2.GetHp() / Player2.maxHP;
            yield return wait;
        }
    }

    private void CheckSkill()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            //왼쪽 1p 스킬 아이콘 활성화 SkillActive1P[]
            //오른쪽 2p 스킬 아이콘 활성화 SkillActive2P[]
        }
        else
        {
            //오른쪽 1p 스킬 아이콘 활성화 SkillActive1P[]
            //왼쪽 2p 스킬 아이콘 활성화 SkillActive2P[]
        }
    }
}
